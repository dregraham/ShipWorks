using System.Reactive;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Orchestrate the order archiving process
    /// </summary>
    public interface IOrderArchiveOrchestrator
    {
        /// <summary>
        /// Start the archiving process
        /// </summary>
        Task<Unit> Archive();
    }
}
