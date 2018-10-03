namespace Collector.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Service client interface
    /// </summary>
    public interface IAutoScaleProducerClient
    {
        /// <summary>
        /// GEts the next wait time form the provider
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The wait time if there is one. Null if no new item on the provider.</returns>
        Task<int?> GetWaitTimeAsync(CancellationToken cancellationToken);
    }
}