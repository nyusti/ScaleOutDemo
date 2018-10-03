namespace Collector.Configuration
{
    using System;

    public sealed class AppSettings
    {
        public int MaxDegreeOfParallelism { get; set; } = 10;

        public Uri AutoScaleProducerBaseUri { get; set; }

        public TimeSpan PollingTimeout { get; set; } = TimeSpan.FromSeconds(1);
    }
}