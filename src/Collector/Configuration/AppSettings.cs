namespace Collector.Configuration
{
    using System;

    /// <summary>
    /// The application settings
    /// </summary>
    public sealed class AppSettings
    {
        /// <summary>
        /// Gets or sets the max worker count. Default is 10.
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = 10;

        /// <summary>
        /// Gets or sets the service base URI
        /// </summary>
        public Uri AutoScaleProducerBaseUri { get; set; }

        /// <summary>
        /// Gets or sets the timeout for polling hte service. Default is 1 second.
        /// </summary>
        public TimeSpan PollingTimeout { get; set; } = TimeSpan.FromSeconds(1);
    }
}