using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    public class AmazonCreateShipmentRequest : IAmazonShipmentRequest
    {
        private readonly IOrderManager orderManager;
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonShipmentRequestDetailsFactory requestFactory;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateShipmentRequest(IOrderManager orderManager, 
            IAmazonShippingWebClient webClient, 
            IAmazonShipmentRequestDetailsFactory requestFactory, 
            IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.orderManager = orderManager;
            this.webClient = webClient;
            this.requestFactory = requestFactory;
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

            orderManager.PopulateOrderDetails(shipment);
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;
            
            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, order);
            IAmazonMwsWebClientSettings amazonSettings = settingsFactory.Create(shipment.Amazon);

            if (order == null)
            {
                throw new ShippingException("Amazon shipping can only be used for Amazon orders");
            }
       
            // Send a max of $100 in insured value for carriers who aren't Stamps.  Send $0 for Stamps
            requestDetails.ShippingServiceOptions.DeclaredValue.Amount =
                !shipment.Amazon.CarrierName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase) ? 
                Math.Min(shipment.Amazon.InsuranceValue, 100m) : 
                0;

            CreateShipmentResponse createShipmentRresponse = webClient.CreateShipment(requestDetails, amazonSettings, shipment.Amazon.ShippingServiceID);

            return createShipmentRresponse.CreateShipmentResult.AmazonShipment;
        }
    }
}