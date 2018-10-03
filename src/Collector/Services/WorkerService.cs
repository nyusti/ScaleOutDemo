namespace Collector.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Client;

    /// <summary>
    /// Default worker service implementation
    /// </summary>
    public class WorkerService : IWorkerService
    {
        private readonly IAutoScaleProducerClient autoScaleProducerClient;
        private readonly IJobProcessor jobProcessor;

        /// <summary>
        /// Creates a new instance from the worker service
        /// </summary>
        /// <param name="autoScaleProducerClient">Auto sclae producer service client.</param>
        /// <param name="jobProcessor">The processor to handle the job if needed</param>
        public WorkerService(IAutoScaleProducerClient autoScaleProducerClient, IJobProcessor jobProcessor)
        {
            this.autoScaleProducerClient = autoScaleProducerClient ?? throw new ArgumentNullException(nameof(autoScaleProducerClient));
            this.jobProcessor = jobProcessor ?? throw new ArgumentNullException(nameof(jobProcessor));
        }

        /// <inheritdoc/>
        public virtual async Task ProcessAsync(CancellationToken cancellationToken)
        {
            do
            {
                var res = await this.autoScaleProducerClient.GetWaitTimeAsync(cancellationToken).ConfigureAwait(false);
                if (res != null)
                {
                    Console.WriteLine($"Worker working for {res}");
                    // simulate processing
                    await this.jobProcessor.ProcessAsync(res.Value, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    // no work avaialbe, just exit
                    return;
                }
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}