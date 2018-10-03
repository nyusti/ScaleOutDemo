namespace Collector
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class WorkerService : IWorkerService
    {
        private readonly IAutoScaleProducerClient autoScaleProducerClient;

        public WorkerService(IAutoScaleProducerClient autoScaleProducerClient)
        {
            this.autoScaleProducerClient = autoScaleProducerClient ?? throw new ArgumentNullException(nameof(autoScaleProducerClient));
        }

        public virtual async Task ProcessAsync(CancellationToken cancellationToken)
        {
            do
            {
                var res = await this.autoScaleProducerClient.GetWaitTimeAsync(cancellationToken).ConfigureAwait(false);
                if (res != null)
                {
                    Console.WriteLine($"Worker working for {res}");
                    // simulate processing
                    await Task.Delay(res.Value, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    return;
                }
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}