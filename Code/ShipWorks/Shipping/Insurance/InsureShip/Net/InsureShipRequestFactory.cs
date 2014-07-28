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
        /// Creates the submit claim request.
        /// </summary>
        public InsureShipRequestBase CreateSubmitClaimRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            return new InsureShipSubmitClaimRequest(shipmentEntity, insureShipAffiliate);
        }
    }
}
