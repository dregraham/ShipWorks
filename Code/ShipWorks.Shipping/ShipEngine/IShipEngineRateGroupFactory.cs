using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.ShipEngine
{   
    /// <summary>
    /// Factory for creating a RateGroup from a ShipEngine rate response
    /// </summary>
    public interface IShipEngineRateGroupFactory
    {
        /// <summary>
        /// Creates a RateGroup from the given RateShipmentResponse
        /// </summary>
        /// <param name="rateResponse">The rate response from ShipEngine</param>
        /// <param name="shipmentType">Shipment type for the RateGroup</param>
        RateGroup Create(RateShipmentResponse rateResponse, ShipmentTypeCode shipmentType);
    }
}
