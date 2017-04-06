﻿using System;
using System.Configuration;

namespace Bundler.Configuration {

    /// <summary>
    /// Represents a CruncherSecuritySection within a configuration file.
    /// </summary>
    public class BundlerSecuritySection : ConfigurationSection {
        /// <summary>
        /// Gets or sets a value indicating whether the current application is allowed download remote files.
        /// </summary>
        /// <value><see langword="true"/> if the current application is allowed download remote files; otherwise, <see langword="false"/>.</value>
        [ConfigurationProperty("allowRemoteDownloads", DefaultValue = false, IsRequired = true)]
        public bool AllowRemoteDownloads {
            get { return (bool)this["allowRemoteDownloads"]; }
            set { this["allowRemoteDownloads"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum allowed remote file timeout in milliseconds for the application.
        /// </summary>
        /// <value>The maximum number of days to store an image in the cache.</value>
        /// <remarks>Defaults to 30000 (30 seconds) if not set.</remarks>
        [ConfigurationProperty("timeout", DefaultValue = "300000", IsRequired = true)]
        public int Timeout {
            get {
                return (int)this["timeout"];
            }

            set {
                this["timeout"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed remote file size in bytes for the application.
        /// </summary>
        /// <value>The maximum number of days to store an image in the cache.</value>
        /// <remarks>Defaults to 524288 (512kb) if not set.</remarks>
        [ConfigurationProperty("maxBytes", DefaultValue = "524288", IsRequired = true)]
        public int MaxBytes {
            get {
                return (int)this["maxBytes"];
            }

            set {
                this["maxBytes"] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Bundler.Configuration.CruncherSecuritySection.WhiteListElementCollection"/>.
        /// </summary>
        /// <value>The <see cref="T:Bundler.Configuration.CruncherSecuritySection.WhiteListElementCollection"/>.</value>
        [ConfigurationProperty("whiteList", IsRequired = true)]
        public WhiteListElementCollection WhiteList {
            get {
                object o = this["whiteList"];
                return o as WhiteListElementCollection;
            }
        }

        /// <summary>
        /// Retrieves the security configuration section from the current application configuration. 
        /// </summary>
        /// <returns>The cache configuration section from the current application configuration.</returns>
        public static BundlerSecuritySection GetConfiguration() {
            BundlerSecuritySection cruncherProcessingSection = ConfigurationManager.GetSection("cruncher/security") as BundlerSecuritySection;

            if (cruncherProcessingSection != null) {
                return cruncherProcessingSection;
            }

            return new BundlerSecuritySection();
        }

        /// <summary>
        /// Represents a whitelist collection configuration element within the configuration.
        /// </summary>
        public class WhiteListElementCollection : ConfigurationElementCollection {
            /// <summary>
            /// Gets or sets the whitelist item at the given index.
            /// </summary>
            /// <param name="index">The index of the whitelist item to get.</param>
            /// <returns>The whitelist item at the given index.</returns>
            public SafeUrl this[int index] {
                get {
                    return this.BaseGet(index) as SafeUrl;
                }

                set {
                    if (this.BaseGet(index) != null) {
                        this.BaseRemoveAt(index);
                    }

                    this.BaseAdd(index, value);
                }
            }

            /// <summary>
            /// Creates a new SafeURL configuration element.
            /// </summary>
            /// <returns>
            /// A new SafeURL configuration element.
            /// </returns>
            protected override ConfigurationElement CreateNewElement() {
                return new SafeUrl();
            }

            /// <summary>
            /// Gets the element key for a specified whitelist configuration element.
            /// </summary>
            /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
            /// <returns>The element key for a specified whitelist configuration element.</returns>
            protected override object GetElementKey(ConfigurationElement element) {
                return ((SafeUrl)element).Token;
            }
        }

        /// <summary>
        /// Represents a whitelist configuration element within the configuration.
        /// </summary>
        public class SafeUrl : ConfigurationElement {
            /// <summary>
            /// Gets or sets the token of the whitelisted file.
            /// </summary>
            /// <value>The token of the whitelisted file.</value>
            [ConfigurationProperty("token", DefaultValue = "", IsRequired = true)]
            public string Token {
                get { return (string)this["token"]; }

                set { this["token"] = value; }
            }

            /// <summary>
            /// Gets or sets the url of the whitelisted file.
            /// </summary>
            /// <value>The url of the whitelisted file.</value>
            [ConfigurationProperty("url", DefaultValue = "", IsRequired = true)]
            public Uri Url {
                get { return new Uri(this["url"].ToString(), UriKind.Absolute); }

                set { this["url"] = value; }
            }
        }
    }
}
