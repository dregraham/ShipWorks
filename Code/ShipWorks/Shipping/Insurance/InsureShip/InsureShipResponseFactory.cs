using log4net;

namespace ShipWorks.Shipping.Insurance.InsureShip
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
            return new InsureShipmentResponse(insureShipRequestBase, LogManager.GetLogger(typeof(InsureShipmentResponse)));
        }
    }
}
