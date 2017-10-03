using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for downloading orders from ChannelAdvisor via their REST api
    /// </summary>
    [Component]
    public class ChannelAdvisorRestDownloader : StoreDownloader, IChannelAdvisorRestDownloader
    {
        private readonly ILog log;
        private readonly IChannelAdvisorRestClient restClient;
        private readonly Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory;
        private readonly string refreshToken;
        private readonly ISqlAdapterRetry sqlAdapter;
        private IEnumerable<ChannelAdvisorDistributionCenter> distributionCenters;
        private int totalOrders;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
            "The parameters are dependencies that were already part of the downloader, but now they are explicit")]
        public ChannelAdvisorRestDownloader(StoreEntity store,
            IChannelAdvisorRestClient restClient,
            IEncryptionProviderFactory encryptionProviderFactory,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory,
            Func<Type, ILog> createLogger) :
            base(store)
        {
            this.restClient = restClient;
            this.orderLoaderFactory = orderLoaderFactory;

            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "ChannelAdvisorRestDownloader.Download");
            ChannelAdvisorStoreEntity caStore = Store as ChannelAdvisorStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(caStore, "ChannelAdvisor Store");
            refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor")
                .Decrypt(caStore.RefreshToken);
            log = createLogger(GetType());
        }

        /// <summary>
        /// Download orders from ChannelAdvisor REST
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                UpdateDistributionCenters();

                DateTime start = (await GetOrderDateStartingPoint().ConfigureAwait(false)) ??
                    DateTime.UtcNow.AddDays(-30);

                ChannelAdvisorOrderResult ordersResult = restClient.GetOrders(start.AddSeconds(-2), refreshToken);
                totalOrders = ordersResult.ResultCount;

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

                        // Get the products for the order to pass into the loader
                        List<ChannelAdvisorProduct> caProducts =
                            caOrder.Items
                                .Select(item => restClient.GetProduct(item.ProductID, refreshToken))
                                .Where(p => p != null).ToList();

                        await LoadOrder(caOrder, caProducts).ConfigureAwait(false);
                    }

                    ordersResult = string.IsNullOrEmpty(ordersResult.OdataNextLink)
                        ? null
                        : restClient.GetOrders(ordersResult.OdataNextLink, refreshToken);
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
        /// Updates local copy of Distribution Centers
        /// </summary>
        private void UpdateDistributionCenters()
        {
            List<ChannelAdvisorDistributionCenter> refreshedDistributionCenters = new List<ChannelAdvisorDistributionCenter>();

            ChannelAdvisorDistributionCenterResponse response = restClient.GetDistributionCenters(refreshToken);
            
            while(response?.DistributionCenters?.Any() ?? false)
            {
                refreshedDistributionCenters.AddRange(response.DistributionCenters);

                response = string.IsNullOrEmpty(response.OdataNextLink) 
                    ? null 
                    : restClient.GetDistributionCenters(response.OdataNextLink, refreshToken);
            }

            distributionCenters = refreshedDistributionCenters;
        }

        /// <summary>
        /// Load the given ChannelAdvisor order
        /// </summary>
        private async Task LoadOrder(ChannelAdvisorOrder caOrder, List<ChannelAdvisorProduct> caProducts)
        {
            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1} of {totalOrders}...";
            Progress.PercentComplete = Math.Min(100 * QuantitySaved / totalOrders, 100);

            // Check if it has been canceled
            if (!Progress.IsCancelRequested)
            {
                // get the order instance
                GenericResult<OrderEntity> result = await InstantiateOrder(new OrderNumberIdentifier(caOrder.ID)).ConfigureAwait(false);
                if (result.Failure)
                {
                    log.InfoFormat("Skipping order '{0}': {1}.", caOrder.ID, result.Message);
                    return;
                }

                ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity) result.Value;

                // Required by order loader
                order.Store = Store;

                //Order loader loads the order
                orderLoaderFactory(distributionCenters).LoadOrder(order, caOrder, caProducts, this);

                // Save the downloaded order
                await sqlAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
            }
        }
    }
}