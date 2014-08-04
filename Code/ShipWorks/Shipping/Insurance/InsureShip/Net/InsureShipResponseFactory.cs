using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using log4net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Factory to create an InsureShipResponse
    /// </summary>
    public class InsureShipResponseFactory: IInsureShipResponseFactory
    {
        /// <summary>
        /// Creates the insure shipment response.
        /// </summary>
        public IInsureShipResponse CreateInsureShipmentResponse(InsureShipRequestBase insureShipRequestBase)
        {
            return new InsureShipInsureShipmentResponse(insureShipRequestBase, LogManager.GetLogger(typeof(InsureShipInsureShipmentResponse)));
        }

        /// <summary>
        /// Creates an instance of an IInsureShipResponse for submitting a claim to InsureShip.
        /// </summary>
        public IInsureShipResponse CreateSubmitClaimResponse(InsureShipRequestBase insureShipRequestBase)
        {
           return new InsureShipSubmitClaimResponse(insureShipRequestBase, LogManager.GetLogger(typeof(InsureShipSubmitClaimResponse)));
        }

        /// <summary>
        /// Creates the void policy response.
        /// </summary>
        public IInsureShipResponse CreateVoidPolicyResponse(InsureShipRequestBase insureShipRequestBase)
        {
            return new InsureShipVoidPolicyResponse(insureShipRequestBase, LogManager.GetLogger(typeof(InsureShipVoidPolicyResponse)));
        }

        /// <summary>
        /// Creates the claim status response.
        /// </summary>
        public IInsureShipResponse CreateClaimStatusResponse(InsureShipRequestBase insureShipRequestBase)
        {
            throw new System.NotImplementedException();
        }
    }
}
