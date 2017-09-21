using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    [Component]
    public class AmazonCancelShipmentRequest : IAmazonCancelShipmentRequest
    {
        private readonly IAmazonShippingWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCancelShipmentRequest(IAmazonShippingWebClient webClient)
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