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
        private readonly Func<StoreEntity, IChannelAdvisorStoreType> getStoreType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorUpdateClient(Func<ChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory, 
            IChannelAdvisorRestClient restClient, Func<StoreEntity, IChannelAdvisorStoreType> getStoreType,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.soapClientFactory = soapClientFactory;
            this.restClient = restClient;
            this.getStoreType = getStoreType;

            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor");
        }

        /// <summary>
        /// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
        /// combined orders.
        /// </summary>
        public async Task<IEnumerable<int>> GetOrderIdentifiers(IChannelAdvisorStoreType storeType, IOrderEntity order)
        {
            return order.CombineSplitStatus == CombineSplitStatusType.Combined ?
                await storeType.GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
                new[] { storeType.GetOnlineOrderIdentifier(order) };
        }

        /// <summary>
        /// Uses the soap or rest interface to update Channel Advisor shipments
        /// </summary>
        public async Task UploadShipmentDetails(ChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, IOrderEntity order)
        {
            IChannelAdvisorStoreType storeType = getStoreType(store);
            IEnumerable<int> identifiers = await GetOrderIdentifiers(storeType, order).ConfigureAwait(false);

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
                        soapSoapClient.UploadShipmentDetails(orderIdentifier, shipment.ShippedDateUtc, shipment.ShippingCarrier, shipment.ShippingClass, shipment.TrackingNumber);
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