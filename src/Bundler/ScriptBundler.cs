﻿using Bundler.Compression;
using Bundler.Extensions;
using Bundler.Helpers;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Bundler {

    /// <summary>
    /// The JavaScript cruncher.
    /// </summary>
    public class ScriptBundler : BundlerBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundler"/> class.
        /// </summary>
        /// <param name="options">
        /// The options containing instructions for the cruncher.
        /// </param>
        /// <param name="context">
        /// The current context.
        /// </param>
        public ScriptBundler(BundlerOptions options, HttpContext context)
            : base(options, context) {
        }

        /// <summary>
        /// Minifies the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>
        /// The minified resource.
        /// </returns>
        public override string Minify(string resource) {
            JavaScriptMinifier minifier;

            if (this.Options.Minify) {
                minifier = new JavaScriptMinifier {
                    VariableMinification = VariableMinification.LocalVariablesAndFunctionArguments
                };
            } else {
                minifier = new JavaScriptMinifier {
                    VariableMinification = VariableMinification.None,
                    PreserveFunctionNames = true,
                    RemoveWhiteSpace = false
                };
            }

            return minifier.Minify(resource);
        }

        /// <summary>
        /// Loads the local file.
        /// </summary>
        /// <param name="file">The file to load.</param>
        /// <returns>
        /// The contents of the local file as a string.
        /// </returns>
        protected override async Task<string> LoadLocalFileAsync(string file) {
            string contents = await base.LoadLocalFileAsync(file);

            contents = this.PreProcessInput(contents, file);

            // Cache if applicable.
            this.AddFileMonitor(file, contents);

            return contents;
        }

    }
}