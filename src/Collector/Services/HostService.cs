namespace Collector.Services
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Client;
    using Collector.Configuration;

    /// <summary>
    /// Main worker service
    /// </summary>
    public class HostService : IWorkerService
    {
        private readonly IAutoScaleProducerClient autoScaleProducerClient;
        private readonly IJobProcessor jobProcessor;
        private int processCount;

        /// <summary>
        /// Creates an instance of the main worker service
        /// </summary>
        /// <param name="autoScaleProducerClient">Auto scale producer service client</param>
        /// <param name="jobProcessor">Job processor for handling the job if needed.</param>
        public HostService(IAutoScaleProducerClient autoScaleProducerClient, IJobProcessor jobProcessor)
        {
            this.autoScaleProducerClient = autoScaleProducerClient ?? throw new ArgumentNullException(nameof(autoScaleProducerClient));
            this.jobProcessor = jobProcessor ?? throw new ArgumentNullException(nameof(jobProcessor));
            this.processCount = 1; // 1 because of the host process
        }

        /// <inheritdoc/>
        public virtual async Task ProcessAsync(CancellationToken cancellationToken)
        {
            do
            {
                // get the first job from the provider
                var waitTime = await this.autoScaleProducerClient.GetWaitTimeAsync(cancellationToken).ConfigureAwait(false);

                if (waitTime != null)
                {
                    // there is a job to handle
                    if (this.processCount < ConfigurationReader.Instance.Settings.MaxDegreeOfParallelism)
                    {
                        // we are okay with the parallel threshold so start a new worker process
                        var workerProcess = Process.Start("dotnet", $"{Assembly.GetExecutingAssembly().Location} --worker");
                        workerProcess.EnableRaisingEvents = true;

                        // increment the worker prcess count
                        Interlocked.Increment(ref this.processCount);

                        // subscribe for the process termination
                        workerProcess.Exited += (s, o) => Interlocked.Decrement(ref this.processCount);
                    }

                    // do the current job on the host
                    Console.WriteLine($"Host working for {waitTime}");
                    await this.jobProcessor.ProcessAsync(waitTime.Value, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    // no job available so just wait a bit
                    Console.WriteLine($"Host waiting for {ConfigurationReader.Instance.Settings.PollingTimeout}");
                    await Task.Delay(ConfigurationReader.Instance.Settings.PollingTimeout, cancellationToken).ConfigureAwait(false);
                }
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}