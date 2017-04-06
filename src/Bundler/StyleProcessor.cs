﻿using Bundler.Caching;
using Bundler.Configuration;
using Bundler.Extensions;
using Bundler.Helpers;
using Bundler.Postprocessors.AutoPrefixer;
using Bundler.Preprocessors;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bundler {

    /// <summary>
    /// The CSS processor for processing CSS files.
    /// </summary>
    public class StyleProcessor : ProcessorBase {
        /// <summary>
        /// Ensures processing is atomic.
        /// </summary>
        private static readonly AsyncDuplicateLock Locker = new AsyncDuplicateLock();

        /// <summary>
        /// Processes the css request and returns the result.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="minify">Whether to minify the output.</param>
        /// <param name="paths">The paths to the resources to bundle.</param>
        /// <returns>
        /// The <see cref="string"/> representing the processed result.
        /// </returns>
        public async Task<string> ProcessCssCrunchAsync(HttpContext context, bool minify, params string[] paths) {
            string combinedCSS = string.Empty;

            if (paths != null) {
                string key = string.Join(string.Empty, paths).ToMd5Fingerprint();

                using (await Locker.LockAsync(key)) {
                    combinedCSS = (string)CacheManager.GetItem(key);

                    if (string.IsNullOrWhiteSpace(combinedCSS)) {
                        StringBuilder stringBuilder = new StringBuilder();

                        BundlerOptions cruncherOptions = new BundlerOptions {
                            MinifyCacheKey = key,
                            Minify = minify,
                            CacheFiles = true,
                            AllowRemoteFiles = BundlerConfiguration.Instance.AllowRemoteDownloads,
                            RemoteFileMaxBytes = BundlerConfiguration.Instance.MaxBytes,
                            RemoteFileTimeout = BundlerConfiguration.Instance.Timeout
                        };

                        StyleBundler cssCruncher = new StyleBundler(cruncherOptions, context);

                        // Expand bundles
                        paths = ExpandBundles(cssCruncher, paths);

                        // Loop through and process each file.
                        foreach (string path in paths) {
                            // Local files.
                            if (PreprocessorManager.Instance.AllowedExtensionsRegex.IsMatch(path)) {
                                List<string> files = new List<string>();

                                // Try to get the file by absolute/relative path
                                if (!ResourceHelper.IsResourceFilenameOnly(path)) {
                                    string cssFilePath = ResourceHelper.GetFilePath(
                                        path,
                                        cruncherOptions.RootFolder,
                                        context);

                                    if (File.Exists(cssFilePath)) {
                                        files.Add(cssFilePath);
                                    }
                                } else {
                                    // Get the path from the server.
                                    // Loop through each possible directory.
                                    foreach (string cssPath in BundlerConfiguration.Instance.CSSPaths) {
                                        if (!string.IsNullOrWhiteSpace(cssPath) && cssPath.Trim().StartsWith("~/")) {
                                            DirectoryInfo directoryInfo = new DirectoryInfo(context.Server.MapPath(cssPath));

                                            if (directoryInfo.Exists) {
                                                IEnumerable<FileInfo> fileInfos =
                                                    await
                                                    directoryInfo.EnumerateFilesAsync(path, SearchOption.AllDirectories);
                                                files.AddRange(fileInfos.Select(f => f.FullName));
                                            }
                                        }
                                    }
                                }

                                if (files.Any()) {
                                    // We only want the first file.
                                    string first = files.FirstOrDefault();
                                    cruncherOptions.RootFolder = Path.GetDirectoryName(first);
                                    stringBuilder.Append(await cssCruncher.CrunchAsync(first));
                                }
                            } else {
                                // Remote files.
                                string remoteFile = this.GetUrlFromToken(path).ToString();
                                stringBuilder.Append(await cssCruncher.CrunchAsync(remoteFile));
                            }
                        }

                        combinedCSS = stringBuilder.ToString();

                        // Apply autoprefixer
                        combinedCSS = cssCruncher.AutoPrefix(combinedCSS, BundlerConfiguration.Instance.AutoPrefixerOptions);

                        if (minify) {
                            combinedCSS = cssCruncher.Minify(combinedCSS);
                        }

                        this.AddItemToCache(key, combinedCSS, cssCruncher.FileMonitors);
                    }
                }
            }

            return combinedCSS;
        }
    }
}
