namespace Collector.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Job processor interface
    /// </summary>
    public interface IJobProcessor
    {
        /// <summary>
        /// Processes one job at a time
        /// </summary>
        /// <param name="processingTime">How longdoes the job takes to process.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Task reference.</returns>
        Task ProcessAsync(int processingTime, CancellationToken cancellationToken);
    }
}