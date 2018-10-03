using System.Threading;
using System.Threading.Tasks;

namespace Collector
{
    public interface IWorkerService
    {
        Task ProcessAsync(CancellationToken cancellationToken);
    }
}