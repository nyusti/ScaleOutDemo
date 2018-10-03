namespace Collector.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Mock job processor implementation
    /// </summary>
    public sealed class MockJobProcessor : IJobProcessor
    {
        /// <summary>
        /// Simulates the work by a simple wait
        /// </summary>
        /// <param name="processingTime">The processing time to wait</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Task reference.</returns>
        public Task ProcessAsync(int processingTime, CancellationToken cancellationToken)
        {
            if (processingTime < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(processingTime), processingTime, "The value must be greater of equal than -1");
            }

            return Task.Delay(processingTime, cancellationToken);
        }
    }
}