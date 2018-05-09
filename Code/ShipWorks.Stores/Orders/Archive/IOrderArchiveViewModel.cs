using System;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the archive orders dialog
    /// </summary>
    public interface IOrderArchiveViewModel
    {
        /// <summary>
        /// Get order archive details from user
        /// </summary>
        Task<DateTime> GetArchiveDateFromUser();
    }
}
