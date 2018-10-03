namespace Collector.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Worker service interface
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Processes work from the provider until they are available
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Task reference.</returns>
        Task ProcessAsync(CancellationToken cancellationToken);
    }
}