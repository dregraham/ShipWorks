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
        /// Creates the void policy response.
        /// </summary>
        public IInsureShipResponse CreateVoidPolicyResponse(InsureShipRequestBase insureShipRequestBase)
        {
            throw new System.NotImplementedException();
        }
    }
}
