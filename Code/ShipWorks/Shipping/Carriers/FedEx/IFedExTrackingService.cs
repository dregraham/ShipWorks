using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Tracking service for FedEx
    /// </summary>
    public interface IFedExTrackingService
    {
        /// <summary>
        /// Tracks a FedEx shipment
        /// </summary>
        TrackingResult TrackShipment(ShipmentEntity shipment, string trackingUrl);
    }
}
