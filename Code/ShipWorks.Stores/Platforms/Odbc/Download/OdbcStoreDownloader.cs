using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Warehouse.Orders;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Downloader for an OdbcStoreDownloader
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Odbc)]
    public class OdbcStoreDownloader : StoreDownloader
    {
        private readonly IOdbcDownloadCommandFactory downloadCommandFactory;
        private readonly Lazy<IOdbcFieldMap> fieldMap;
        private readonly IOdbcOrderLoader orderLoader;
        private readonly IWarehouseOrderClient warehouseOrderClient;
        private readonly ILicenseService licenseService;
        private readonly IConfigurationData configurationData;
        private readonly IStoreManager storeManager;
        private readonly OdbcStoreEntity store;
        private readonly ILog log;
        private readonly OdbcStoreType odbcStoreType;
        private readonly Lazy<OdbcStore> odbcStore;
        readonly Lazy<string> warehouseID;

        // not including decimal, money, numeric, real and float because that would be stupid
        // included names for integers from sql, mysql, oracle, and db2
        private readonly string[] numericSqlDataTypes =
        {
            "tinyint", "smallint", "mediumint", "int", "bigint", "integer", "shortinteger", "longinteger",
            "sql_smallint","sql_integer","sql_bigint", "number", "smallserial", "serial", "bigserial", "long",
            "autonumber", "short", "number"
        };
        private readonly string[] numericSystemTypes = { "byte", "int16", "int", "int32", "int64" };

        private const int UploadOrdersBatchSize = 25;


        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreDownloader"/> class.
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public OdbcStoreDownloader(StoreEntity store,
            IOdbcDownloadCommandFactory downloadCommandFactory,
            Func<IOdbcFieldMap> createFieldMap,
            IOdbcOrderLoader orderLoader,
            IOdbcDownloaderExtraDependencies extras,
            IWarehouseOrderClient warehouseOrderClient,
            IOdbcStoreRepository odbcStoreRepository,
            ILicenseService licenseService,
            IConfigurationData configurationData,
            IStoreManager storeManager) : base(store, extras.GetStoreType(store))
        {
            this.downloadCommandFactory = downloadCommandFactory;
            this.orderLoader = orderLoader;
            this.warehouseOrderClient = warehouseOrderClient;
            this.licenseService = licenseService;
            this.configurationData = configurationData;
            this.storeManager = storeManager;
            this.store = (OdbcStoreEntity) store;
            log = extras.GetLog(GetType());
            odbcStoreType = StoreType as OdbcStoreType;

            odbcStore = new Lazy<OdbcStore>(() => odbcStoreRepository.GetStore(this.store));
            fieldMap = new Lazy<IOdbcFieldMap>(() => GetFieldMap(createFieldMap));
            warehouseID = new Lazy<string>(() => configurationData.FetchReadOnly().WarehouseID);
        }

        /// <summary>
        /// Gets the field map used in this class
        /// </summary>
        private IOdbcFieldMap GetFieldMap(Func<IOdbcFieldMap> createFieldMap)
        {
            IOdbcFieldMap newFieldMap = createFieldMap();
            newFieldMap.Load(odbcStore.Value.ImportMap);
            return newFieldMap;
        }

        /// <summary>
        /// Download the order with matching order number for the store
        /// </summary>
        protected override async Task Download(string orderNumber, TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                IOdbcCommand downloadCommand = downloadCommandFactory.CreateDownloadCommand(store, orderNumber, fieldMap.Value);
                AddTelemetryData(trackedDurationEvent, downloadCommand);
                await Download(downloadCommand, null).ConfigureAwait(false);
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
            await Download(trackedDurationEvent, null).ConfigureAwait(false);
        }


        /// <summary>
        /// Import ODBC Orders from external data source.
        /// </summary>
        private async Task Download(TrackedDurationEvent trackedDurationEvent, Func<Task> hubDownloadCallback)
        {
            try
            {
                if (odbcStore.Value.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
                {
                    throw new DownloadException($"The store, {store.StoreName}, is set to download orders on order search only. \r\n\r\n" +
                                                "To automatically download orders, change this store's order import settings.");
                }

                Progress.Detail = "Querying data source...";

                IOdbcCommand downloadCommand = await GenerateDownloadCommand(trackedDurationEvent).ConfigureAwait(false);
                AddTelemetryData(trackedDurationEvent, downloadCommand);

                await Download(downloadCommand, hubDownloadCallback).ConfigureAwait(false);
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Only download from warehouse if we are not using ondemand
        /// </summary>
        protected override async Task DownloadWarehouseOrders(Guid batchId)
        {
            try
            {
                OdbcImportStrategy importStrategy = (OdbcImportStrategy) odbcStore.Value.ImportStrategy;
                if (importStrategy == OdbcImportStrategy.OnDemand)
                {
                    throw new DownloadException(
                        $"The store, {store.StoreName}, is set to download orders on order search only. \r\n\r\n" +
                        "To automatically download orders, change this store's order import settings.");
                }

                if (importStrategy == OdbcImportStrategy.All)
                {
                    // This should never happen
                    throw new DownloadException(
                        $"The store, {store.StoreName}, is set to download ALL orders and this is not supported. \r\n\r\n" +
                        "To automatically download orders, change this store's order import settings to download by last modified.");
                }

                if (!string.IsNullOrEmpty(store.ImportConnectionString) &&
                    (string.IsNullOrEmpty(odbcStore.Value.OrderImportingWarehouseId) ||
                     warehouseID.Value == odbcStore.Value.OrderImportingWarehouseId) &&
                    IsWarehouseAllowed())
                {
                    using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                    {
                        await Download(trackedDurationEvent, () => base.DownloadWarehouseOrders(batchId)).ConfigureAwait(false);

                        CollectDownloadTelemetry(trackedDurationEvent);
                    }
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

            await base.DownloadWarehouseOrders(batchId).ConfigureAwait(false);
        }

        /// <summary>
        /// Add telemetry data to the TrackedDurationEvent
        /// </summary>
        private void AddTelemetryData(TrackedDurationEvent trackedDurationEvent, IOdbcCommand downloadCommand)
        {
            trackedDurationEvent.AddProperty("Odbc.Driver", downloadCommand.Driver);
            trackedDurationEvent.AddProperty("Import.Strategy", EnumHelper.GetApiValue((OdbcImportStrategy) odbcStore.Value.ImportStrategy));
        }

        /// <summary>
        /// Download using the download command
        /// </summary>
        private async Task Download(IOdbcCommand downloadCommand, Func<Task> hubDownloadCallback)
        {
            IEnumerable<OdbcRecord> downloadedOrders = downloadCommand.Execute();
            List<IGrouping<string, OdbcRecord>> orderGroups =
                downloadedOrders.GroupBy(o => o.RecordIdentifier).ToList();

            int orderCount = GetOrderCount(orderGroups);

            if (orderCount > 0)
            {
                EnsureRecordIdentifiersAreNotNull(orderGroups);

                await LoadOrders(orderGroups, orderCount, hubDownloadCallback).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Generates the download command based on the store entity
        /// </summary>
        private async Task<IOdbcCommand> GenerateDownloadCommand(TrackedDurationEvent trackedDurationEvent)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            if (odbcStore.Value.ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime)
            {
                // Used in the case that GetOnlineLastModifiedStartingPoint returns null
                int defaultDaysBack = store.InitialDownloadDays.GetValueOrDefault(7);

                // Get the starting point and include it for telemetry
                DateTime? onlineLastModifiedStartingPoint = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);
                DateTime startingPoint = onlineLastModifiedStartingPoint.GetValueOrDefault(DateTime.UtcNow.AddDays(-defaultDaysBack));

                if (IsWarehouseAllowed() && store.WarehouseLastModified.HasValue)
                {
                    startingPoint = store.WarehouseLastModified.Value;
                }

                trackedDurationEvent.AddMetric("Minutes.Back", DateTime.UtcNow.Subtract(startingPoint).TotalMinutes);

                return downloadCommandFactory.CreateDownloadCommand(store, startingPoint, fieldMap.Value);
            }

            // Use -1 to indicate that we are using the "all orders" download strategy
            trackedDurationEvent.AddMetric("Minutes.Back", -1);
            return downloadCommandFactory.CreateDownloadCommand(store, fieldMap.Value);
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
                    $"At least one order is missing a value in {fieldMap.Value.RecordIdentifierSource}");
            }
        }

        /// <summary>
        /// Gets the order count.
        /// </summary>
        private int GetOrderCount(List<IGrouping<string, OdbcRecord>> orderGroups)
        {
            string noOrdersMessage = "No orders to download.";
            if (IsWarehouseAllowed())
            {
                noOrdersMessage = "No orders to upload.";
            }

            int orderCount = orderGroups.Count;

            Progress.Detail = orderCount == 0 ? noOrdersMessage : $"{orderCount} orders found.";

            return orderCount;
        }

        /// <summary>
        /// Loads the order information into order entities
        /// </summary>
        private async Task LoadOrders(List<IGrouping<string, OdbcRecord>> orderGroups, int totalCount, Func<Task> hubDownloadCallback)
        {
            List<OrderEntity> ordersToSendToHub = new List<OrderEntity>();

            var lastOrder = orderGroups.Last();
            foreach (IGrouping<string, OdbcRecord> odbcRecordsForOrder in orderGroups)
            {
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                GenericResult<OrderEntity> downloadedOrder = await LoadOrder(odbcRecordsForOrder).ConfigureAwait(false);

                if (downloadedOrder.Success)
                {
                    if (IsWarehouseAllowed())
                    {
                        Progress.Detail = $"Uploading orders to the hub.";

                        if (odbcStore.Value.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
                        {
                            await UploadOrderToHub(downloadedOrder.Value).ConfigureAwait(false);
                            await SaveOrder(downloadedOrder).ConfigureAwait(false);
                        }

                        if (odbcStore.Value.ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime)
                        {
                            ordersToSendToHub.Add(downloadedOrder.Value);

                            if (ordersToSendToHub.Count >= UploadOrdersBatchSize || odbcRecordsForOrder == lastOrder)
                            {
                                await UploadOrderToHubWithRetry(ordersToSendToHub);

                                await hubDownloadCallback().ConfigureAwait(false);
                            }

                        }
                    }
                    else
                    {
                        await SaveOrder(downloadedOrder).ConfigureAwait(false);
                        Progress.PercentComplete = 100 * QuantitySaved / totalCount;
                    }
                }
            }
        }

        /// <summary>
        /// Upload orders to the hub with retry logic
        /// </summary>
        private async Task UploadOrderToHubWithRetry(List<OrderEntity> orders, int retryCount = 3)
        {
            await UploadOrdersToHub(orders).ConfigureAwait(false);

            store.WarehouseLastModified = orders.Where(x => x.HubOrderID != null).Max(x => x.OnlineLastModified);
            storeManager.SaveStore(store);

            // if we made progress then reset the retry counter
            bool madeProgress = orders.Where(o => o.HubOrderID != null).Any();

            orders.RemoveAll(o => o.HubOrderID != null);

            if (orders.Any())
            {
                if (retryCount >= 0)
                {
                    await UploadOrderToHubWithRetry(orders, madeProgress ? 3 : retryCount - 1);
                }
                else
                {
                    throw new DownloadException("Error saving ODBC Orders.");
                }
            }
        }

        /// <summary>
        /// Save order to the database
        /// </summary>
        private async Task SaveOrder(GenericResult<OrderEntity> downloadedOrder)
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

        /// <summary>
        /// Is warehouse allowed for the customer
        /// </summary>
        private bool IsWarehouseAllowed()
        {
            return licenseService.CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None &&
                    store.WarehouseStoreID.HasValue;
        }

        /// <summary>
        /// Upload the order to the hub (if required)
        /// </summary>
        private async Task UploadOrderToHub(OrderEntity downloadedOrder)
        {
            if (store.WarehouseStoreID.HasValue)
            {
                GenericResult<IEnumerable<WarehouseUploadOrderResponse>> result =
                    await warehouseOrderClient.UploadOrders(new[] { downloadedOrder }, store, true).ConfigureAwait(false);
                if (result.Failure)
                {
                    throw new DownloadException(result.Message);
                }

                WarehouseUploadOrderResponse orderResponse = result.Value.Single();

                downloadedOrder.HubOrderID = Guid.Parse(orderResponse.HubOrderID);
                downloadedOrder.HubSequence = orderResponse.HubSequence;
            }
        }

        /// <summary>
        /// Upload the order to the hub (if required)
        /// </summary>
        private async Task UploadOrdersToHub(IEnumerable<OrderEntity> downloadedOrders)
        {
            if (store.WarehouseStoreID.HasValue)
            {
                GenericResult<IEnumerable<WarehouseUploadOrderResponse>> result =
                    await warehouseOrderClient.UploadOrders(downloadedOrders, store, false).ConfigureAwait(false);
                if (result.Failure)
                {
                    throw new DownloadException(result.Message);
                }

                foreach (WarehouseUploadOrderResponse orderResponse in result.Value)
                {
                    OrderEntity downloadedOrder = downloadedOrders.Single(x => x.OrderNumberComplete == orderResponse.OrderNumber);

                    downloadedOrder.HubOrderID = Guid.Parse(orderResponse.HubOrderID);
                    downloadedOrder.HubSequence = orderResponse.HubSequence;
                }
            }
        }

        /// <summary>
        /// Downloads the order.
        /// </summary>
        /// <exception cref="DownloadException">Order number not found in map.</exception>
        private async Task<GenericResult<OrderEntity>> LoadOrder(IGrouping<string, OdbcRecord> odbcRecordsForOrder)
        {
            OdbcRecord firstRecord = odbcRecordsForOrder.First();

            fieldMap.Value.ApplyValues(firstRecord);

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

            bool reloadEntireOrder = odbcStore.Value.ImportStrategy == (int) OdbcImportStrategy.OnDemand;
            if (reloadEntireOrder)
            {
                RemoveOrderItems(orderEntity);
            }
            orderLoader.Load(fieldMap.Value, orderEntity, odbcRecordsForOrder, reloadEntireOrder);

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
            IOdbcFieldMapEntry odbcFieldMapEntry;

            try
            {
                // Find the OrderNumber Entry
                odbcFieldMapEntry = fieldMap.Value.FindEntriesBy(OrderFields.OrderNumberComplete).FirstOrDefault();
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

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
