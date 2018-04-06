using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View model for the archive manager
    /// </summary>
    public interface IArchiveManagerViewModel
    {
        /// <summary>
        /// Perform an archive now
        /// </summary>
        ICommand ArchiveNow { get; }

        /// <summary>
        /// Is the application busy
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Show the manager window
        /// </summary>
        /// <returns></returns>
        Task<Unit> ShowManager();
    }
}
