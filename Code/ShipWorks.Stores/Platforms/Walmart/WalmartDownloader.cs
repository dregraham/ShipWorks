﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Downloader for Walmart
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Walmart)]
    public class WalmartDownloader : StoreDownloader
    {
        private readonly IWalmartWebClient walmartWebClient;
        private readonly IWalmartOrderLoader walmartOrderLoader;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlAdapterRetry sqlAdapter;
        private readonly ILog log;
        private readonly WalmartStoreEntity walmartStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartDownloader"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
            "These parameters are dependencies the store downloader already had, they're just explicit now")]
        public WalmartDownloader(StoreEntity store,
            IWalmartWebClient walmartWebClient,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            IWalmartOrderLoader walmartOrderLoader,
            IDateTimeProvider dateTimeProvider,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory,
            IStoreTypeManager storeTypeManager,
            Func<Type, ILog> logFactory)
            : base(store, storeTypeManager.GetType(store), configurationData, sqlAdapterFactory)
        {
            this.walmartWebClient = walmartWebClient;
            this.walmartOrderLoader = walmartOrderLoader;
            this.dateTimeProvider = dateTimeProvider;
            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "WalmartDownloader.Download");
            log = logFactory(GetType());

            walmartStore = store as WalmartStoreEntity;
        }

        /// <summary>
        /// Must be implemented by derived types to do the actual download
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
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

                ordersListType ordersList = await GetFirstBatch().ConfigureAwait(false);
                int totalOrders = ordersList.meta.totalCount;

                if (totalOrders == 0)
                {
                    Progress.Detail = "No orders to download.";
                    return;
                }

                Progress.Detail = $"Downloading {totalOrders} orders...";

                while (ordersList?.elements?.Any() ?? false)
                {
                    await LoadOrders(ordersList).ConfigureAwait(false);

                    ordersList = GetNextBatch(ordersList);
                }

                Progress.PercentComplete = 100;
                Progress.Detail = "Done";
            }
            catch (WalmartException ex)
            {
                throw new DownloadException(ex.Message);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the first batch.
        /// </summary>
        private async Task<ordersListType> GetFirstBatch()
        {
            DateTime startDate = await GetWalmartOrderDateStartingPoint();
            return walmartWebClient.GetOrders(walmartStore, startDate);
        }

        /// <summary>
        /// Gets the next batch.
        /// </summary>
        private ordersListType GetNextBatch(ordersListType ordersList)
        {
            string nextCursor = ordersList.meta.nextCursor;
            if (nextCursor.IsNullOrWhiteSpace())
            {
                return null;
            }

            return walmartWebClient.GetOrders(walmartStore, nextCursor);
        }

        /// <summary>
        /// Saves the orders.
        /// </summary>
        /// <param name="ordersList">The orders list.</param>
        private async Task LoadOrders(ordersListType ordersList)
        {
            foreach (Order downloadedOrder in ordersList.elements)
            {
                await LoadOrder(downloadedOrder).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        private async Task LoadOrder(Order downloadedOrder)
        {
            // Check if it has been canceled
            if (Progress.IsCancelRequested)
            {
                return;
            }

            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            // See remarks in WalmartOrderIdentifier for why we use this vs OrderNumberIdentifier
            GenericResult<OrderEntity> result = await InstantiateOrder(new WalmartOrderIdentifier(downloadedOrder.purchaseOrderId)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", downloadedOrder.purchaseOrderId, result.Message);
                return;
            }

            WalmartOrderEntity orderToSave = (WalmartOrderEntity) result.Value;

            walmartOrderLoader.LoadOrder(downloadedOrder, orderToSave);

            // Save the downloaded order
            await sqlAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(orderToSave)).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to. We then compare that with the
        /// date calculated from DownloadModifiedNumberOfDaysBack and send the earlier of the two dates.
        /// </summary>
        protected async Task<DateTime> GetWalmartOrderDateStartingPoint()
        {
            DateTime? defaultStartingPoint = await base.GetOrderDateStartingPoint();
            DateTime modifiedDaysBack = dateTimeProvider.UtcNow.AddDays(-walmartStore.DownloadModifiedNumberOfDaysBack);

            if (defaultStartingPoint == null || modifiedDaysBack < defaultStartingPoint.Value)
            {
                return modifiedDaysBack;
            }
            else
            {
                return defaultStartingPoint.Value;
            }
        }
    }
}
