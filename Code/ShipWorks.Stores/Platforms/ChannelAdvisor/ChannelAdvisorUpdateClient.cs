using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// A facade that calls the soap or rest Channel Advisor clients to upload shipments
    /// </summary>
    [Component]
    public class ChannelAdvisorUpdateClient : IChannelAdvisorUpdateClient
    {
        private readonly Func<ChannelAdvisorStoreEntity, IChannelAdvisorClient> soapClientFactory;
        private readonly IChannelAdvisorRestClient restClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorUpdateClient(Func<ChannelAdvisorStoreEntity, IChannelAdvisorClient> soapClientFactory, IChannelAdvisorRestClient restClient)
        {
            this.soapClientFactory = soapClientFactory;
            this.restClient = restClient;
        }

        /// <summary>
        /// Uses the soap or rest interface to update Channel Advisor shipments
        /// </summary>
        public void UploadShipmentDetails(ChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, long orderNumber)
        {
            if (string.IsNullOrWhiteSpace(store.RefreshToken))
            {
                IChannelAdvisorClient soapClient = soapClientFactory(store);
                soapClient.UploadShipmentDetails((int) orderNumber, shipment.ShippedDateUtc, shipment.ShippingCarrier, shipment.ShippingClass, shipment.TrackingNumber);
            }
            else
            {
                restClient.UploadShipmentDetails(shipment, store.RefreshToken, orderNumber.ToString());
            }
        }
    }
}