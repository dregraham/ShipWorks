using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Interface for creating carrier specific RateShipmentRequests
    /// </summary>
    public interface ICarrierRateShipmentRequestFactory
    {
        /// <summary>
        /// Creates a RateShipmentRequest with carrier specific details from the given shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        RateShipmentRequest Create(ShipmentEntity shipment);
    }
}
