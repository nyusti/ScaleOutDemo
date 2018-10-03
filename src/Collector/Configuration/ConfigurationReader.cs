namespace Collector.Configuration
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public sealed class ConfigurationReader
    {
        private static Lazy<ConfigurationReader> singletonFactory = new Lazy<ConfigurationReader>(() => new ConfigurationReader());

        private ConfigurationReader()
        {
        }

        public static ConfigurationReader Instance => singletonFactory.Value;

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