using System.Threading;

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
        void TrackShipments(CancellationToken cancellationToken);

        /// <summary>
        /// Fetch tracking notifications from the hub and populate them
        /// </summary>
        void PopulateLatestTracking(CancellationToken cancellationToken);
    }
}