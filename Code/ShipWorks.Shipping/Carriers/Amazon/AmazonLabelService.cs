using System;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    public class AmazonLabelService : IAmazonLabelService
    {
        private readonly IOrderManager orderManager;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;
        private readonly IAmazonShippingWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory, IOrderManager orderManager)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            orderManager.PopulateOrderDetails(shipment);
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;
            if (order == null)
            {
                throw new ShippingException("Amazon shipping can only be used for Amazon orders");
            }

            AmazonMwsWebClientSettings settings = settingsFactory.Create(shipment.Amazon);
            webClient.CreateShipment(null, settings, null);
        }
    }
}
