using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Service that uses ShipEngine for tracking
    /// </summary>
    public interface IShipEngineTrackingService
    {
        /// <summary>
        /// Attempts to track a shipengine shipment
        /// </summary>
        Task<TrackingResult> TrackShipment(ShipmentEntity shipment, ApiLogSource apiLogSource, string engineCarrierCode, string failedOrNoResultsSummary);
    }
}
