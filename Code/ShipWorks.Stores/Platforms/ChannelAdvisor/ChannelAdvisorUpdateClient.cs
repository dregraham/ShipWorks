using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
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
        private ICombineOrderSearchProvider<long> combinedOrderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorUpdateClient(Func<ChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory, 
            IChannelAdvisorRestClient restClient, ICombineOrderSearchProvider<long> combinedOrderSearchProvider,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.soapClientFactory = soapClientFactory;
            this.restClient = restClient;
            this.combinedOrderSearchProvider = combinedOrderSearchProvider;

            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor");
        }
        
        /// <summary>
        /// Uses the soap or rest interface to update Channel Advisor shipments
        /// </summary>
        public async Task UploadShipmentDetails(ChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, IOrderEntity order)
        {
            IEnumerable<long> identifiers = await combinedOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            if (identifiers.Count() == 1 && order.IsManual)
            {
                return;
            }

            foreach (var chunk in identifiers.SplitIntoChunksOf(4))
            {
                foreach (var orderIdentifier in chunk)
                {
                    if (string.IsNullOrWhiteSpace(store.RefreshToken))
                    {
                        IChannelAdvisorSoapClient soapSoapClient = soapClientFactory(store);
                        soapSoapClient.UploadShipmentDetails((int) orderIdentifier, shipment.ShippedDateUtc, shipment.ShippingCarrier, shipment.ShippingClass, shipment.TrackingNumber);
                    }
                    else
                    {
                        string refreshToken = encryptionProvider.Decrypt(store.RefreshToken);
                        restClient.UploadShipmentDetails(shipment, refreshToken, orderIdentifier.ToString());
                    }
                }
            }
        }
    }
}