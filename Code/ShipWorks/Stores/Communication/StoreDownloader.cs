using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Actions;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Base that all store types must implement to provide downloading store data
    /// </summary>
    public abstract class StoreDownloader
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreDownloader));

        private long downloadLogID;
        protected DbConnection connection;
        private readonly IConfigurationEntity config;
        private string orderStatusText = string.Empty;
        private string itemStatusText = string.Empty;
        private bool orderStatusHasTokens = false;
        private bool itemStatusHasTokens = false;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader(StoreEntity store) : this(store, StoreTypeManager.GetType(store))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader([Obfuscation(Exclude = true)] StoreEntity store, StoreType storeType)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            Store = store;
            StoreType = storeType;
            config = ConfigurationData.FetchReadOnly();
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
        public StoreEntity Store { get; private set; }

        /// <summary>
        /// The StoreType instance for the store
        /// </summary>
        protected StoreType StoreType { get; private set; }

        /// <summary>
        /// Gets the address validation setting.
        /// </summary>
        private AddressValidationStoreSettingType addressValidationSetting =>
            (AddressValidationStoreSettingType) Store.AddressValidationSetting;

        /// <summary>
        /// The progress reporting interface used to report progress and check cancellation.
        /// </summary>
        public IProgressReporter Progress { get; private set; }

        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        public int QuantitySaved { get; private set; }

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        public int QuantityNew { get; private set; }

        /// <summary>
        /// Download data from the configured store.
        /// </summary>
        public async Task Download(IProgressReporter progress, long downloadLogID, DbConnection connection)
        {
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            if (Progress != null)
            {
                throw new InvalidOperationException("Download should only be called once per instance.");
            }

            orderStatusText = StatusPresetManager.GetStoreDefault(Store, StatusPresetTarget.Order).StatusText;
            itemStatusText = StatusPresetManager.GetStoreDefault(Store, StatusPresetTarget.OrderItem).StatusText;
            orderStatusHasTokens = TemplateTokenProcessor.HasTokens(orderStatusText);
            itemStatusHasTokens = TemplateTokenProcessor.HasTokens(itemStatusText);

            Progress = progress;
            this.downloadLogID = downloadLogID;
            this.connection = connection;

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                await Download(trackedDurationEvent).ConfigureAwait(false);

                trackedDurationEvent.AddProperty("Store.Type", EnumHelper.GetDescription(StoreType.TypeCode));
                trackedDurationEvent.AddMetric("Orders.Total", QuantitySaved);
                trackedDurationEvent.AddMetric("Orders.New", QuantityNew);
            }
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
        /// Gets the next OrderNumber that an order should use.  This is useful for store types that don't supply their own
        /// order numbers for ShipWorks, such as Amazon and eBay.
        /// </summary>
        protected long GetNextOrderNumber() => OrderUtility.GetNextOrderNumber(Store.StoreID);

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        protected virtual GenericResult<OrderEntity> InstantiateOrder(long orderNumber) =>
            InstantiateOrder(new OrderNumberIdentifier(orderNumber));

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        protected virtual GenericResult<OrderEntity> InstantiateOrder(OrderIdentifier orderIdentifier)
        {
            if (orderIdentifier == null)
            {
                throw new ArgumentNullException(nameof(orderIdentifier));
            }

            // Try to find an existing order
            OrderEntity order = FindOrder(orderIdentifier);

            if (order != null)
            {
                log.Debug($"Found existing {orderIdentifier}");

                ShippingAddressBeforeDownload = new AddressAdapter();
                AddressAdapter.Copy(order, "Ship", ShippingAddressBeforeDownload);

                BillingAddressBeforeDownload = new AddressAdapter();
                AddressAdapter.Copy(order, "Bill", BillingAddressBeforeDownload);

                return GenericResult.FromSuccess(order);
            }

            if (IsCombinedOrder(orderIdentifier))
            {
                log.InfoFormat("{0} was combined, skipping", orderIdentifier);

                return GenericResult.FromError<OrderEntity>("Combined");
            }

            order = CreateOrder(orderIdentifier);
            return GenericResult.FromSuccess(order);
        }

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

            // Rollup defaults
            order.RollupNoteCount = 0;
            order.RollupItemCount = 0;
            order.RollupItemTotalWeight = 0;

            order.ShipSenseHashKey = string.Empty;
            order.ShipSenseRecognitionStatus = (int) ShipSenseOrderRecognitionStatus.NotRecognized;

            return order;
        }

        private bool IsCombinedOrder(OrderIdentifier orderIdentifier)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create();

                QueryFactory factory = new QueryFactory();
                QuerySpec combinedSearchQuery = orderIdentifier.CreateCombinedSearchQuery(factory);
                DynamicQuery query = factory.Create().Select(combinedSearchQuery.Any());

                return sqlAdapter.FetchScalar<bool?>(query) ?? false;
            }
        }

        /// <summary>
        /// Find the order with the configured OrderNumber.  If no order exists, null is returned.
        /// </summary>
        protected virtual OrderEntity FindOrder(OrderIdentifier orderIdentifier)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                RelationPredicateBucket bucket =
                    new RelationPredicateBucket(OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                // We use a prototype approach to determine what to search for
                OrderEntity prototype = StoreType.CreateOrder();
                prototype.IsNew = false;
                prototype.RollbackChanges();
                orderIdentifier.ApplyTo(prototype);

                // We are searching for all the fields that have changed (the ones the OrderIdentifier applied)
                foreach (EntityField2 field in prototype.Fields)
                {
                    if (field.IsChanged)
                    {
                        bucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(field, null,
                            ComparisonOperator.Equal, field.CurrentValue));
                    }
                }

                OrderEntity order = adapter.FetchNewEntity<OrderEntity>(bucket);

                if (order.IsNew)
                {
                    return null;
                }
                else
                {
                    OrderUtility.PopulateOrderDetails(order);
                    return order;
                }
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
        /// Create a new order charge based on the given order
        /// </summary>
        protected OrderChargeEntity InstantiateOrderCharge(OrderEntity order)
        {
            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Order = order;

            return charge;
        }

        /// <summary>
        /// Create a new order charge based on the given order, type, description and amount
        /// </summary>
        protected OrderChargeEntity InstantiateOrderCharge(OrderEntity order, string type, string description, decimal amount)
        {
            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Order = order;
            charge.Type = type;
            charge.Description = description;
            charge.Amount = amount;

            return charge;
        }

        /// <summary>
        /// Create a new payment detail based on the given order
        /// </summary>
        protected OrderPaymentDetailEntity InstantiateOrderPaymentDetail(OrderEntity order)
        {
            OrderPaymentDetailEntity detail = new OrderPaymentDetailEntity();
            detail.Order = order;

            return detail;
        }

        /// <summary>
        /// Creates a new note instance, but only if the note text is non-blank.  If its blank, null is returned.
        /// </summary>
        protected NoteEntity InstantiateNote(OrderEntity order, string noteText, DateTime? noteDate,
            NoteVisibility visibility, bool ignoreDuplicateText = false)
        {
            if (string.IsNullOrWhiteSpace(noteText))
            {
                return null;
            }

            noteText = noteText.Trim();

            // If we need to ignore adding any notes that are dupes of ones that exist...
            if (ignoreDuplicateText)
            {
                // First see if any of the current (newly downloaded) notes match this note
                if (order.Notes.Any(n =>
                    string.Compare(n.Text, noteText, true) == 0
                    && n.Source == (int) NoteSource.Downloaded))
                {
                    return null;
                }

                // If the order isn't new, check the ones in the database too
                if (!order.IsNew)
                {
                    IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
                    relationPredicateBucket.PredicateExpression.AddWithAnd(
                        new FieldCompareValuePredicate(NoteFields.Text, null, ComparisonOperator.Equal, noteText));
                    relationPredicateBucket.PredicateExpression.AddWithAnd(
                        new FieldCompareValuePredicate(NoteFields.Source, null, ComparisonOperator.Equal,
                            (int) NoteSource.Downloaded));

                    using (EntityCollection<NoteEntity> notes = new EntityCollection<NoteEntity>())
                    {
                        int matchingNotes = SqlAdapter.Default.GetDbCount(notes, relationPredicateBucket);

                        if (matchingNotes > 0)
                        {
                            return null;
                        }
                    }
                }
            }

            // Instantiate the note
            NoteEntity note = new NoteEntity();
            note.Order = order;
            note.UserID = null;
            note.Edited = noteDate ?? order.OrderDate;
            note.Source = (int) NoteSource.Downloaded;
            note.Visibility = (int) visibility;
            note.Text = noteText;

            return note;
        }

        /// <summary>
        /// Save the given order that has been downloaded.
        /// </summary>
        protected virtual async Task SaveDownloadedOrder(OrderEntity order)
        {
            using (DbTransaction transaction = connection.BeginTransaction())
            {
                await SaveDownloadedOrder(order, transaction).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Save the given order that has been downloaded.
        /// </summary>
        protected virtual async Task SaveDownloadedOrder(OrderEntity order, DbTransaction transaction)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

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

                    ResetAddressIfRequired(order.IsNew, order, transaction);

                    using (SqlAdapter adapter = new SqlAdapter(connection, transaction))
                    {
                        if (order.IsNew)
                        {
                            await SaveNewOrder(order, adapter).ConfigureAwait(false);
                        }
                        else
                        {
                            await SaveExistingOrder(order, adapter).ConfigureAwait(false);
                        }

                        //alreadyDownloaded.Wait();
                        isAlreadyDownloaded = await alreadyDownloaded.ConfigureAwait(false);
                        log.InfoFormat("{0} is {1} new", orderIdentifier, isAlreadyDownloaded ? "not " : "");

                        // Log this download
                        AddToDownloadHistory(order.OrderID, orderIdentifier, isAlreadyDownloaded, adapter);

                        // Dispatch the order downloaded action
                        ActionDispatcher.DispatchOrderDownloaded(order.OrderID, Store.StoreID, !isAlreadyDownloaded, adapter);

                        adapter.Commit();
                    }

                    QuantitySaved++;

                    if (!isAlreadyDownloaded)
                    {
                        QuantityNew++;
                    }
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
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethod]
        private async Task SaveNewOrder(OrderEntity order, SqlAdapter adapter)
        {
            if (!order.IsNew)
            {
                await SaveExistingOrder(order, adapter).ConfigureAwait(false);
                return;
            }

            // Start getting the customer asynchronously since we don't need it until right before we save.
            Task<CustomerEntity> getCustomerTask = GetCustomer(order, adapter);

            // Protect payment details
            foreach (OrderPaymentDetailEntity detail in order.OrderPaymentDetails)
            {
                if (detail.IsNew)
                {
                    PaymentDetailSecurity.Protect(detail);
                }
            }

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

            // Wait for the customer to be found or created
            CustomerEntity customer = await getCustomerTask.ConfigureAwait(false);

            // Update the note counts
            AdjustNoteCount(order, customer);

            try
            {
                // If note count changed, save the customer
                if (customer.IsDirty)
                {
                    adapter.SaveEntity(customer, false);
                }

                order.CustomerID = customer.CustomerID;

                // Save the order so we can get its OrderID
                adapter.SaveEntity(order, false);
            }
            catch (ORMQueryExecutionException ex)
                when (ex.Message.Contains("SqlDateTime overflow", StringComparison.OrdinalIgnoreCase))
            {
                throw new DownloadException(
                    $"Order {order.OrderNumber} has an invalid Order Date and/or Last Modified Online date/time. " +
                    "Please ensure that these values are between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.");
            }

            bool orderChanged = false;
            string tmpLocalStatus = string.Empty;

            // Apply default order status, if it contained tokens it has to be after the save.  Don't overwrite what the downloader set.
            if (orderStatusHasTokens && string.IsNullOrEmpty(order.LocalStatus))
            {
                tmpLocalStatus = TemplateTokenProcessor.ProcessTokens(orderStatusText, order.OrderID);
                if (!order.LocalStatus.Equals(tmpLocalStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    order.LocalStatus = tmpLocalStatus;
                    orderChanged = true;
                }
            }

            // Apply default item status, if it contained token it has to be after the save and out of the transaction.
            if (itemStatusHasTokens)
            {
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
                adapter.SaveEntity(order);
            }
        }

        /// <summary>
        /// Get a customer for the order
        /// </summary>
        private async Task<CustomerEntity> GetCustomer(OrderEntity order, SqlAdapter adapter)
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
        [NDependIgnoreLongMethod]
        private async Task SaveExistingOrder(OrderEntity order, SqlAdapter adapter)
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

            List<OrderItemEntity> newOrderItems = order.OrderItems.Where(i => i.IsNew).ToList();

            // Apply default item status, if it contained token it has to be after the save and out of the transaction.
            if (itemStatusHasTokens)
            {
                foreach (OrderItemEntity item in newOrderItems)
                {
                    // Don't overwrite what the downloader set
                    if (string.IsNullOrEmpty(item.LocalStatus))
                    {
                        item.LocalStatus = TemplateTokenProcessor.ProcessTokens(itemStatusText, item.OrderItemID);
                    }
                }
            }
            else
            {
                // Apply default status to items.  If tokenized, it has to be done after the save.
                foreach (OrderItemEntity item in newOrderItems)
                {
                    // Don't overwrite what the downloader set
                    if (string.IsNullOrEmpty(item.LocalStatus))
                    {
                        item.LocalStatus = itemStatusText;
                    }
                }
            }

            // Everything has been set on the order, so calculate the hash key
            OrderUtility.UpdateShipSenseHashKey(order);

            // Update unprocessed shipment addresses if the order address has changed
            AddressAdapter newShippingAddress = new AddressAdapter(order, "Ship");
            bool shippingAddressChanged = ShippingAddressBeforeDownload != newShippingAddress;
            if (shippingAddressChanged)
            {
                SetAddressValidationStatus(order, false, "Ship", adapter);
                adapter.SaveAndRefetch(order);

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
                    UpdateCustomerAddressIfNecessary(billingAddressChanged, (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedBilling, order, customer, BillingAddressBeforeDownload, "Bill");
                    UpdateCustomerAddressIfNecessary(shippingAddressChanged, (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedShipping, order, customer, ShippingAddressBeforeDownload, "Ship");
                }
            }

            // Update the note counts
            customer = customer ?? new CustomerEntity(order.CustomerID) { IsNew = false };
            AdjustNoteCount(order, customer);

            adapter.SaveEntity(customer, false);
            adapter.SaveEntity(order, true);
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
            else
            {
                order.Fields[(int) OrderFieldIndex.RollupNoteCount].ExpressionToApply = OrderFields.RollupNoteCount + noteCount;
                order.IsDirty = true;
            }

            if (customer.IsNew)
            {
                customer.RollupNoteCount = noteCount;
            }
            else
            {
                customer.Fields[(int) CustomerFieldIndex.RollupNoteCount].ExpressionToApply = CustomerFields.RollupNoteCount + noteCount;
                customer.IsDirty = true;
            }
        }

        /// <summary>
        /// If an order's addresses change to the originally validated address, change it back.
        /// </summary>
        private void ResetAddressIfRequired(bool isOrderNew, OrderEntity order, DbTransaction transaction)
        {
            if (!isOrderNew)
            {
                using (SqlAdapter adapter = new SqlAdapter(connection, transaction))
                {
                    bool shipAddressReset = ResetAddressIfRequired(order, "Ship", ShippingAddressBeforeDownload);
                    bool billAddressReset = ResetAddressIfRequired(order, "Bill", BillingAddressBeforeDownload);
                    if (shipAddressReset || billAddressReset)
                    {
                        adapter.SaveAndRefetch(order);
                    }
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
        private static bool ResetAddressIfRequired(OrderEntity order, string prefix, AddressAdapter addressBeforeDownload)
        {
            if (addressBeforeDownload == null)
            {
                return false;
            }

            bool addressReset = false;
            AddressAdapter orderAddress = new AddressAdapter(order, prefix);

            if (addressBeforeDownload != orderAddress)
            {
                ValidatedAddressEntity addressBeforeValidation =
                    ValidatedAddressManager.GetOriginalAddress(SqlAdapter.Default, order.OrderID, prefix);

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

            return addressReset;
        }

        /// <summary>
        /// Sets the address validation status on the order, depending on the store settings
        /// </summary>
        private void SetAddressValidationStatus(OrderEntity order, bool isNewOrder, string prefix, SqlAdapter adapter)
        {
            AddressAdapter address = new AddressAdapter(order, prefix);

            address.POBox = (int) ValidationDetailStatusType.Unknown;
            address.MilitaryAddress = (int) ValidationDetailStatusType.Unknown;
            address.USTerritory = (int) ValidationDetailStatusType.Unknown;
            address.ResidentialStatus = (int) ValidationDetailStatusType.Unknown;
            address.AddressValidationSuggestionCount = 0;
            address.AddressValidationError = string.Empty;
            address.AddressType = (int) AddressType.NotChecked;

            if (!isNewOrder)
            {
                ValidatedAddressManager.DeleteExistingAddresses(adapter, order.OrderID, prefix);
            }

            if (ValidatedAddressManager.EnsureAddressCanBeValidated(address))
            {
                if ((addressValidationSetting == AddressValidationStoreSettingType.ValidateAndApply ||
                     addressValidationSetting == AddressValidationStoreSettingType.ValidateAndNotify) &&
                    prefix == "Ship")
                {
                    address.AddressValidationStatus = (int) AddressValidationStatusType.Pending;
                }
                else
                {
                    address.AddressValidationStatus = (int) AddressValidationStatusType.NotChecked;
                }
            }
        }
        /// <summary>
        /// Update's the customer's address from an order, if it's necessary
        /// </summary>
        [NDependIgnoreTooManyParams]
        private static void UpdateCustomerAddressIfNecessary(bool shouldUpdate, ModifiedOrderCustomerUpdateBehavior behavior,
            OrderEntity order, CustomerEntity existingCustomer, AddressAdapter originalAddress, string prefix)
        {
            if (!shouldUpdate || IsAddressEmpty(order, prefix))
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
                    if (IsAddressEmpty(existingCustomer, prefix))
                    {
                        shouldCopy = true;
                    }
                    else if (originalAddress == null || originalAddress.Equals(new PersonAdapter(existingCustomer, prefix)))
                    {
                        shouldCopy = true;
                    }
                    else
                    {
                        shouldCopy = false;
                    }
                    break;
                case ModifiedOrderCustomerUpdateBehavior.AlwaysCopy:
                    shouldCopy = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("behavior");
            }

            if (shouldCopy)
            {
                PersonAdapter.Copy(order, existingCustomer, prefix);
            }
        }

        /// <summary>
        /// Is the entity's address considered empty?
        /// </summary>
        private static bool IsAddressEmpty(EntityBase2 entity, string prefix)
        {
            PersonAdapter personAdapter = new PersonAdapter(entity, prefix);

            return string.IsNullOrEmpty(personAdapter.City) &&
                   string.IsNullOrEmpty(personAdapter.PostalCode);
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

            using (SqlAdapter adapter = new SqlAdapter())
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
        private void AddToDownloadHistory(long orderID, OrderIdentifier orderIdentifier, bool alreadyDownloaded, SqlAdapter adapter)
        {
            DownloadDetailEntity history = new DownloadDetailEntity();
            history.DownloadID = downloadLogID;
            history.OrderID = orderID;
            history.InitialDownload = !alreadyDownloaded;

            orderIdentifier.ApplyTo(history);

            adapter.SaveEntity(history);
        }
    }
}
