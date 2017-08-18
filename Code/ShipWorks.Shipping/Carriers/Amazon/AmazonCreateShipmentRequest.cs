using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    [Component]
    public class AmazonCreateShipmentRequest : IAmazonCreateShipmentRequest
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
            IAmazonOrder order = shipment.Order as IAmazonOrder;

            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, order);
            IAmazonMwsWebClientSettings amazonSettings = settingsFactory.Create(shipment.Amazon);

            if (order == null)
            {
                throw new ShippingException("Amazon shipping can only be used for Amazon orders");
            }

            // Send a max of $100 in insured value for carriers who aren't Stamps.  Send $0 for Stamps
            requestDetails.ShippingServiceOptions.DeclaredValue.Amount =
                IsCarrierUsps(shipment) ?
                0 :
                Math.Min(shipment.Amazon.InsuranceValue, 100M);

            return webClient.CreateShipment(requestDetails, amazonSettings, shipment.Amazon.ShippingServiceID);
        }

        /// <summary>
        /// Is the carrier USPS
        /// </summary>
        /// <remarks>
        /// Amazon used to send STAMPS_DOT_COM, but changed to USPS. We're checking both in case they change back.
        /// </remarks>
        private static bool IsCarrierUsps(ShipmentEntity shipment)
        {
            return shipment.Amazon.CarrierName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase) ||
                shipment.Amazon.CarrierName.Equals("USPS", StringComparison.OrdinalIgnoreCase);
        }
    }
}