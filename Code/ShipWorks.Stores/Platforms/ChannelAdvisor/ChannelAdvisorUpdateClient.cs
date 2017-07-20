using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
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
        private readonly Func<ChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory;
        private readonly IChannelAdvisorRestClient restClient;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorUpdateClient(Func<ChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory, 
            IChannelAdvisorRestClient restClient,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.soapClientFactory = soapClientFactory;
            this.restClient = restClient;

            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor");
        }

        /// <summary>
        /// Uses the soap or rest interface to update Channel Advisor shipments
        /// </summary>
        public void UploadShipmentDetails(ChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, long orderNumber)
        {
            if (string.IsNullOrWhiteSpace(store.RefreshToken))
            {
                IChannelAdvisorSoapClient soapSoapClient = soapClientFactory(store);
                soapSoapClient.UploadShipmentDetails((int) orderNumber, shipment.ShippedDateUtc, shipment.ShippingCarrier, shipment.ShippingClass, shipment.TrackingNumber);
            }
            else
            {
                string refreshToken = encryptionProvider.Decrypt(store.RefreshToken);
                restClient.UploadShipmentDetails(shipment, refreshToken, orderNumber.ToString());
            }
        }
    }
}