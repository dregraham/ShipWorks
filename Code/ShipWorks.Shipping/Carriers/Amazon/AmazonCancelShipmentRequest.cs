using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    public class AmazonCancelShipmentRequest : IAmazonShipmentRequest
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;
        private static readonly ILog log = LogManager.GetLogger(typeof(AmazonLabelService));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCancelShipmentRequest(IAmazonShippingWebClient webClient,
            IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
        }

        /// <summary>
        /// Submits a request to the Amazon shipping web client
        /// with the resources needed. 
        /// </summary>
        /// <returns></returns>
        public AmazonShipment Submit(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            
            IAmazonMwsWebClientSettings amazonSettings = settingsFactory.Create(shipment.Amazon);

            CancelShipmentResponse cancelShipmentRresponse = webClient.CancelShipment(amazonSettings, shipment.Amazon.AmazonUniqueShipmentID);

            return cancelShipmentRresponse.CancelShipmentResult.AmazonShipment;
        }
    }
}