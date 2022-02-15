using System.Threading;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Class to send tracking information and receive 
    /// </summary>
    public interface IPlatformShipmentTracker
    {
        /// <summary>
        /// Get all the shipments and send them up to hub for tracking
        /// </summary>
        Task TrackShipments(CancellationToken cancellationToken);

        /// <summary>
        /// Fetch tracking notifications from the hub and populate them
        /// </summary>
        Task PopulateLatestTracking(CancellationToken cancellationToken);
    }
}