using System;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Downloader for Walmart
    /// </summary>
    [KeyedComponent(typeof(StoreDownloader), StoreTypeCode.Walmart, ExternallyOwned = true)]
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
        public WalmartDownloader(StoreEntity store,
            IWalmartWebClient walmartWebClient,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            IWalmartOrderLoader walmartOrderLoader,
            IDateTimeProvider dateTimeProvider,
            Func<Type, ILog> logFactory)
            : base(store)
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
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                ordersListType ordersList = GetFirstBatch();
                int totalOrders = ordersList.meta.totalCount;

                if (totalOrders == 0)
                {
                    Progress.Detail = "No orders to download.";
                    return;
                }

                Progress.Detail = $"Downloading {totalOrders} orders...";

                while (ordersList?.elements?.Any() ?? false)
                {
                    LoadOrders(ordersList);

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
        private ordersListType GetFirstBatch()
        {
            DateTime startTime = GetOrderDateStartingPoint();
            return walmartWebClient.GetOrders(walmartStore, startTime);
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
        private void LoadOrders(ordersListType ordersList)
        {
            foreach (Order downloadedOrder in ordersList.elements)
            {
                LoadOrder(downloadedOrder);
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        private void LoadOrder(Order downloadedOrder)
        {
            // Check if it has been canceled
            if (Progress.IsCancelRequested)
            {
                return;
            }

            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            // See remarks in WalmartOrderIdentifier for why we use this vs OrderNumberIdentifier
            GenericResult<OrderEntity> result = InstantiateOrder(new WalmartOrderIdentifier(downloadedOrder.purchaseOrderId));
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", downloadedOrder.purchaseOrderId, result.Message);
                return;
            }

            WalmartOrderEntity orderToSave = (WalmartOrderEntity) result.Value;

            walmartOrderLoader.LoadOrder(downloadedOrder, orderToSave);

            // Save the downloaded order
            sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderToSave));
        }

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to. We then compare that with the
        /// date calculated from DownloadModifiedNumberOfDaysBack and send the earlier of the two dates.
        /// </summary>
        protected new DateTime GetOrderDateStartingPoint()
        {
            DateTime? defaultStartingPoint = base.GetOrderDateStartingPoint();
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
