﻿using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Class for handling requests to the Amazon shipping api
    /// </summary>
    [Component]
    public class AmazonCancelShipmentRequest : IAmazonCancelShipmentRequest
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

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

            CancelShipmentResponse cancelShipmentRresponse = webClient.CancelShipment(amazonSettings, shipment.Amazon);

            return cancelShipmentRresponse.CancelShipmentResult.AmazonShipment;
        }
    }
}