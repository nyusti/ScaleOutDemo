namespace Collector
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAutoScaleProducerClient
    {
        Task<int?> GetWaitTimeAsync(CancellationToken cancellationToken);
    }
}