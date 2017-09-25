using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating
{
    /// <summary>
    /// A facade that calls the soap or rest Channel Advisor clients to upload shipments
    /// </summary>
    [Component]
    public class ChannelAdvisorUpdateClient : IChannelAdvisorUpdateClient
    {
        private readonly Func<IChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory;
        private readonly IChannelAdvisorRestClient restClient;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ICombineOrderNumberSearchProvider combinedOrderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorUpdateClient(Func<IChannelAdvisorStoreEntity, IChannelAdvisorSoapClient> soapClientFactory,
            IChannelAdvisorRestClient restClient, ICombineOrderNumberSearchProvider combinedOrderSearchProvider,
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
        public async Task UploadShipmentDetails(IChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, IOrderEntity order)
        {
            IEnumerable<long> identifiers = await combinedOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            if (identifiers.Count() == 1 && order.IsManual)
            {
                return;
            }

            var handler = Result.Handle<ChannelAdvisorException>();
            identifiers
                .Select(x => handler.Execute(() => PerformUpload(store, shipment, x)))
                .ThrowFailures((msg, ex) => new ChannelAdvisorException(msg, ex));
        }

        /// <summary>
        /// Perform the actual upload
        /// </summary>
        private void PerformUpload(IChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, long orderIdentifier)
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