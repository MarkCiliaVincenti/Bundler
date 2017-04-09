﻿using Bundler.Postprocessors.AutoPrefixer;
using JavaScriptEngineSwitcher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bundler.Configuration {

    /// <summary>
    /// Configuration options.
    /// </summary>
    public class BundlerConfig {

        private static BundlerConfig _current;

        private readonly string _physicalFilesPath;
        private readonly int _physicalFilesDaysBeforeRemoveExpired;
        private readonly AutoPrefixerOptions _autoPrefixerOptions;

        /// <summary>
        /// Initialize a new instance of the <see cref="BundlerConfig"/> class.
        /// </summary>
        public BundlerConfig() {
            _physicalFilesPath = "~/bundles";
            _physicalFilesDaysBeforeRemoveExpired = 7;
            _autoPrefixerOptions = new AutoPrefixerOptions() {
                Enabled = true,
                Browsers = new string[] { "last 2 versions" }
            };
        }

        /// <summary>
        /// Configure Bundler with default options.
        /// </summary>
        public static void Configure() {
            Configure(new BundlerConfig());
        }

        /// <summary>
        /// Configure Bundler to use the specifified configuration.
        /// </summary>
        /// <param name="config"></param>
        public static void Configure(BundlerConfig config) {
            _current = config;
        }

        /// <summary>
        /// Gets the current instance of the <see cref="BundlerConfig"/> class.
        /// </summary>
        public static BundlerConfig Instance {
            get {
                return _current ?? new BundlerConfig();
            }
        }

        /// <summary>
        /// Gets the directory's path where to store physical files
        /// </summary>
        public string PhysicalFilesPath {
            get {
                return _physicalFilesPath;
            }
        }

        /// <summary>
        /// Gets the number of days to keep physical files
        /// </summary>
        public int PhysicalFilesDaysBeforeRemoveExpired {
            get {
                return _physicalFilesDaysBeforeRemoveExpired;
            }
        }
        /// <summary>
        /// Gets the auto prefixer options.
        /// </summary>
        public AutoPrefixerOptions AutoPrefixerOptions {
            get {
                return _autoPrefixerOptions;
            }
        }
    }
}
