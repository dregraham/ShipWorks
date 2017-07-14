using System;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for downloading orders from ChannelAdvisor via their REST api
    /// </summary>
    [Component]
    public class ChannelAdvisorRestDownloader : StoreDownloader, IChannelAdvisorRestDownloader
    {
        private readonly IChannelAdvisorRestClient restClient;
        private readonly string refreshToken;
        private ChannelAdvisorStoreEntity caStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestDownloader(StoreEntity store, IChannelAdvisorRestClient restClient, IEncryptionProviderFactory encryptionProviderFactory) : base(store)
        {
            this.restClient = restClient;

            caStore = Store as ChannelAdvisorStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(caStore, "ChannelAdvisor Store");
            refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor")
                .Decrypt(caStore.RefreshToken);
        }
        
        /// <summary>
        /// Download orders from ChannelAdvisor REST
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            DateTime start = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-30);
            string token = restClient.GetAccessToken(refreshToken);
            
            while (true)
            {
                
                ChannelAdvisorOrderResult result = restClient.GetOrders(start, token);
                
                if (result.Orders.None())
                {
                    break;
                }
            }

        }
    }
}