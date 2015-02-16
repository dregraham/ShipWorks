using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the Express1/Stamps API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    public class Express1UspsScanFormGateway : UspsScanFormGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsScanFormGateway(IUspsWebClient webClient)
            : base(webClient)
        {
            InvalidCarrierMessage = "An attempt to create an Express1 SCAN form was made for a carrier other than Express1.";
            InvalidShipmentMessage = "Cannot create a Express1 SCAN form for a shipment that was not shipped with Express1.";
        }
    }
}