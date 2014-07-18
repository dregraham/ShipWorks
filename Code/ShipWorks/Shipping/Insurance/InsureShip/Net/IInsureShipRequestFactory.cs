using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Interface for a InsureShip Request Factory
    /// </summary>
    public interface IInsureShipRequestFactory
    {
        /// <summary>
        /// Creates the insure shipment request.
        /// </summary>
        InsureShipRequestBase CreateInsureShipmentRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);
    }
}
