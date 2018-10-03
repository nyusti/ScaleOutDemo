namespace Collector.Configuration
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///Default configuration reader
    /// </summary>
    public sealed class ConfigurationReader
    {
        private static Lazy<ConfigurationReader> singletonFactory = new Lazy<ConfigurationReader>(() => new ConfigurationReader());

        /// <summary>
        /// Prevents creating instance from the reader
        /// </summary>
        private ConfigurationReader()
        {
        }

        /// <summary>
        /// Default reader instance
        /// </summary>
        public static ConfigurationReader Instance => singletonFactory.Value;

        /// <summary>
        /// Bound settings from the application configuration file
        /// </summary>
        public AppSettings Settings = GetSettings();

        private static AppSettings GetSettings()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            var appSettings = new AppSettings();
            config.Bind(appSettings);

            return appSettings;
        }
    }
}