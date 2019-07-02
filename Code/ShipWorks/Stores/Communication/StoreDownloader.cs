using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.Tools.BCLExtensions.CollectionsRelated;
using ShipWorks.Actions;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Products;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Tokens;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Base that all store types must implement to provide downloading store data
    /// </summary>
    public abstract class StoreDownloader : IOrderElementFactory, IStoreDownloader
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreDownloader));
        private static readonly HashSet<EntityType> defaultOrderPreloads = new HashSet<EntityType> { EntityType.OrderItemEntity, EntityType.OrderChargeEntity };
        private readonly IConfigurationEntity config;
        protected readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IOrderUtility orderUtility;
        protected long downloadLogID;
        protected DbConnection connection;
        private string orderStatusText = string.Empty;
        private string itemStatusText = string.Empty;
        private bool orderStatusHasTokens;
        private bool itemStatusHasTokens;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader(StoreEntity store, StoreType storeType) :
            this(store, storeType, ConfigurationData.FetchReadOnly(), new SqlAdapterFactory())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader(StoreEntity store, StoreType storeType, IConfigurationEntity configuration, ISqlAdapterFactory sqlAdapterFactory) :
            this(store, storeType, configuration, sqlAdapterFactory, new OrderUtilityWrapper(sqlAdapterFactory))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader(StoreEntity store, StoreType storeType, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory) :
            this(store, storeType, configurationData.FetchReadOnly(), sqlAdapterFactory, new OrderUtilityWrapper(sqlAdapterFactory))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private StoreDownloader(StoreEntity store, StoreType storeType, IConfigurationEntity configuration,
                                ISqlAdapterFactory sqlAdapterFactory, IOrderUtility orderUtility)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            Store = store;
            StoreType = storeType;
            config = configuration;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderUtility = orderUtility;
        }

        /// <summary>
        /// Gets the address of order from InstantiateOrder
        /// </summary>
        protected AddressAdapter ShippingAddressBeforeDownload { get; private set; }

        /// <summary>
        /// Gets the address of order from InstantiateOrder
        /// </summary>
        protected AddressAdapter BillingAddressBeforeDownload { get; private set; }

        /// <summary>
        /// The store the downloader downloads from
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// The StoreType instance for the store
        /// </summary>
        protected StoreType StoreType { get; }

        /// <summary>
        /// The progress reporting interface used to report progress and check cancellation.
        /// </summary>
        public IProgressReporter Progress { get; protected set; }

        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        public int QuantitySaved { get; private set; }

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        public int QuantityNew { get; private set; }

        /// <summary>
        /// Gets the next OrderNumber that an order should use.  This is useful for store types that don't supply their own
        /// order numbers for ShipWorks, such as Amazon and eBay.
        /// </summary>
        protected async Task<long> GetNextOrderNumberAsync() => await orderUtility.GetNextOrderNumberAsync(Store.StoreID)
                                                                             .ConfigureAwait(false);

        /// <summary>
        /// Download data from the configured store.
        /// </summary>
        public async Task Download(IProgressReporter progress, IDownloadEntity downloadLog, DbConnection connection)
        {
            MethodConditions.EnsureArgumentIsNotNull(progress, nameof(progress));

            if (Progress != null)
            {
                throw new InvalidOperationException("Download should only be called once per instance.");
            }

            orderStatusText = StatusPresetManager.GetStoreDefault(Store, StatusPresetTarget.Order).StatusText;
            itemStatusText = StatusPresetManager.GetStoreDefault(Store, StatusPresetTarget.OrderItem).StatusText;
            orderStatusHasTokens = TemplateTokenProcessor.HasTokens(orderStatusText);
            itemStatusHasTokens = TemplateTokenProcessor.HasTokens(itemStatusText);

            Progress = progress;
            this.downloadLogID = downloadLog.DownloadID;
            this.connection = connection;

            if (Store.WarehouseStoreID == null)
            {
                using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                {
                    await Download(trackedDurationEvent).ConfigureAwait(false);

                    CollectDownloadTelemetry(trackedDurationEvent);
                }
            }
            else
            {
                await DownloadWarehouseOrders(downloadLog.BatchID).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Collect the download telemetry
        /// </summary>
        private void CollectDownloadTelemetry(TrackedDurationEvent trackedDurationEvent)
        {
            trackedDurationEvent.AddProperty("Store.Type", EnumHelper.GetDescription(StoreType.TypeCode));
            trackedDurationEvent.AddMetric("Orders.Total", QuantitySaved);
            trackedDurationEvent.AddMetric("Orders.New", QuantityNew);
        }

        /// <summary>
        /// Must be implemented by derived types to do the actual download
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected abstract Task Download(TrackedDurationEvent trackedDurationEvent);

        /// <summary>
        /// Gets the largest last modified time we have in our database for non-manual orders for this store.
        /// If no such orders exist, and there is an initial download policy, that policy is applied.  Otherwise null is returned.
        /// </summary>
        protected virtual async Task<DateTime?> GetOnlineLastModifiedStartingPoint()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IDownloadStartingPoint startingPoint = lifetimeScope.Resolve<IDownloadStartingPoint>();
                return await startingPoint.OnlineLastModified(Store).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        protected async Task<DateTime?> GetOrderDateStartingPoint()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IDownloadStartingPoint startingPoint = lifetimeScope.Resolve<IDownloadStartingPoint>();
                return await startingPoint.OrderDate(Store).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the largest OrderNumber we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then if there is an InitialDownloadPolicy it is applied.  Otherwise, 0 is returned.
        /// </summary>
        protected async Task<long> GetOrderNumberStartingPoint()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IDownloadStartingPoint startingPoint = lifetimeScope.Resolve<IDownloadStartingPoint>();
                return await startingPoint.OrderNumber(Store).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the largest HubSequence we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then 0 is returned.
        /// </summary>
        protected async Task<long> GetHubSequenceStartingPoint()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IDownloadStartingPoint startingPoint = lifetimeScope.Resolve<IDownloadStartingPoint>();
                return await startingPoint.HubSequence(Store).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        protected virtual Task<GenericResult<OrderEntity>> InstantiateOrder(long orderNumber) =>
            InstantiateOrder(new OrderNumberIdentifier(orderNumber));

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        protected virtual Task<GenericResult<OrderEntity>> InstantiateOrder(OrderIdentifier orderIdentifier) =>
            InstantiateOrder(orderIdentifier, Enumerable.Empty<EntityType>());

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        [SuppressMessage("ShipWorks", "SW0002")]
        protected virtual async Task<GenericResult<OrderEntity>> InstantiateOrder(OrderIdentifier orderIdentifier, IEnumerable<EntityType> orderPreloads)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderIdentifier, nameof(orderIdentifier));

            // Try to find an existing order
            OrderEntity order = await FindOrder(orderIdentifier, orderPreloads).ConfigureAwait(false);

            if (order != null)
            {
                log.Debug($"Found existing {orderIdentifier}");

                if (order.CombineSplitStatus != CombineSplitStatusType.None)
                {
                    string combineSplitStatus = EnumHelper.GetDescription(order.CombineSplitStatus);
                    log.InfoFormat($"{ order.OrderNumberComplete } was { combineSplitStatus }, skipping");

                    // Increment the quantity saved so we show the user we are moving to the next order.
                    QuantitySaved++;

                    return GenericResult.FromError<OrderEntity>(combineSplitStatus);
                }

                ShippingAddressBeforeDownload = new AddressAdapter();
                AddressAdapter.Copy(order, "Ship", ShippingAddressBeforeDownload);

                BillingAddressBeforeDownload = new AddressAdapter();
                AddressAdapter.Copy(order, "Bill", BillingAddressBeforeDownload);

                return GenericResult.FromSuccess(order);
            }

            bool isCombinedOrder = await IsCombinedOrder(orderIdentifier).ConfigureAwait(false);
            if (isCombinedOrder)
            {
                OrderEntity orderForLogging = StoreType.CreateOrder();
                orderIdentifier.ApplyTo(orderForLogging);

                log.InfoFormat($"{ orderForLogging.OrderNumberComplete } was combined, skipping");

                // Increment the quantity saved so we show the user we are moving to the next order.
                QuantitySaved++;

                return GenericResult.FromError<OrderEntity>("Combined");
            }

            order = CreateOrder(orderIdentifier);
            return GenericResult.FromSuccess(order);
        }

        /// <summary>
        /// Create an order for the given order identifier.
        /// </summary>
        private OrderEntity CreateOrder(OrderIdentifier orderIdentifier)
        {
            log.Debug($"{orderIdentifier} not found, creating");

            // Create a new order
            OrderEntity order = StoreType.CreateOrder();

            // Its for this store
            order.StoreID = Store.StoreID;

            // Apply the identifier values to the order
            orderIdentifier.ApplyTo(order);
            order.IsManual = false;

            // Set defaults
            order.OnlineStatus = "";
            order.LocalStatus = "";
            PersonAdapter.ApplyDefaults(order, "Bill");
            PersonAdapter.ApplyDefaults(order, "Ship");

            // Roll-up defaults
            order.RollupNoteCount = 0;
            order.RollupItemCount = 0;
            order.RollupItemTotalWeight = 0;

            order.ShipSenseHashKey = string.Empty;
            order.ShipSenseRecognitionStatus = (int) ShipSenseOrderRecognitionStatus.NotRecognized;

            order.CombineSplitStatus = CombineSplitStatusType.None;

            return order;
        }

        /// <summary>
        /// Is this a combined order
        /// </summary>
        private async Task<bool> IsCombinedOrder(OrderIdentifier orderIdentifier)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                QueryFactory factory = new QueryFactory();
                QuerySpec combinedSearchQuery = orderIdentifier.CreateCombinedSearchQuery(factory);
                combinedSearchQuery.AndWhere(OrderSearchFields.StoreID == this.Store.StoreID);
                combinedSearchQuery.AndWhere(OrderSearchFields.IsManual == false);
                DynamicQuery query = factory.Create().Select(combinedSearchQuery.Any());

                return (await sqlAdapter.FetchScalarAsync<bool?>(query).ConfigureAwait(false)) ?? false;
            }
        }

        /// <summary>
        /// Find the order with the configured OrderNumber.  If no order exists, null is returned.
        /// </summary>
        protected virtual Task<OrderEntity> FindOrder(OrderIdentifier orderIdentifier) =>
            FindOrder(orderIdentifier, Enumerable.Empty<EntityType>());

        /// <summary>
        /// Find the order with the configured OrderNumber.  If no order exists, null is returned.
        /// </summary>
        protected virtual async Task<OrderEntity> FindOrder(OrderIdentifier orderIdentifier, IEnumerable<EntityType> orderPreloads)
        {
            // We use a prototype approach to determine what to search for
            OrderEntity prototype = StoreType.CreateOrder();
            prototype.IsNew = false;
            prototype.RollbackChanges();
            orderIdentifier.ApplyTo(prototype);

            IPredicateExpression predicate = prototype.Fields.Where(x => x.IsChanged)
                .Select(x => new FieldCompareValuePredicate(x, null, ComparisonOperator.Equal, x.CurrentValue))
                .Aggregate(
                    (IPredicateExpression) new PredicateExpression(OrderFields.StoreID == Store.StoreID),
                    (accumulator, x) => accumulator.And(x));

            QueryFactory factory = new QueryFactory();
            EntityQuery<OrderEntity> query = factory.Order
                .Where(OrderFields.IsManual == false)
                .AndWhere(predicate);

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                OrderEntity order = await adapter.FetchFirstAsync(query).ConfigureAwait(false);

                if (order?.IsNew == false)
                {
                    order.Store = StoreManager.GetStore(order.StoreID);

                    CancellationTokenSource token = new CancellationTokenSource();

                    var preloads = defaultOrderPreloads.Concat(orderPreloads).Distinct().ToHashSet();
                    await PreloadItems(preloads, adapter, order, token).ConfigureAwait(false);
                    await PreloadCharges(preloads, adapter, order, token).ConfigureAwait(false);
                    await PreloadPaymentDetails(preloads, adapter, order, token).ConfigureAwait(false);

                    return order;
                }

                return null;
            }
        }

        /// <summary>
        /// Pre-load order items
        /// </summary>
        /// <param name="orderPreloads">Entities that should be pre-loaded on the order</param>
        /// <param name="adapter">Adapter that will be used for the query</param>
        /// <param name="order">Order that will have its data pre-loaded</param>
        /// <param name="token">Cancellation token</param>
        private static async Task PreloadItems(IEnumerable<EntityType> orderPreloads, ISqlAdapter adapter, OrderEntity order, CancellationTokenSource token)
        {
            if (orderPreloads.Contains(EntityType.OrderItemEntity))
            {
                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderItemEntity);
                prefetch.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

                await adapter.FetchEntityCollectionAsync(new QueryParameters
                {
                    CollectionToFetch = order.OrderItems,
                    FilterToUse = OrderItemFields.OrderID == order.OrderID,
                    PrefetchPathToUse = prefetch
                }, token.Token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Pre-load order charges
        /// </summary>
        /// <param name="orderPreloads">Entities that should be pre-loaded on the order</param>
        /// <param name="adapter">Adapter that will be used for the query</param>
        /// <param name="order">Order that will have its data pre-loaded</param>
        /// <param name="token">Cancellation token</param>
        private static async Task PreloadCharges(IEnumerable<EntityType> orderPreloads, ISqlAdapter adapter, OrderEntity order, CancellationTokenSource token)
        {
            if (orderPreloads.Contains(EntityType.OrderChargeEntity))
            {
                await adapter.FetchEntityCollectionAsync(new QueryParameters
                {
                    CollectionToFetch = order.OrderCharges,
                    FilterToUse = OrderChargeFields.OrderID == order.OrderID
                }, token.Token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Pre-load order payment details
        /// </summary>
        /// <param name="orderPreloads">Entities that should be pre-loaded on the order</param>
        /// <param name="adapter">Adapter that will be used for the query</param>
        /// <param name="order">Order that will have its data pre-loaded</param>
        /// <param name="token">Cancellation token</param>
        private static async Task PreloadPaymentDetails(IEnumerable<EntityType> orderPreloads, ISqlAdapter adapter, OrderEntity order, CancellationTokenSource token)
        {
            if (orderPreloads.Contains(EntityType.OrderPaymentDetailEntity))
            {
                await adapter.FetchEntityCollectionAsync(new QueryParameters
                {
                    CollectionToFetch = order.OrderPaymentDetails,
                    FilterToUse = OrderPaymentDetailFields.OrderID == order.OrderID
                }, token.Token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create a new OrderItem based on the given OrderEntity
        /// </summary>
        protected OrderItemEntity InstantiateOrderItem(OrderEntity order)
        {
            OrderItemEntity item = StoreType.CreateOrderItemInstance();
            item.Order = order;

            // Downloaded items are assumed not manual
            item.IsManual = false;

            // Initialize the rest of the fields
            item.InitializeNullsToDefault();

            return item;
        }

        /// <summary>
        /// Create a new OrderItemAttribute based on the given OrderItemEntity
        /// </summary>
        protected OrderItemAttributeEntity InstantiateOrderItemAttribute(OrderItemEntity item)
        {
            OrderItemAttributeEntity attribute = StoreType.CreateOrderItemAttributeInstance();
            attribute.OrderItem = item;

            // downloaded attributes are not manual
            attribute.IsManual = false;

            return attribute;
        }

        /// <summary>
        /// Creates and populates a new OrderItemAttribute based on the given OrderItemEntity, name, description, unitPrice, and isManual flag
        /// </summary>
        protected OrderItemAttributeEntity InstantiateOrderItemAttribute(OrderItemEntity item, string name, string description, decimal unitPrice, bool isManual)
        {
            OrderItemAttributeEntity attribute = StoreType.CreateOrderItemAttributeInstance();
            attribute.OrderItem = item;
            attribute.Name = name;
            attribute.Description = description;
            attribute.UnitPrice = unitPrice;
            attribute.IsManual = isManual;

            return attribute;
        }

        /// <summary>
        /// Create a new order charge based on the given order
        /// </summary>
        protected OrderChargeEntity InstantiateOrderCharge(OrderEntity order) =>
            new OrderChargeEntity { Order = order };

        /// <summary>
        /// Create a new order charge based on the given order, type, description and amount
        /// </summary>
        protected OrderChargeEntity InstantiateOrderCharge(OrderEntity order, string type, string description, decimal amount) =>
            new OrderChargeEntity
            {
                Order = order,
                Type = type,
                Description = description,
                Amount = amount
            };

        /// <summary>
        /// Create a new payment detail based on the given order
        /// </summary>
        protected OrderPaymentDetailEntity InstantiateOrderPaymentDetail(OrderEntity order) =>
            new OrderPaymentDetailEntity { Order = order };

        /// <summary>
        /// Create a new payment detail based on the given order, label, and value
        /// </summary>
        protected OrderPaymentDetailEntity InstantiateOrderPaymentDetail(OrderEntity order, string label, string value) => new OrderPaymentDetailEntity
        {
            Order = order,
            Label = label,
            Value = value
        };

        /// <summary>
        /// Creates a new note instance, but only if the note text is non-blank.  If its blank, null is returned.
        /// </summary>
        protected async Task<NoteEntity> InstantiateNote(OrderEntity order, string noteText, DateTime? noteDate,
            NoteVisibility visibility, bool ignoreDuplicateText = false)
        {
            if (string.IsNullOrWhiteSpace(noteText))
            {
                return null;
            }

            noteText = noteText.Trim();

            // If we need to ignore adding any notes that are dupes of ones that exist...
            if (ignoreDuplicateText && await NoteExists(order, noteText, visibility).ConfigureAwait(false))
            {
                return null;
            }

            // Instantiate the note
            return new NoteEntity()
            {
                Order = order,
                UserID = null,
                Edited = noteDate ?? order.OrderDate,
                Source = (int) NoteSource.Downloaded,
                Visibility = (int) visibility,
                Text = noteText
            };
        }

        /// <summary>
        /// Check whether the given note text exists in the order
        /// </summary>
        private async Task<bool> NoteExists(OrderEntity order, string noteText, NoteVisibility visibility)
        {
            // First see if any of the current (newly downloaded) notes match this note
            if (order.Notes.Any(n =>
                string.Compare(n.Text, noteText, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                n.Visibility == (int) visibility &&
                n.Source == (int) NoteSource.Downloaded))
            {
                return true;
            }

            // If the order isn't new, check the ones in the database too
            if (order.IsNew)
            {
                return false;
            }

            IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();

            QueryFactory factory = new QueryFactory();
            DynamicQuery query = factory.Note
                .Select(Functions.CountRow())
                .From(relationPredicateBucket.Relations)
                .Where(relationPredicateBucket.PredicateExpression)
                .AndWhere(NoteFields.Text == noteText)
                .AndWhere(NoteFields.Visibility == (int) visibility)
                .AndWhere(NoteFields.Source == (int) NoteSource.Downloaded);

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                int? count = await adapter.FetchScalarAsync<int?>(query).ConfigureAwait(false);
                return count.GetValueOrDefault() > 0;
            }
        }

        /// <summary>
        /// Save the given order that has been downloaded.
        /// </summary>
        protected virtual async Task SaveDownloadedOrder(OrderEntity order)
        {
            await connection.WithTransactionAsync(async adapter =>
            {
                var postAction = await SaveDownloadedOrderWithoutPostAction(order, adapter).ConfigureAwait(false);

                adapter.Commit();
                postAction();
            }).ConfigureAwait(false);

            // Updating order/order item statuses have to be done outside of a transaction,
            // so do that now.
            using (ISqlAdapter adapter = sqlAdapterFactory.Create(connection))
            {
                await UpdateOrderStatusesAfterSave(order, adapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Save the given order that has been downloaded.
        /// </summary>
        /// <returns>
        /// Action that should be executed at the end of the logical save procedure. This is intended to be used when a downloader
        /// needs to do more work for saving an order than just calling this method.
        /// </returns>
        [SuppressMessage("ShipWorks", "SW0002",
            Justification = "The parameter name is not used for binding.")]
        protected virtual async Task<Action> SaveDownloadedOrderWithoutPostAction(OrderEntity order, ISqlAdapter adapter)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            try
            {
                using (new LoggedStopwatch(log, $"SaveDownloadedOrder: {order.OrderNumber}"))
                {
                    // Update the address casing of the order
                    if (config.AddressCasing)
                    {
                        ApplyAddressCasing(order);
                    }

                    // if the downloaders specified they parsed the name, also put the name in the unparsed field
                    PersonAdapter ship = new PersonAdapter(order, "Ship");
                    PersonAdapter bill = new PersonAdapter(order, "Bill");

                    if (ship.NameParseStatus == PersonNameParseStatus.Simple)
                    {
                        ship.UnparsedName = new PersonName(ship.FirstName, ship.MiddleName, ship.LastName).FullName;
                    }

                    if (bill.NameParseStatus == PersonNameParseStatus.Simple)
                    {
                        bill.UnparsedName = new PersonName(bill.FirstName, bill.MiddleName, bill.LastName).FullName;
                    }

                    // We have to get this order's identifier
                    OrderIdentifier orderIdentifier = StoreType.CreateOrderIdentifier(order);

                    // Now we have to see if it was new
                    Task<bool> alreadyDownloaded = HasDownloadHistory(orderIdentifier);
                    bool isAlreadyDownloaded = false;

                    ResetAddressIfRequired(order.IsNew, order, adapter);

                    if (order.IsNew)
                    {
                        await SaveNewOrder(order, adapter).ConfigureAwait(false);
                    }
                    else
                    {
                        await SaveExistingOrder(order, adapter).ConfigureAwait(false);
                    }

                    isAlreadyDownloaded = await alreadyDownloaded.ConfigureAwait(false);
                    log.InfoFormat("{0} is {1} new", orderIdentifier, isAlreadyDownloaded ? "not " : "");

                    // Log this download
                    await AddToDownloadHistory(order.OrderID, orderIdentifier, isAlreadyDownloaded, adapter).ConfigureAwait(false);

                    // Dispatch the order downloaded action
                    ActionDispatcher.DispatchOrderDownloaded(order.OrderID, Store.StoreID, !isAlreadyDownloaded, adapter);

                    return () =>
                    {
                        QuantitySaved++;

                        if (!isAlreadyDownloaded)
                        {
                            QuantityNew++;
                        }
                    };
                }
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException ?? ae;
            }
        }

        /// <summary>
        /// Save a new order
        /// </summary>
        private async Task SaveNewOrder(OrderEntity order, ISqlAdapter adapter)
        {
            if (!order.IsNew)
            {
                await SaveExistingOrder(order, adapter).ConfigureAwait(false);
                return;
            }

            // Start getting the customer asynchronously since we don't need it until right before we save.
            Task<CustomerEntity> getCustomerTask = GetCustomer(order, adapter);

            ProtectOrderPaymentDetails(order);

            // Apply default status to order.  If tokenized, it has to be done after the save.
            // Don't overwrite it if its already set.
            if (string.IsNullOrEmpty(order.LocalStatus) && !orderStatusHasTokens)
            {
                order.LocalStatus = orderStatusText;
            }

            // Apply default status to items.  If tokenized, it has to be done after the save.
            if (!itemStatusHasTokens)
            {
                foreach (OrderItemEntity item in order.OrderItems)
                {
                    // Don't overwrite what the downloader set
                    if (string.IsNullOrEmpty(item.LocalStatus))
                    {
                        item.LocalStatus = itemStatusText;
                    }
                }
            }

            // If it's new and LastModified isn't set, use the date
            if (!order.Fields[(int) OrderFieldIndex.OnlineLastModified].IsChanged)
            {
                order.OnlineLastModified = order.OrderDate;
            }

            // Calculate or verify the order total
            VerifyOrderTotal(order);

            OrderUtility.UpdateShipSenseHashKey(order);

            SetAddressValidationStatus(order, true, "Ship", adapter);
            SetAddressValidationStatus(order, true, "Bill", adapter);

            log.Info($"StoreDownloader.SaveNewOrder waiting for getCustomerTask");
            // Wait for the customer to be found or created
            CustomerEntity customer = await getCustomerTask.ConfigureAwait(false);

            log.Info($"StoreDownloader.SaveNewOrder AdjustNoteCount");
            // Update the note counts
            AdjustNoteCount(order, customer);

            log.Info($"StoreDownloader.SaveNewOrder UpdateItemsProductCatalog");
            UpdateItemsFromProductCatalog(adapter, order);

            await PerformInitialOrderSave(order, customer, adapter).ConfigureAwait(false);
        }

        /// <summary>
        /// Loop through each item. If there is a matching active product in the catalog, update the dimensions
        /// </summary>
        private void UpdateItemsFromProductCatalog(ISqlAdapter adapter, OrderEntity order)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IProductCatalog productCatalog = lifetimeScope.Resolve<IProductCatalog>();

                foreach (OrderItemEntity orderItem in order.OrderItems)
                {
                    productCatalog.FetchProductVariant(adapter, orderItem.SKU).Apply(orderItem);
                }
            }
        }

        /// <summary>
        /// Protect order payment details
        /// </summary>
        private static void ProtectOrderPaymentDetails(OrderEntity order)
        {
            foreach (OrderPaymentDetailEntity detail in order.OrderPaymentDetails.Where(x => x.IsNew))
            {
                PaymentDetailSecurity.Protect(detail);
            }
        }

        /// <summary>
        /// Perform the initial order save
        /// </summary>
        private static async Task PerformInitialOrderSave(OrderEntity order, CustomerEntity customer, ISqlAdapter adapter)
        {
            try
            {
                // If note count changed, save the customer
                if (customer.IsDirty)
                {
                    log.Info($"StoreDownloader.PerformInitialOrderSave saving customer");
                    await adapter.SaveEntityAsync(customer, false).ConfigureAwait(false);
                }

                log.Info($"StoreDownloader.PerformInitialOrderSave setting order.CustomerID");
                order.CustomerID = customer.CustomerID;

                log.Info($"StoreDownloader.PerformInitialOrderSave");
                // Save the order so we can get its OrderID
                await adapter.SaveEntityAsync(order, false).ConfigureAwait(false);
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("SqlDateTime overflow", StringComparison.OrdinalIgnoreCase))
                {
                    throw new DownloadException(
                        $"Order {order.OrderNumber} has an invalid Order Date and/or Last Modified Online date/time. " +
                        "Please ensure that these values are between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.");
                }

                log.Error($"An order was not able to successfully download.  Please try downloading again.", ex);
                throw new DownloadException($"An order was not able to successfully download.  Please try downloading again.");
            }
        }

        /// <summary>
        /// Update order statuses after saving to handle tokenized statuses
        /// </summary>
        protected async Task UpdateOrderStatusesAfterSave(OrderEntity order, ISqlAdapter sqlAdapter)
        {
            bool orderChanged = false;
            string tmpLocalStatus = string.Empty;

            // Apply default order status, if it contained tokens it has to be after the save.  Don't overwrite what the downloader set.
            if (orderStatusHasTokens)
            {
                order = DataProvider.GetEntity(order.OrderID, sqlAdapter, true) as OrderEntity;
                if (string.IsNullOrEmpty(order.LocalStatus))
                {
                    tmpLocalStatus = TemplateTokenProcessor.ProcessTokens(orderStatusText, order.OrderID);
                    if (!order.LocalStatus.Equals(tmpLocalStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        order.LocalStatus = tmpLocalStatus;
                        orderChanged = true;
                    }
                }
            }

            // Apply default item status, if it contained token it has to be after the save and out of the transaction.
            if (itemStatusHasTokens)
            {
                OrderUtility.PopulateOrderDetails(order, sqlAdapter);

                foreach (OrderItemEntity item in order.OrderItems)
                {
                    // Don't overwrite what the downloader set
                    if (string.IsNullOrEmpty(item.LocalStatus))
                    {
                        tmpLocalStatus = TemplateTokenProcessor.ProcessTokens(itemStatusText, item.OrderItemID);

                        if (!item.LocalStatus.Equals(tmpLocalStatus, StringComparison.InvariantCultureIgnoreCase))
                        {
                            item.LocalStatus = tmpLocalStatus;
                            orderChanged = true;
                        }
                    }
                }
            }

            if (orderChanged)
            {
                await sqlAdapter.SaveEntityAsync(order).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        private async Task<CustomerEntity> GetCustomer(OrderEntity order, ISqlAdapter adapter)
        {
            try
            {
                return await CustomerProvider.AcquireCustomer(order, StoreType, adapter).ConfigureAwait(false);
            }
            catch (CustomerAcquisitionLockException)
            {
                throw new DownloadException(
                    "ShipWorks was unable to find the customer in the time allotted.  Please try downloading again.");
            }
        }

        /// <summary>
        /// Save an existing order
        /// </summary>
        private async Task SaveExistingOrder(OrderEntity order, ISqlAdapter adapter)
        {
            // Setting this as a local variable because a SaveAndRefetch changes IsNew to false.
            bool isOrderNew = order.IsNew;

            if (isOrderNew)
            {
                await SaveNewOrder(order, adapter).ConfigureAwait(false);
                return;
            }

            // Protect payment details
            foreach (OrderPaymentDetailEntity detail in order.OrderPaymentDetails)
            {
                if (detail.IsNew)
                {
                    PaymentDetailSecurity.Protect(detail);
                }
            }

            // Calculate or verify the order total
            VerifyOrderTotal(order);

            UpdateLocalItemStatus(order.OrderItems);

            // Everything has been set on the order, so calculate the hash key
            OrderUtility.UpdateShipSenseHashKey(order);

            CustomerEntity customer = await PerformValidationSteps(order, adapter).ConfigureAwait(false);

            // Update the note counts
            customer = customer ?? new CustomerEntity(order.CustomerID) { IsNew = false };
            AdjustNoteCount(order, customer);

            await adapter.SaveEntityAsync(customer, false).ConfigureAwait(false);
            await adapter.SaveEntityAsync(order, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Update the local item status
        /// </summary>
        /// <remarks>
        /// Don't overwrite what the downloader set
        /// </remarks>
        private void UpdateLocalItemStatus(IEnumerable<OrderItemEntity> orderItems)
        {
            IEnumerable<OrderItemEntity> filteredItems = orderItems
                .Where(i => i.IsNew)
                .Where(x => string.IsNullOrEmpty(x.LocalStatus));

            foreach (OrderItemEntity item in filteredItems)
            {
                item.LocalStatus = itemStatusHasTokens ?
                    TemplateTokenProcessor.ProcessTokens(itemStatusText, item.OrderItemID) :
                    itemStatusText;
            }
        }

        /// <summary>
        /// Copy addresses from validation if necessary
        /// </summary>
        private async Task<CustomerEntity> PerformValidationSteps(OrderEntity order, ISqlAdapter adapter)
        {
            // Update unprocessed shipment addresses if the order address has changed
            AddressAdapter newShippingAddress = new AddressAdapter(order, "Ship");
            bool shippingAddressChanged = ShippingAddressBeforeDownload != newShippingAddress;
            if (shippingAddressChanged)
            {
                SetAddressValidationStatus(order, false, "Ship", adapter);
                await adapter.SaveAndRefetchAsync(order).ConfigureAwait(false);

                ValidatedAddressManager.PropagateAddressChangesToShipments(adapter, order.OrderID, ShippingAddressBeforeDownload, newShippingAddress);
            }

            // Update the customer's addresses if necessary
            AddressAdapter newBillingAddress = new AddressAdapter(order, "Bill");
            bool billingAddressChanged = BillingAddressBeforeDownload != newBillingAddress;

            if (billingAddressChanged)
            {
                SetAddressValidationStatus(order, false, "Bill", adapter);
            }

            CustomerEntity customer = null;

            // Don't even bother loading the customer if the addresses haven't changed, or if we shouldn't copy
            if ((billingAddressChanged && config.CustomerUpdateModifiedBilling != (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy)
                || (shippingAddressChanged && config.CustomerUpdateModifiedShipping != (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy))
            {
                customer = DataProvider.GetEntity(order.CustomerID, adapter) as CustomerEntity;
                if (customer != null)
                {
                    UpdateCustomerAddressIfNecessary(billingAddressChanged,
                        (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedBilling,
                        order.BillPerson, customer.BillPerson, BillingAddressBeforeDownload);
                    UpdateCustomerAddressIfNecessary(shippingAddressChanged,
                        (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedShipping,
                        order.ShipPerson, customer.ShipPerson, ShippingAddressBeforeDownload);
                }
            }

            return customer;
        }

        /// <summary>
        /// Adjust the note counts on the order and customer
        /// </summary>
        private static void AdjustNoteCount(OrderEntity order, CustomerEntity customer)
        {
            int noteCount = order.IsNew ? order.Notes.Count : order.Notes.Count(n => n.IsNew);

            if (order.IsNew)
            {
                order.RollupNoteCount = noteCount;
            }
            else if (noteCount != 0)
            {
                order.Fields[(int) OrderFieldIndex.RollupNoteCount].ExpressionToApply = OrderFields.RollupNoteCount + noteCount;
                order.IsDirty = true;
            }

            if (customer.IsNew)
            {
                customer.RollupNoteCount = noteCount;
            }
            else if (noteCount != 0)
            {
                customer.Fields[(int) CustomerFieldIndex.RollupNoteCount].ExpressionToApply = CustomerFields.RollupNoteCount + noteCount;
                customer.IsDirty = true;
            }
        }

        /// <summary>
        /// If an order's addresses change to the originally validated address, change it back.
        /// </summary>
        private void ResetAddressIfRequired(bool isOrderNew, OrderEntity order, ISqlAdapter adapter)
        {
            if (!isOrderNew)
            {
                bool shipAddressReset = ResetAddressIfRequired(order, "Ship", ShippingAddressBeforeDownload);
                bool billAddressReset = ResetAddressIfRequired(order, "Bill", BillingAddressBeforeDownload);
                if (shipAddressReset || billAddressReset)
                {
                    adapter.SaveAndRefetch(order);
                }
            }
        }

        /// <summary>
        /// Resets the address if required.
        /// </summary>
        /// <param name="order">The order that now has the downloaded address.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="addressBeforeDownload">The address before download.</param>
        /// <returns>True if address was reset, else false.</returns>
        /// <remarks>
        /// If address changed and the new address matches the address pre-address validation (the AV original address)
        /// from address validation, reset the address back to the original address.
        /// </remarks>
        private bool ResetAddressIfRequired(OrderEntity order, string prefix, AddressAdapter addressBeforeDownload)
        {
            if (addressBeforeDownload == null)
            {
                return false;
            }

            bool addressReset = false;
            AddressAdapter orderAddress = new AddressAdapter(order, prefix);

            if (addressBeforeDownload != orderAddress)
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    ValidatedAddressEntity addressBeforeValidation =
                    ValidatedAddressManager.GetOriginalAddress(adapter, order.OrderID, prefix);

                    if (addressBeforeValidation != null)
                    {
                        AddressAdapter originalAddressAdapter = new AddressAdapter(addressBeforeValidation, string.Empty);

                        if (originalAddressAdapter == orderAddress)
                        {
                            AddressAdapter.Copy(addressBeforeDownload, orderAddress);
                            addressReset = true;
                        }
                    }
                }
            }

            return addressReset;
        }

        /// <summary>
        /// Sets the address validation status on the order, depending on the store settings
        /// </summary>
        private void SetAddressValidationStatus(OrderEntity order, bool isNewOrder, string prefix, ISqlAdapter adapter)
        {
            AddressAdapter address = new AddressAdapter(order, prefix)
            {
                POBox = (int) ValidationDetailStatusType.Unknown,
                MilitaryAddress = (int) ValidationDetailStatusType.Unknown,
                USTerritory = (int) ValidationDetailStatusType.Unknown,
                ResidentialStatus = (int) ValidationDetailStatusType.Unknown,
                AddressValidationSuggestionCount = 0,
                AddressValidationError = string.Empty,
                AddressType = (int) AddressType.NotChecked
            };

            if (!isNewOrder)
            {
                ValidatedAddressManager.DeleteExistingAddresses(adapter, order.OrderID, prefix);
            }

            if (prefix == "Ship")
            {
                address.UpdateValidationStatus(Store);
            }
        }

        /// <summary>
        /// Update's the customer's address from an order, if it's necessary
        /// </summary>
        [SuppressMessage("ShipWorks", "SW0002:Identifier should not be obfuscated",
            Justification = "Identifier is not being used for data binding")]
        public static void UpdateCustomerAddressIfNecessary(bool shouldUpdate, ModifiedOrderCustomerUpdateBehavior behavior,
            PersonAdapter orderAddress, PersonAdapter customerAddress, AddressAdapter originalAddress)
        {
            if (!shouldUpdate || PersonAdapter.IsAddressEmpty(orderAddress))
            {
                return;
            }

            bool shouldCopy;
            switch (behavior)
            {
                case ModifiedOrderCustomerUpdateBehavior.NeverCopy:
                    shouldCopy = false;
                    break;
                case ModifiedOrderCustomerUpdateBehavior.CopyIfBlankOrMatching:
                    shouldCopy = ShouldCopyIfBlankOrMatching(originalAddress, customerAddress);
                    break;
                case ModifiedOrderCustomerUpdateBehavior.AlwaysCopy:
                    shouldCopy = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(behavior));
            }

            if (shouldCopy)
            {
                orderAddress.CopyTo(customerAddress);
            }
        }

        /// <summary>
        /// Should the address be copied if it's blank or matching
        /// </summary>
        private static bool ShouldCopyIfBlankOrMatching(AddressAdapter originalAddress, PersonAdapter address)
        {
            if (PersonAdapter.IsAddressEmpty(address))
            {
                return true;
            }

            return originalAddress == null || originalAddress.Equals(address);
        }

        /// <summary>
        /// Apply address casing to the order
        /// </summary>
        private static void ApplyAddressCasing(OrderEntity order)
        {
            // List of fields we apply address casing to
            var fields = new List<EntityField2>
            {
                OrderFields.BillFirstName,
                OrderFields.BillMiddleName,
                OrderFields.BillLastName,
                OrderFields.BillCompany,
                OrderFields.BillStreet1,
                OrderFields.BillStreet2,
                OrderFields.BillStreet3,
                OrderFields.BillCity,
                OrderFields.ShipFirstName,
                OrderFields.ShipMiddleName,
                OrderFields.ShipLastName,
                OrderFields.ShipCompany,
                OrderFields.ShipStreet1,
                OrderFields.ShipStreet2,
                OrderFields.ShipStreet3,
                OrderFields.ShipCity
            };

            foreach (var fieldIndex in fields.Select(f => f.FieldIndex))
            {
                if (order.Fields[fieldIndex].IsChanged)
                {
                    order.SetNewFieldValue(fieldIndex, AddressCasing.Apply((string) order.GetCurrentFieldValue(fieldIndex)));
                }
            }
        }

        /// <summary>
        /// If the OrderTotal is set, the total is recalculated and verified.  If the verification fails, an assertion is raised.
        /// If the OrderTotal is not set, and the order is new, an error is raised.
        /// </summary>
        protected virtual void VerifyOrderTotal(OrderEntity order)
        {
            if (order.Fields[(int) OrderFieldIndex.OrderTotal].IsChanged)
            {
                decimal total = OrderUtility.CalculateTotal(order);

                Debug.Assert(total == order.OrderTotal,
                    $"Order total does not match calculated total \r\n Calculated Total {total}\r\n should equal Order Total {order.OrderTotal} for order {order.OrderNumber}.");
            }
            else if (order.IsNew)
            {
                Debug.Fail("New order does not have OrderTotal set.");
            }
        }

        /// <summary>
        /// Indicates if this order is already present in the download history.
        /// </summary>
        private async Task<bool> HasDownloadHistory(OrderIdentifier orderIdentifier)
        {
            IPredicate predicate = CreateDownloadHistoryPredicate(orderIdentifier);

            QueryFactory factory = new QueryFactory();
            DynamicQuery query = factory.Create()
                .From(factory.DownloadDetail.InnerJoin(DownloadDetailEntity.Relations.DownloadEntityUsingDownloadID))
                .Select(DownloadDetailFields.DownloadedDetailID.Count())
                .Where(predicate);

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                int count = await adapter.FetchScalarAsync<int>(query).ConfigureAwait(false);

                return count > 0;
            }
        }

        /// <summary>
        /// Create a predicate to get download history
        /// </summary>
        private IPredicateExpression CreateDownloadHistoryPredicate(OrderIdentifier orderIdentifier)
        {
            // We make a prototype history that will allow us to determinate what to search for
            DownloadDetailEntity history = new DownloadDetailEntity();
            orderIdentifier.ApplyTo(history);

            // We are searching for all the fields that have changed (the ones the OrderIdentifier applied)
            return history.Fields
                .Where(x => x.IsChanged)
                .Select(x => new FieldCompareValuePredicate(x, null, ComparisonOperator.Equal, x.CurrentValue))
                .Aggregate(
                    (IPredicateExpression) new PredicateExpression(DownloadFields.StoreID == Store.StoreID),
                    (accumulator, x) => accumulator.And(x));
        }

        /// <summary>
        /// Add the given order to the download history
        /// </summary>
        private Task AddToDownloadHistory(long orderID, OrderIdentifier orderIdentifier, bool alreadyDownloaded, ISqlAdapter adapter)
        {
            DownloadDetailEntity history = new DownloadDetailEntity()
            {
                DownloadID = downloadLogID,
                OrderID = orderID,
                InitialDownload = !alreadyDownloaded
            };

            orderIdentifier.ApplyTo(history);

            return adapter.SaveEntityAsync(history);
        }

        /// <summary>
        /// Download the order with matching order number for the store
        /// </summary>
        public async virtual Task Download(string orderNumber, long downloadID, DbConnection con)
        {
            Progress = new ProgressItem("Download single order");
            downloadLogID = downloadID;
            connection = con;

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                await Download(orderNumber, trackedDurationEvent).ConfigureAwait(false);

                CollectDownloadTelemetry(trackedDurationEvent);
            }
        }

        /// <summary>
        /// store specific download by order number
        /// </summary>
        protected virtual Task Download(string orderNumber, TrackedDurationEvent trackedDurationEvent)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Should not download
        /// </summary>
        public virtual bool ShouldDownload(string orderNumber) => false;

        /// <summary>
        /// Download orders for this store from the ShipWorks Warehouse app
        /// </summary>
        private async Task DownloadWarehouseOrders(Guid batchId)
        {
            if (string.IsNullOrEmpty(config.WarehouseID))
            {
                throw new DownloadException("Could not download orders because this ShipWorks database is not currently linked to a warehouse in ShipWorks Hub");
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IWarehouseOrderClient webClient = lifetimeScope.Resolve<IWarehouseOrderClient>();
                IWarehouseOrderFactory orderFactory = lifetimeScope
                    .ResolveKeyed<IWarehouseOrderFactory>(StoreType.TypeCode, TypedParameter.From<IOrderElementFactory>(this));

                bool shouldContinue = false;
                var count = 1;

                do
                {
                    long downloadStartPoint = await GetHubSequenceStartingPoint().ConfigureAwait(false);

                    // get orders for this store and warehouse
                    IEnumerable<WarehouseOrder> orders = await webClient
                        .GetOrders(config.WarehouseID, Store.WarehouseStoreID.ToString(), downloadStartPoint, StoreType.TypeCode, batchId)
                        .ConfigureAwait(false);

                    foreach (WarehouseOrder warehouseOrder in orders)
                    {
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        count += 1;
                        Progress.Detail = "Downloading order " + count.ToString("#,##0");

                        OrderEntity orderEntity = await orderFactory.CreateOrder(warehouseOrder).ConfigureAwait(false);
                        await SaveDownloadedOrder(orderEntity).ConfigureAwait(false);
                    }

                    shouldContinue = orders.Any();
                } while (shouldContinue);
            }
        }

        #region Order Element Factory
        // Explicit implementation of the IOrderElementFactory, this allows dependencies to create order elements without
        // exposing the whole downloader to the dependency

        /// <summary>
        /// Create an order with the given identifier
        /// </summary>
        Task<GenericResult<OrderEntity>> IOrderElementFactory.CreateOrder(OrderIdentifier orderIdentifier) =>
            InstantiateOrder(orderIdentifier);

        /// <summary>
        /// Create an item for the given order
        /// </summary>
        OrderItemEntity IOrderElementFactory.CreateItem(OrderEntity order) => InstantiateOrderItem(order);

        /// <summary>
        /// Create an item attribute for the given item
        /// </summary>
        OrderItemAttributeEntity IOrderElementFactory.CreateItemAttribute(OrderItemEntity item) =>
            InstantiateOrderItemAttribute(item);

        /// <summary>
        /// Create an item attribute for the given item
        /// </summary>
        OrderItemAttributeEntity IOrderElementFactory.CreateItemAttribute(OrderItemEntity item, string name,
            string description, decimal unitPrice,
            bool isManual) => InstantiateOrderItemAttribute(item, name, description, unitPrice, isManual);

        /// <summary>
        /// Create an order charge for the given order
        /// </summary>
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order) => InstantiateOrderCharge(order);

        /// <summary>
        /// Create an order charge for the given order
        /// </summary>
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order, string type, string description,
            decimal amount) => InstantiateOrderCharge(order, type, description, amount);

        /// <summary>
        /// Create a note for the given order
        /// </summary>
        Task<NoteEntity> IOrderElementFactory.CreateNote(OrderEntity order, string noteText, DateTime noteDate,
            NoteVisibility noteVisibility, bool ignoreDuplicateText = false) => InstantiateNote(order, noteText, noteDate, noteVisibility, ignoreDuplicateText);

        /// <summary>
        /// Crate a payment for the given order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order) =>
            InstantiateOrderPaymentDetail(order);

        /// <summary>
        /// Create a payment for the given order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order, string label, string value) =>
            InstantiateOrderPaymentDetail(order, label, value);
        #endregion
    }
}
