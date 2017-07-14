using System;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
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
        private readonly ChannelAdvisorStoreEntity caStore;
        private readonly ISqlAdapterRetry sqlAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestDownloader(StoreEntity store, IChannelAdvisorRestClient restClient, IEncryptionProviderFactory encryptionProviderFactory,
        ISqlAdapterRetryFactory sqlAdapterRetryFactory) : base(store)
        {
            this.restClient = restClient;

            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "WalmartDownloader.Download");
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
            Progress.Detail = "Checking for orders...";

            try
            {
                string token = restClient.GetAccessToken(refreshToken);
                
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                ChannelAdvisorOrderResult ordersResult = GetNextBatch(token);
                
                Progress.Detail = $"Downloading {ordersResult.ResultCount} orders...";

                while (ordersResult?.Orders?.Any() ?? false)
                {
                    foreach (ChannelAdvisorOrder caOrder in ordersResult.Orders)
                    {
                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        LoadOrder(caOrder);
                    }

                    ordersResult = GetNextBatch(token);
                }

            }
            catch (ChannelAdvisorException ex)
            {
                throw new DownloadException(ex.Message);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

            Progress.PercentComplete = 100;
            Progress.Detail = "Done";
        }

        /// <summary>
        /// Get the next batch of orders
        /// </summary>
        private ChannelAdvisorOrderResult GetNextBatch(string token)
        {
            DateTime start = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-30);
            return restClient.GetOrders(start, token);
        }

        /// <summary>
        /// Load the given ChannelAdvisor order
        /// </summary>
        private void LoadOrder(ChannelAdvisorOrder caOrder)
        {
            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            // Check if it has been canceled
            if (Progress.IsCancelRequested)
            {
                return;
            }

            ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity) InstantiateOrder(
                new OrderNumberIdentifier(caOrder.ID));

            //Order loader loads the order


            // Save the downloaded order
            sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }
    }
}