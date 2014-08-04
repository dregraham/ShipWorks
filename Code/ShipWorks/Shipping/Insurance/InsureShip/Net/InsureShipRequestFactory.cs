using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;

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

        /// <summary>
        /// Creates the void policy request.
        /// </summary>
        public InsureShipRequestBase CreateVoidPolicyRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            return new InsureShipVoidPolicyRequest(shipmentEntity, insureShipAffiliate);
        }

        /// <summary>
        /// Creates the claim status request.
        /// </summary>
        public InsureShipRequestBase CreateClaimStatusRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            throw new System.NotImplementedException();
        }
    }
}
