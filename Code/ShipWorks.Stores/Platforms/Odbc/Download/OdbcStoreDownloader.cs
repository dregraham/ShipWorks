﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

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
        private readonly IWarehouseOrderClient warehouseOrderClient;
        private readonly OdbcStoreEntity store;
        private readonly ILog log;
        private readonly OdbcStoreType odbcStoreType;
        private readonly bool reloadEntireOrder;
		
        // not including decimal, money, numeric, real and float because that would be stupid
        // included names for integers from sql, mysql, oracle, and db2
        private readonly string[] numericSqlDataTypes =
        {
            "tinyint", "smallint", "mediumint", "int", "bigint", "integer", "shortinteger", "longinteger",
            "sql_smallint","sql_integer","sql_bigint", "number", "smallserial", "serial", "bigserial", "long",
            "autonumber", "short", "number"
        };
        private readonly string[] numericSystemTypes = { "byte", "int16", "int", "int32", "int64" };

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreDownloader"/> class.
        /// </summary>
        public OdbcStoreDownloader(StoreEntity store,
            IOdbcDownloadCommandFactory downloadCommandFactory,
            IOdbcFieldMap fieldMap,
            IOdbcOrderLoader orderLoader,
            IOdbcDownloaderExtraDependencies extras, 
            IWarehouseOrderClient warehouseOrderClient) : base(store, extras.GetStoreType(store))
        {
            this.downloadCommandFactory = downloadCommandFactory;
            this.fieldMap = fieldMap;
            this.orderLoader = orderLoader;
            this.warehouseOrderClient = warehouseOrderClient;
            this.store = (OdbcStoreEntity) store;
            log = extras.GetLog(GetType());
            odbcStoreType = StoreType as OdbcStoreType;

            fieldMap.Load(this.store.ImportMap);
            reloadEntireOrder = this.store.ImportStrategy == (int) OdbcImportStrategy.OnDemand;
        }

        /// <summary>
        /// Download the order with matching order number for the store
        /// </summary>
        protected override async Task Download(string orderNumber, TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                IOdbcCommand downloadCommand = downloadCommandFactory.CreateDownloadCommand(store, orderNumber, fieldMap);
                AddTelemetryData(trackedDurationEvent, downloadCommand);
                await Download(downloadCommand).ConfigureAwait(false);
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new OnDemandDownloadException(IsCastException(ex), ex.Message, ex);
            }
        }

        /// <summary>
        /// Return true if orderNumber is of same type as the order number of the external source of this store.
        /// </summary>
        public override bool ShouldDownload(string orderNumber)
        {
            // if datatype is a primary key it will be called something like "bigint identity"
            // so, grab the first word.
            // Also, mySql includes lengths of fields within parenthesis. 
            string dataType = GetOrderNumberFieldMapEntry().ExternalField?.Column?.DataType?.Split(' ', '(')[0];

            // I don't think this should ever happen...
            if (string.IsNullOrEmpty(dataType))
            {
                throw new DownloadException("OrderNumberComplete needs to be remapped.");
            }

            bool isNumeric = numericSqlDataTypes.Any(t => dataType.Equals(t, StringComparison.InvariantCultureIgnoreCase)) ||
                   numericSystemTypes.Any(t => dataType.Equals(t, StringComparison.InvariantCultureIgnoreCase));

            bool shouldDownload = true;
            if (isNumeric)
            {
                shouldDownload = long.TryParse(orderNumber, out _);
            }

            if (!shouldDownload)
            {
                log.Info($"SearchTerm '{orderNumber}' could not be converted to a long. Skipping search for store '{store.StoreName}'");
            }

            return shouldDownload;
        }

        /// <summary>
        /// Return true if ODBC Cast Exception
        /// </summary>
        private bool IsCastException(ShipWorksOdbcException shipWorksOdbcException)
        {
            OdbcException odbcException = shipWorksOdbcException.GetBaseException() as OdbcException;
            
            return odbcException?.Errors.Cast<OdbcError>().None(error => error.SQLState == "22018") ?? true;
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
                AddTelemetryData(trackedDurationEvent, downloadCommand);

                await Download(downloadCommand).ConfigureAwait(false);
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Add telemetry data to the TrackedDurationEvent
        /// </summary>
        private void AddTelemetryData(TrackedDurationEvent trackedDurationEvent, IOdbcCommand downloadCommand)
        {
            trackedDurationEvent.AddProperty("Odbc.Driver", downloadCommand.Driver);
            trackedDurationEvent.AddProperty("Import.Strategy", EnumHelper.GetApiValue((OdbcImportStrategy) store.ImportStrategy));
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
                    await UploadOrderToHub(downloadedOrder.Value).ConfigureAwait(false);
                    
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
        /// Upload the order to the hub (if required)
        /// </summary>
        private async Task UploadOrderToHub(OrderEntity downloadedOrder)
        {
            if (store.WarehouseStoreID.HasValue)
            {
                GenericResult<WarehouseUploadOrderResponse> result = await warehouseOrderClient.UploadOrder(downloadedOrder, store).ConfigureAwait(false);
                if (result.Failure)
                {
                    throw new DownloadException(result.Message);
                }

                downloadedOrder.HubOrderID = Guid.Parse(result.Value.HubOrderID);
                downloadedOrder.HubSequence = result.Value.HubSequence;
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

            IOdbcFieldMapEntry odbcFieldMapEntry = GetOrderNumberFieldMapEntry();

            if (odbcFieldMapEntry.ShipWorksField.Value == null)
            {
                throw new DownloadException("Order number is empty in your ODBC data source.");
            }

            string orderNumberToUse = odbcFieldMapEntry.ShipWorksField.Value.ToString();
            GenericResult<OrderEntity> orderResultToUse =
                await InstantiateOrder(odbcStoreType.CreateOrderIdentifier(orderNumberToUse)).ConfigureAwait(false);
            if (orderResultToUse.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumberToUse, orderResultToUse.Message);
                return orderResultToUse;
            }

            GenericResult<OrderEntity> resultWithTrimmedOrderNumber;

            if (orderResultToUse.Value.IsNew)
            {

                // We strip out leading 0's. If all 0's, TrimStart would make it an empty string,
                // so in that case, we leave a single 0.
                string trimmedOrderNumber = orderNumberToUse.All(n => n == '0') ? "0" : orderNumberToUse.TrimStart('0');

                resultWithTrimmedOrderNumber = await InstantiateOrder(odbcStoreType.CreateOrderIdentifier(trimmedOrderNumber)).ConfigureAwait(false);
                if (resultWithTrimmedOrderNumber.Failure)
                {
                    log.InfoFormat("Skipping order '{0}': {1}.", trimmedOrderNumber, resultWithTrimmedOrderNumber.Message);
                    return resultWithTrimmedOrderNumber;
                }
                if (!resultWithTrimmedOrderNumber.Value.IsNew)
                {
                    orderResultToUse = resultWithTrimmedOrderNumber;
                    orderNumberToUse = trimmedOrderNumber;
                }
            }

            OrderEntity orderEntity = orderResultToUse.Value;

            if (reloadEntireOrder)
            {
                RemoveOrderItems(orderEntity);
            }

            orderLoader.Load(fieldMap, orderEntity, odbcRecordsForOrder, reloadEntireOrder);

            orderEntity.ChangeOrderNumber(orderNumberToUse);

            return GenericResult.FromSuccess(orderEntity);
        }

        /// <summary>
        /// Gets the OrderNumberComplete fieldMapEntry
        /// </summary>
        /// <remarks>
        /// Throws DownloadException if cannot find OrderNumberComplete field in the map
        /// </remarks>
        private IOdbcFieldMapEntry GetOrderNumberFieldMapEntry()
        {
            // Find the OrderNumber Entry
            IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.FindEntriesBy(OrderFields.OrderNumberComplete).FirstOrDefault();

            if (odbcFieldMapEntry == null)
            {
                throw new DownloadException("Order number not found in map.");
            }

            return odbcFieldMapEntry;
        }
		
		/// <summary>
        /// Removes order items from the order
        /// </summary>
        private void RemoveOrderItems(OrderEntity order)
        {
            if (order.OrderItems.Any())
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    foreach (OrderItemEntity item in order.OrderItems)
                    {
                        if (item.OrderItemAttributes.Any())
                        {
                            adapter.DeleteEntityCollection(item.OrderItemAttributes);
                        }
                    }

                    adapter.DeleteEntityCollection(order.OrderItems);
                    adapter.Commit();
                }
                order.OrderItems.Clear();
            }
        }
    }
}
