using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    [Component]
    public class AmazonSFPCancelShipmentRequest : IAmazonSFPCancelShipmentRequest
    {
        private readonly IAmazonSFPShippingWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPCancelShipmentRequest(IAmazonSFPShippingWebClient webClient)
        {
            this.webClient = webClient;
        }

        /// <summary>
        /// Submits a request to the Amazon shipping web client
        /// with the resources needed.
        /// </summary>
        /// <returns></returns>
        public AmazonShipment Submit(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            
            CancelShipmentResponse cancelShipmentRresponse = webClient.CancelShipment(shipment.Amazon);

            return cancelShipmentRresponse.CancelShipmentResult.AmazonShipment;
        }
    }
}