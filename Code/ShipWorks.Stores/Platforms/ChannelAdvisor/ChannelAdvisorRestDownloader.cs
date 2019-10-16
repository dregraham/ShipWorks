﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Recovery;
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
        private readonly ILog log;
        private readonly IChannelAdvisorRestClient restClient;
        private readonly Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory;
        private readonly string refreshToken;
        private readonly ISqlAdapterRetry sqlAdapter;
        private IEnumerable<ChannelAdvisorDistributionCenter> distributionCenters;
        private readonly ChannelAdvisorStoreEntity caStore;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
            "The parameters are dependencies that were already part of the downloader, but now they are explicit")]
        public ChannelAdvisorRestDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IChannelAdvisorRestClient restClient,
            IEncryptionProviderFactory encryptionProviderFactory,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory,
            Func<Type, ILog> createLogger) :
            base(store, storeTypeManager.GetType(store))
        {
            this.restClient = restClient;
            this.orderLoaderFactory = orderLoaderFactory;

            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "ChannelAdvisorRestDownloader.Download");
            caStore = Store as ChannelAdvisorStoreEntity;
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

                ChannelAdvisorOrderResult ordersResult = restClient.GetOrders(refreshToken);

                string previousLink = String.Empty;
                double daysback = 30;

                if (Store.InitialDownloadDays.HasValue && Store.InitialDownloadDays > 30)
                {
                    daysback = Store.InitialDownloadDays ?? 30;
                }

                var oldestDownload = DateTime.UtcNow.AddDays(-daysback);

                Progress.Detail = $"Downloading orders...";

                while (ordersResult?.Orders?.Any() ?? false)
                {
                    // This is a work-around for a bug in ChannelAdvisor where sometimes they would continue to send us
                    // the same "next link" causing ShipWorks to download forever
                    if (ordersResult.OdataNextLink == previousLink)
                    {
                        break;
                    }

                    previousLink = ordersResult.OdataNextLink;

                    if (!await ProcessOrders(ordersResult, previousLink).ConfigureAwait(false))
                    {
                        break;
                    }

                    // Don't download orders older than oldestDownload. With each download, we will be marking one page of old shipped orders
                    // as exported, so eventually all of the orders will be marked
                    ordersResult = string.IsNullOrEmpty(ordersResult.OdataNextLink) || ordersResult.Orders.FirstOrDefault()?.CreatedDateUtc < oldestDownload
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

        private async Task<bool> ProcessOrders(ChannelAdvisorOrderResult ordersResult, string previousLink)
        {
            foreach (ChannelAdvisorOrder caOrder in ordersResult.Orders)
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Skip any orders that haven't been paid yet. We do this instead of filtering in the request
                // because filtering slows down the download significantly
                if (!caOrder.PaymentStatus?.Equals("Cleared", StringComparison.OrdinalIgnoreCase) == true)
                {
                    continue;
                }

                DownloadOtherLineItems(caOrder);

                List<ChannelAdvisorProduct> caProducts = DownloadChannelAdvisorProducts(caOrder);

                await LoadOrder(caOrder, caProducts).ConfigureAwait(false);

                // If the order has already been shipped, or is an FBA order, mark it as exported so we don't re-download it
                if (caOrder.ShippingStatus?.Equals("Shipped", StringComparison.OrdinalIgnoreCase) == true ||
                    caOrder.DistributionCenterTypeRollup?.Equals("ExternallyManaged", StringComparison.OrdinalIgnoreCase) == true)
                {
                    restClient.MarkOrderExported(caOrder.ID, refreshToken);
                }
            }

            return true;
        }

        /// <summary>
        /// Loads all order items, gets pages if required.
        /// </summary>
        private void DownloadOtherLineItems(ChannelAdvisorOrder caOrder)
        {
            int maxItems = 20;
            List<ChannelAdvisorOrderItem> caOrderItems = caOrder.Items.ToList();
            while (caOrderItems.Count == maxItems)
            {
                string nextToken =
                    $"https://api.channeladvisor.com/v1/Orders({caOrder.ID})/Items?$skip={caOrderItems.Count}&$expand=FulfillmentItems";

                ChannelAdvisorOrderItemsResult nextPage = restClient.GetOrderItems(
                    nextToken,
                    refreshToken);

                caOrderItems.AddRange(nextPage.OrderItems);
                maxItems += 20;
            }

            caOrder.Items = caOrderItems;
        }

        /// <summary>
        /// Download product details from ChannelAdvisor
        /// </summary>
        private List<ChannelAdvisorProduct> DownloadChannelAdvisorProducts(ChannelAdvisorOrder caOrder)
        {
            // Get the products for the order to pass into the loader
            List<ChannelAdvisorProduct> caProducts =
                caOrder.Items
                    .Select(item => restClient.GetProduct(item.ProductID, refreshToken))
                    .Where(p => p != null).ToList();
            return caProducts;
        }

        /// <summary>
        /// Updates local copy of Distribution Centers
        /// </summary>
        private void UpdateDistributionCenters()
        {
            List<ChannelAdvisorDistributionCenter> refreshedDistributionCenters = new List<ChannelAdvisorDistributionCenter>();

            ChannelAdvisorDistributionCenterResponse response = restClient.GetDistributionCenters(refreshToken);

            while (response?.DistributionCenters?.Any() ?? false)
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
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

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