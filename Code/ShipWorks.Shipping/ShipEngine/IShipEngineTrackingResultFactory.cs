using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating ShipWorks TrackingResult from the ShipEngine TrackingInformation
    /// </summary>
    public interface IShipEngineTrackingResultFactory
    {
        /// <summary>
        /// Creates ShipWorks TrackingResult from the ShipEngine TrackingInformation
        /// </summary>
        TrackingResult Create(TrackingInformation shipEngineTrackingInfo);
    }
}
