using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Implementation of an InsureShip Insure Shipment Request Factory
    /// </summary>
    public class InsureShipRequestFactory : IInsureShipRequestFactory
    {
        /// <summary>
        /// Creates the insure shipment request.
        /// </summary>
        public InsureShipRequestBase CreateInsureShipmentRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            return new InsureShipInsureShipmentRequest(shipmentEntity, insureShipAffiliate);
        }

        /// <summary>
        /// Creates the void policy request.
        /// </summary>
        public InsureShipRequestBase CreateVoidPolicyRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            throw new System.NotImplementedException();
        }
    }
}
