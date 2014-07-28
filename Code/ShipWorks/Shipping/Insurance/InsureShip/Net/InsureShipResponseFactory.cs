using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using log4net;

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
            // TODO: Update after concrete response class has been added/implemented
            throw new System.NotImplementedException();
        }
    }
}
