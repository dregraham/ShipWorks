using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Downloader for an OdbcStoreDownloader
    /// </summary>
    public class OdbcStoreDownloader : StoreDownloader
    {
        private readonly IOdbcDownloadCommandFactory downloadCommandFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly IOdbcOrderLoader orderLoader;
        private readonly OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreDownloader"/> class.
        /// </summary>
        public OdbcStoreDownloader(StoreEntity store,
            IOdbcDownloadCommandFactory downloadCommandFactory,
            IOdbcFieldMap fieldMap,
            IOdbcOrderLoader orderLoader) : base(store)
        {
            this.downloadCommandFactory = downloadCommandFactory;
            this.fieldMap = fieldMap;
            this.orderLoader = orderLoader;
            this.store = (OdbcStoreEntity) store;

            fieldMap.Load(this.store.ImportMap);
        }

        /// <summary>
        /// Import ODBC Orders from external data source.
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        /// <exception cref="DownloadException"></exception>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Querying data source...";
            try
            {
                IOdbcCommand downloadCommand = GenerateDownloadCommand(store, trackedDurationEvent);
                trackedDurationEvent.AddProperty("Odbc.Driver", downloadCommand.Driver);

                IEnumerable<OdbcRecord> downloadedOrders = downloadCommand.Execute();
                List<IGrouping<string, OdbcRecord>> orderGroups =
                    downloadedOrders.GroupBy(o => o.RecordIdentifier).ToList();

                int orderCount = GetOrderCount(orderGroups);

                if (orderCount > 0)
                {
                    EnsureRecordIdentifiersAreNotNull(orderGroups);

                    await LoadOrders(orderGroups, orderCount).ConfigureAwait(false);
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Generates the download command based on the store entity
        /// </summary>
        private IOdbcCommand GenerateDownloadCommand(OdbcStoreEntity odbcStore, TrackedDurationEvent trackedDurationEvent)
        {
            MethodConditions.EnsureArgumentIsNotNull(odbcStore, nameof(odbcStore));

            if (store.ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime)
            {
                // Used in the case that GetOnlineLastModifiedStartingPoint returns null
                int defaultDaysBack = store.InitialDownloadDays.GetValueOrDefault(7);

                // Get the starting point and include it for telemetry
                DateTime startingPoint = GetOnlineLastModifiedStartingPoint().GetValueOrDefault(DateTime.UtcNow.AddDays(-defaultDaysBack));
                trackedDurationEvent.AddMetric("Minutes.Back", DateTime.UtcNow.Subtract(startingPoint).TotalMinutes);

                return downloadCommandFactory.CreateDownloadCommand(odbcStore, startingPoint, fieldMap);
            }

            // Use -1 to indicate that we are using the "all orders" download strategy
            trackedDurationEvent.AddMetric("Minutes.Back", -1);
            return downloadCommandFactory.CreateDownloadCommand(odbcStore, fieldMap);
        }

        /// <summary>
        /// Ensures the orders contain record identifier.
        /// </summary>
        /// <param name="orderGroups">The order groups.</param>
        /// <exception cref="DownloadException">$At least one order is missing a value in {fieldMap.RecordIdentifierSource}</exception>
        private void EnsureRecordIdentifiersAreNotNull(List<IGrouping<string, OdbcRecord>> orderGroups)
        {
            if (orderGroups.Any(groups => string.IsNullOrWhiteSpace(groups.Key)))
            {
                throw new DownloadException(
                    $"At least one order is missing a value in {fieldMap.RecordIdentifierSource}");
            }
        }

        /// <summary>
        /// Gets the order count.
        /// </summary>
        private int GetOrderCount(List<IGrouping<string, OdbcRecord>> orderGroups)
        {
            int orderCount = orderGroups.Count;

            Progress.Detail = orderCount == 0 ? "No orders to download." : $"{orderCount} orders found.";

            return orderCount;
        }

        /// <summary>
        /// Loads the order information into order entities
        /// </summary>
        private async Task LoadOrders(List<IGrouping<string, OdbcRecord>> orderGroups, int totalCount)
        {
            foreach (IGrouping<string, OdbcRecord> odbcRecordsForOrder in orderGroups)
            {
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                Progress.Detail = $"Processing order {QuantitySaved + 1}";

                OrderEntity downloadedOrder = await LoadOrder(odbcRecordsForOrder).ConfigureAwait(false);

                try
                {
                    await SaveDownloadedOrder(downloadedOrder).ConfigureAwait(false);
                }
                catch (ORMQueryExecutionException ex)
                {
                    throw new DownloadException(ex.Message, ex);
                }

                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }
        }

        /// <summary>
        /// Downloads the order.
        /// </summary>
        /// <exception cref="DownloadException">Order number not found in map.</exception>
        private async Task<OrderEntity> LoadOrder(IGrouping<string, OdbcRecord> odbcRecordsForOrder)
        {
            OdbcRecord firstRecord = odbcRecordsForOrder.First();

            fieldMap.ApplyValues(firstRecord);

            // Find the OrderNumber Entry
            IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.FindEntriesBy(OrderFields.OrderNumber).FirstOrDefault();

            if (odbcFieldMapEntry == null)
            {
                throw new DownloadException("Order number not found in map.");
            }

            // Create an order using the order number
            OrderEntity orderEntity = await InstantiateOrder(new OrderNumberIdentifier((long) odbcFieldMapEntry.ShipWorksField.Value)).ConfigureAwait(false);

            orderLoader.Load(fieldMap, orderEntity, odbcRecordsForOrder);

            return orderEntity;
        }
    }
}
