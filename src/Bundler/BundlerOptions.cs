﻿namespace Bundler {
    /// <summary>
    /// The cruncher options.
    /// </summary>
    public class BundlerOptions {
        /// <summary>
        /// Gets or sets a value indicating whether to minify the file.
        /// </summary>
        public bool Minify { get; set; }

        /// <summary>
        /// Gets or sets the minify cache key.
        /// </summary>
        public string MinifyCacheKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to cache files.
        /// </summary>
        public bool CacheFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow remote files.
        /// </summary>
        public bool AllowRemoteFiles { get; set; }

        /// <summary>
        /// Gets or sets the remote file timeout in milliseconds.
        /// Set to 0 for no limit.
        /// </summary>
        public int RemoteFileTimeout { get; set; }

        /// <summary>
        /// Gets or sets the remote file max file size in bytes.
        /// Set to 0 for no limit.
        /// </summary>
        public int RemoteFileMaxBytes { get; set; }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        public string RootFolder { get; set; }
    }
}
