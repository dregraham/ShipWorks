using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Interface for creating carrier specific RateShipmentRequests
    /// </summary>
    public interface ICarrierShipmentRequestFactory
    {
        /// <summary>
        /// Creates a RateShipmentRequest with carrier specific details from the given shipment
        /// </summary>
        DTOs.RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment);

        /// <summary>
        /// Create a PurchaseLabelWithoutShipmentRequest
        /// </summary>
        DTOs.PurchaseLabelWithoutShipmentRequest CreatePurchaseLabelWithoutShipmentRequest(ShipmentEntity shipment);

        /// <summary>
        /// Creates a PurchaseLabelRequest with carrier specific details from the given shipment
        /// </summary>
        DTOs.PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment);
    }
}
