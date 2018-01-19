using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Downloader for an OdbcStoreDownloader
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Odbc)]
    public class OdbcStoreDownloader : StoreDownloader
    {
        private readonly IOdbcDownloadCommandFactory downloadCommandFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly IOdbcOrderLoader orderLoader;
        private readonly OdbcStoreEntity store;
        private readonly ILog log;
        private readonly OdbcStoreType odbcStoreType;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreDownloader"/> class.
        /// </summary>
        public OdbcStoreDownloader(StoreEntity store,
            IOdbcDownloadCommandFactory downloadCommandFactory,
            IOdbcFieldMap fieldMap,
            IOdbcOrderLoader orderLoader,
            IOdbcDownloaderExtraDependencies extras) : base(store, extras.GetStoreType(store))
        {
            this.downloadCommandFactory = downloadCommandFactory;
            this.fieldMap = fieldMap;
            this.orderLoader = orderLoader;
            this.store = (OdbcStoreEntity) store;
            log = extras.GetLog(GetType());
            odbcStoreType = StoreType as OdbcStoreType;

            fieldMap.Load(this.store.ImportMap);
        }

        /// <summary>
        /// Download the order with matching order number for the store
        /// </summary>
        public override async Task Download(string orderNumber, long downloadID, DbConnection con)
        {
            Progress = new ProgressItem("Download single order");
            downloadLogID = downloadID;
            connection = con;
            try
            {
                // Try to find an existing order
                OrderEntity order = await FindOrder(odbcStoreType.CreateOrderIdentifier(orderNumber)).ConfigureAwait(false);
                if (order == null)
                {
                    IOdbcCommand downloadCommand = downloadCommandFactory.CreateDownloadCommand(store, orderNumber, fieldMap);
                    await Download(downloadCommand).ConfigureAwait(false);
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Import ODBC Orders from external data source.
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        /// <exception cref="DownloadException"></exception>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            if (store.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
            {
                throw new DownloadException($"The store, {store.StoreName}, is set to download orders on order search only. \r\n\r\n" +
                                            "To automatically download orders, change this store's order import settings.");
            }

            Progress.Detail = "Querying data source...";
            try
            {
                IOdbcCommand downloadCommand = await GenerateDownloadCommand(store, trackedDurationEvent);
                trackedDurationEvent.AddProperty("Odbc.Driver", downloadCommand.Driver);

                await Download(downloadCommand);
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download using the download command
        /// </summary>
        private async Task Download(IOdbcCommand downloadCommand)
        {
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

        /// <summary>
        /// Generates the download command based on the store entity
        /// </summary>
        private async Task<IOdbcCommand> GenerateDownloadCommand(OdbcStoreEntity odbcStore, TrackedDurationEvent trackedDurationEvent)
        {
            MethodConditions.EnsureArgumentIsNotNull(odbcStore, nameof(odbcStore));

            if (store.ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime)
            {
                // Used in the case that GetOnlineLastModifiedStartingPoint returns null
                int defaultDaysBack = store.InitialDownloadDays.GetValueOrDefault(7);

                // Get the starting point and include it for telemetry
                DateTime startingPoint = (await GetOnlineLastModifiedStartingPoint()).GetValueOrDefault(DateTime.UtcNow.AddDays(-defaultDaysBack));
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

                GenericResult<OrderEntity> downloadedOrder = await LoadOrder(odbcRecordsForOrder).ConfigureAwait(false);

                if (downloadedOrder.Success)
                {
                    try
                    {
                        await SaveDownloadedOrder(downloadedOrder.Value).ConfigureAwait(false);
                    }
                    catch (ORMQueryExecutionException ex)
                    {
                        throw new DownloadException(ex.Message, ex);
                    }
                }

                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }
        }

        /// <summary>
        /// Downloads the order.
        /// </summary>
        /// <exception cref="DownloadException">Order number not found in map.</exception>
        private async Task<GenericResult<OrderEntity>> LoadOrder(IGrouping<string, OdbcRecord> odbcRecordsForOrder)
        {
            OdbcRecord firstRecord = odbcRecordsForOrder.First();

            fieldMap.ApplyValues(firstRecord);

            // Find the OrderNumber Entry
            IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.FindEntriesBy(OrderFields.OrderNumberComplete).FirstOrDefault();

            if (odbcFieldMapEntry == null)
            {
                throw new DownloadException("Order number not found in map.");
            }

            if (odbcFieldMapEntry.ShipWorksField.Value == null)
            {
                throw new DownloadException("Order number is empty in your ODBC data source.");
            }

            string orderNumber = odbcFieldMapEntry.ShipWorksField.Value.ToString();
            // We strip out leading 0's. If all 0's, TrimStart would make it an empty string,
            // so in that case, we leave a single 0.
            orderNumber = orderNumber.All(n => n == '0') ? "0" : orderNumber.TrimStart('0');

            GenericResult<OrderEntity> result = await InstantiateOrder(odbcStoreType.CreateOrderIdentifier(orderNumber)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumber, result.Message);
                return result;
            }

            OrderEntity orderEntity = result.Value;

            orderLoader.Load(fieldMap, orderEntity, odbcRecordsForOrder);

            orderEntity.ChangeOrderNumber(orderNumber);

            return GenericResult.FromSuccess(orderEntity);
        }
    }
}
