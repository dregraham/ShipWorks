using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the schedule archive orders dialog
    /// </summary>
    public interface IScheduleOrderArchiveViewModel
    {
        /// <summary>
        /// Get order archive details from user
        /// </summary>
        Task Show();
    }
}
