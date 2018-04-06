using System.Reactive;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View model for the archive manager
    /// </summary>
    public interface IArchiveManagerViewModel
    {
        /// <summary>
        /// Show the manager window
        /// </summary>
        /// <returns></returns>
        Task<Unit> ShowManager();
    }
}
