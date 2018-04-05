using System.Windows.Forms.Integration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Control to show an archive notification
    /// </summary>
    public interface IArchiveNotification
    {
        /// <summary>
        /// Add to the given host
        /// </summary>
        void AddTo(ElementHost host, IArchiveNotificationViewModel viewModel);
    }
}