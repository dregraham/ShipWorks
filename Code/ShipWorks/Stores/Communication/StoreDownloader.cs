using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Web.Services3.Addressing;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.UI;
using ShipWorks.Stores.Content;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Templates.Processing;
using System.Diagnostics;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Platforms;
using System.Linq;
using ShipWorks.Templates.Tokens;
using ShipWorks.Data.Model;
using ShipWorks.Actions;
using Interapptive.Shared.Business;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Base that all store types must implement to provide downloading store data
    /// </summary>
    public abstract class StoreDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(StoreDownloader));

        StoreEntity store;
        IProgressReporter progress;
        long downloadLogID;

        StoreType storeType;

        int quantitySaved = 0;
        int quantityNew = 0;
        private AddressAdapter originalShippingAddress;
        private AddressAdapter originalBillingAddress;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreDownloader(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
            this.storeType = StoreTypeManager.GetType(store);
        }

        /// <summary>
        /// The store the downloader downloads from
        /// </summary>
        public StoreEntity Store
        {
            get
            {
                return store;
            }
        }

        /// <summary>
        /// The StoreType instance for the store
        /// </summary>
        protected StoreType StoreType
        {
            get
            {
                return storeType;
            }
        }

        /// <summary>
        /// Gets the address validation setting.
        /// </summary>
        private AddressValidationStoreSettingType addressValidationSetting
        {
            get
            {
                AddressValidationStoreSettingType storeSetting = ((AddressValidationStoreSettingType)store.AddressValidationSetting);
                return storeSetting;
            }
        }

        /// <summary>
        /// The progress reporting interface used to report progress and check cancelation.
        /// </summary>
        public IProgressReporter Progress
        {
            get
            {
                return progress;
            }
        }

        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        public int QuantitySaved
        {
            get
            {
                return quantitySaved;
            }
        }

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        public int QuantityNew
        {
            get
            {
                return quantityNew;
            }
        }

        /// <summary>
        /// Download data from the configured store.
        /// </summary>
        public void Download(IProgressReporter progress, long downloadLogID)
        {
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            if (this.progress != null)
            {
                throw new InvalidOperationException("Download should only be called once per instance.");
            }

            this.progress = progress;
            this.downloadLogID = downloadLogID;

            Download();
        }

        /// <summary>
        /// Must be implemented by derived types to do the actual download
        /// </summary>
        protected abstract void Download();

        /// <summary>
        /// Gets the largest last modified time we have in our database for non-manual orders for this store.
        /// If no such orders exist, and there is an initial download policy, that policy is applied.  Otherwise null is returned.
        /// </summary>
        protected DateTime? GetOnlineLastModifiedStartingPoint()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OnlineLastModified,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result is DBNull ? null : (DateTime?)result;

                log.InfoFormat("MAX(OnlineLastModified) = {0:u}", dateTime);

                // If we don't have a max, but do have a days-back policy, use the days back
                if (dateTime == null && store.InitialDownloadDays != null)
                {
                    // Also add on 2 hours just to make sure we are in range
                    dateTime = DateTime.UtcNow.AddDays(-store.InitialDownloadDays.Value).AddHours(2);
                    log.InfoFormat("MAX(OnlineLastModified) adjusted by download policy = {0:u}", dateTime);
                }

                // Dates pulled from the database are always UTC
                if (dateTime != null && dateTime.Value.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
                }

                return dateTime;
            }
        }

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it 
        /// will be used to calculate the initial number of days back to to.
        /// </summary>
        protected DateTime? GetOrderDateStartingPoint()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderDate,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result is DBNull ? null : (DateTime?)result;

                log.InfoFormat("MAX(OrderDate) = {0:u}", dateTime);

                // If we don't have a max, but do have a days-back policy, use the days back
                if (dateTime == null && store.InitialDownloadDays != null)
                {
                    // Also add on 2 hours just to make sure we are in range
                    dateTime = DateTime.UtcNow.AddDays(-store.InitialDownloadDays.Value).AddHours(2);
                    log.InfoFormat("MAX(OrderDate) adjusted by download policy = {0:u}", dateTime);
                }

                // Dates pulled from the database are always UTC
                if (dateTime != null && dateTime.Value.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
                }

                return dateTime;
            }
        }

        /// <summary>
        /// Gets the largest OrderNumber we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then if there is an InitialDownloadPolicy it is applied.  Otherwise, 0 is returned.
        /// </summary>
        /// <returns></returns>
        protected long GetOrderNumberStartingPoint()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderNumber,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == store.StoreID & OrderFields.IsManual == false);

                long orderNumber;

                if (result is DBNull)
                {
                    if (store.InitialDownloadOrder != null)
                    {
                        // We have to subtract one b\c the downloader expects the starting point to be the max order number in the db.  So what 
                        // it does is download all orders AFTER it.  But for the initial download policy, we want to START with it.  So we have
                        // to backoff by one to include it.
                        orderNumber = Math.Max(0, store.InitialDownloadOrder.Value - 1);
                        log.InfoFormat("Max(OrderNumber) - applying initial download polcy.");
                    }
                    else
                    {
                        orderNumber = 0;
                    }
                }
                else
                {
                    orderNumber = (long) result;
                }

                log.InfoFormat("MAX(OrderNumber) = {0}", orderNumber);

                return orderNumber;
            }
        }

        /// <summary>
        /// Gets the next OrderNumber that an order should use.  This is useful for storetypes that don't supply their own
        /// order numbers for ShipWorks, such as Amazon and eBay.
        /// </summary>
        protected long GetNextOrderNumber()
        {
            return OrderUtility.GetNextOrderNumber(store.StoreID);
        }

        /// <summary>
        /// Instantiates the order identified by the given identifier.  If no order exists in the database,
        /// a new one is initialized, created, and returned.  If the order does exist in the database,
        /// that order is returned.
        /// </summary>
        protected OrderEntity InstantiateOrder(OrderIdentifier orderIdentifier)
        {
            if (orderIdentifier == null)
            {
                throw new ArgumentNullException("orderIdentifier");
            }

            // Try to find an existing order
            OrderEntity order = FindOrder(orderIdentifier);

            if (order != null)
            {
                log.InfoFormat("Found existing {0}", orderIdentifier);

                originalShippingAddress = new AddressAdapter();
                AddressAdapter.Copy(order, "Ship", originalShippingAddress);

                originalBillingAddress = new AddressAdapter();
                AddressAdapter.Copy(order, "Bill", originalBillingAddress);

                return order;
            }
            else
            {
                log.InfoFormat("{0} not found, creating", orderIdentifier);

                // Create a new order
                order = storeType.CreateOrder();

                // Its for this store
                order.StoreID = store.StoreID;

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
                order.ShipSenseRecognitionStatus = (int)ShipSenseOrderRecognitionStatus.NotRecognized;
            }

            return order;
        }

        /// <summary>
        /// Find the order with the configured OrderNumber.  If no order exists, null is returned.
        /// </summary>
        protected virtual OrderEntity FindOrder(OrderIdentifier orderIdentifier)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket(OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                // We use a prototype approach to determine what to search for
                OrderEntity prototype = storeType.CreateOrder();
                prototype.IsNew = false;
                prototype.RollbackChanges();
                orderIdentifier.ApplyTo(prototype);

                // We are searching for all the fields that have changed (the ones the OrderIdentifier applied)
                foreach (EntityField2 field in prototype.Fields)
                {
                    if (field.IsChanged)
                    {
                        bucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(field, null, ComparisonOperator.Equal, field.CurrentValue));
                    }
                }

                OrderEntity order = adapter.FetchNewEntity<OrderEntity>(bucket);

                if (order.IsNew)
                {
                    return null;
                }
                else
                {
                    return order;
                }
            }
        }

        /// <summary>
        /// Create a new OrderItem based on the given OrderEntity
        /// </summary>
        protected OrderItemEntity InstantiateOrderItem(OrderEntity order)
        {
            OrderItemEntity item = storeType.CreateOrderItemInstance();
            item.Order = order;

            // Downloaded items are assumed not manua
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
            OrderItemAttributeEntity attribute = storeType.CreateOrderItemAttributeInstance();
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
        protected NoteEntity InstantiateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility visibility, bool ignoreDuplicateText = false)
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
                    && n.Source == (int)NoteSource.Downloaded))
                {
                    return null;
                }

                // If the order isn't new, check the ones in the database too
                if (!order.IsNew)
                {
                    IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
                    relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Text, null, ComparisonOperator.Equal, noteText));
                    relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Source, null, ComparisonOperator.Equal, (int)NoteSource.Downloaded));

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
            note.Edited = noteDate;
            note.Source = (int) NoteSource.Downloaded;
            note.Visibility = (int) visibility;
            note.Text = noteText;

            return note;
        }

        /// <summary>
        /// Save the given order that has been downloaded.
        /// </summary>
        protected void SaveDownloadedOrder(OrderEntity order)
        {
            Stopwatch sw = Stopwatch.StartNew();

            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            ConfigurationEntity config = ConfigurationData.Fetch();

            string orderStatusText = StatusPresetManager.GetStoreDefault(store, StatusPresetTarget.Order).StatusText;
            string itemStatusText = StatusPresetManager.GetStoreDefault(store, StatusPresetTarget.OrderItem).StatusText;

            bool orderStatusHasTokens = TemplateTokenProcessor.HasTokens(orderStatusText);
            bool itemStatusHasTokens = TemplateTokenProcessor.HasTokens(itemStatusText);

            List<OrderItemEntity> newOrderItems = order.OrderItems.Where(i => i.IsNew).ToList();

            // Update the address casing of the order
            if (config.AddressCasing)
            {
                ApplyAddressCasing(order);
            }

            // if the downloaders specified they Parsed the name, also put the name in the unparsed field
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
            OrderIdentifier orderIdentifier = storeType.CreateOrderIdentifier(order);

            // Now we have to see if it was new
            bool alreadyDownloaded = HasDownloadHistory(orderIdentifier);

            // Only audit new orders if new order auditing is turned on.  This also turns off auditing of creating of new customers if the order is not new.
            using (AuditBehaviorScope auditScope = CreateOrderAuditScope(order))
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Get the customer
                    if (order.IsNew)
                    {
                        try
                        {
                            order.CustomerID = CustomerProvider.AcquireCustomer(order, storeType, adapter);
                        }
                        catch (CustomerAcquisitionLockException)
                        {
                            throw new DownloadException("ShipWorks was unable to find the customer in the time allotted.  Please try downloading again.");
                        }
                    }

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
                    if (order.IsNew && string.IsNullOrEmpty(order.LocalStatus) && !orderStatusHasTokens)
                    {
                        order.LocalStatus = orderStatusText;
                    }

                    // Apply default status to items.  If tokenized, it has to be done after the save.
                    if (!itemStatusHasTokens)
                    {
                        foreach (OrderItemEntity item in newOrderItems)
                        {
                            // Don't overwrite what the downloader set
                            if (string.IsNullOrEmpty(item.LocalStatus))
                            {
                                item.LocalStatus = itemStatusText;
                            }
                        }
                    }

                    // If it's new and LastModified isn't set, use the date
                    if (order.IsNew && !order.Fields[(int)OrderFieldIndex.OnlineLastModified].IsChanged)
                    {
                        order.OnlineLastModified = order.OrderDate;
                    }

                    // Calculate or verify the order total
                    VerifyOrderTotal(order);

                    // Save the order so we can get its OrderID
                    adapter.SaveAndRefetch(order);

                    // Update the note counts
                    NoteManager.AdjustNoteCount(adapter, order.OrderID, order.Notes.Select(n => n.IsNew).Count());

                    // Apply default order status, if it contained tokens it has to be after the save.  Don't overwrite what the downloader set.
                    if (order.IsNew && string.IsNullOrEmpty(order.LocalStatus) && orderStatusHasTokens)
                    {
                        order.LocalStatus = TemplateTokenProcessor.ProcessTokens(orderStatusText, order.OrderID);
                        adapter.SaveAndRefetch(order);
                    }

                    // Apply default item status, if it contained token it has to be after the save and out of the transaction.
                    if (itemStatusHasTokens)
                    {
                        foreach (OrderItemEntity item in newOrderItems)
                        {
                            // Don't overwrite what the downloader set
                            if (string.IsNullOrEmpty(item.LocalStatus))
                            {
                                item.LocalStatus = TemplateTokenProcessor.ProcessTokens(itemStatusText, item.OrderItemID);
                                adapter.SaveAndRefetch(item);
                            }
                        }
                    }

                    // Everything has been set on the order, so calculate the hash key
                    OrderUtility.PopulateOrderDetails(order, adapter);
                    OrderUtility.UpdateShipSenseHashKey(order);
                    adapter.SaveAndRefetch(order);
					
					// Update unprocessed shipment addresses if the order address has changed
                    if (!order.IsNew)
                    {
                        AddressAdapter newShippingAddress = new AddressAdapter(order, "Ship");
                        bool shippingAddressChanged = originalShippingAddress != newShippingAddress;
                        if (shippingAddressChanged)
                        {
                            SetAddressValidationStatus(order, "Ship", adapter);
                            adapter.SaveAndRefetch(order);

                            ValidatedAddressManager.PropagateAddressChangesToShipments(adapter, order.OrderID, originalShippingAddress, newShippingAddress);
                        }

                        // Update the customer's addresses if necessary
                        AddressAdapter newBillingAddress = new AddressAdapter(order, "Bill");
                        bool billingAddressChanged = originalBillingAddress != newBillingAddress;

                        if (billingAddressChanged)
                        {
                            SetAddressValidationStatus(order, "Bill", adapter);
                            adapter.SaveAndRefetch(order);
                        }

                        // Don't even bother loading the customer if the addresses haven't changed, or if we shouldn't copy
                        if ((billingAddressChanged && config.CustomerUpdateModifiedBilling != (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy)
                            || (shippingAddressChanged && config.CustomerUpdateModifiedShipping != (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy))
                        {
                            CustomerEntity existingCustomer = DataProvider.GetEntity(order.CustomerID) as CustomerEntity;
                            if (existingCustomer != null)
                            {
                                UpdateCustomerAddressIfNecessary(billingAddressChanged, (ModifiedOrderCustomerUpdateBehavior)config.CustomerUpdateModifiedBilling, order, existingCustomer, originalBillingAddress, "Bill");
                                UpdateCustomerAddressIfNecessary(shippingAddressChanged, (ModifiedOrderCustomerUpdateBehavior)config.CustomerUpdateModifiedShipping, order, existingCustomer, originalShippingAddress, "Ship");

                                adapter.SaveEntity(existingCustomer);
                            }
                        }
                    }
                    else
                    {
                        SetAddressValidationStatus(order, "Ship", adapter);
                        SetAddressValidationStatus(order, "Bill", adapter);
                        adapter.SaveAndRefetch(order);
                    }
                    
                    log.InfoFormat("{0} is {1} new", orderIdentifier, alreadyDownloaded ? "not " : "");

                    // Log this download
                    AddToDownloadHistory(order, orderIdentifier, alreadyDownloaded, adapter);

                    // Dispatch the order downloaded action
                    ActionDispatcher.DispatchOrderDownloaded(order, !alreadyDownloaded, adapter);

                    adapter.Commit();
                }
            }

            quantitySaved++;

            if (!alreadyDownloaded)
            {
                quantityNew++;
            }

            log.InfoFormat("Committed order: {0}", sw.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Sets the address validation status on the order, depending on the store settings
        /// </summary>
        private void SetAddressValidationStatus(OrderEntity order, string prefix, SqlAdapter adapter)
        {
            AddressAdapter address = new AddressAdapter(order, prefix);

            address.POBox = (int)ValidationDetailStatusType.Unknown;
            address.MilitaryAddress = (int)ValidationDetailStatusType.Unknown;
            address.USTerritory = (int)ValidationDetailStatusType.Unknown;
            address.ResidentialStatus = (int)ValidationDetailStatusType.Unknown;
            address.AddressValidationSuggestionCount = 0;
            address.AddressValidationError = string.Empty;

            ValidatedAddressManager.DeleteExistingAddresses(adapter, order.OrderID, prefix);

            if (ValidatedAddressManager.EnsureAddressCanBeValidated(address))
            {
                if ((addressValidationSetting == AddressValidationStoreSettingType.ValidateAndApply ||
                     addressValidationSetting == AddressValidationStoreSettingType.ValidateAndNotify) &&
                    prefix == "Ship")
                {
                    address.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
                }
                else
                {
                    address.AddressValidationStatus = (int)AddressValidationStatusType.NotChecked;
                }
            }
        }
        /// <summary>
        /// Update's the customer's address from an order, if it's necessary
        /// </summary>
        private static void UpdateCustomerAddressIfNecessary(bool shouldUpdate, ModifiedOrderCustomerUpdateBehavior behavior, OrderEntity order, CustomerEntity existingCustomer, AddressAdapter originalAddress, string prefix)
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
                    if (IsAddressEmpty(existingCustomer,prefix))
                    {
                        shouldCopy = true;
                    } 
                    else if (originalAddress==null || originalAddress.Equals(new PersonAdapter(existingCustomer, prefix)))
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
        /// Create the proper AuditBehaviorScope to used based on the configuration of auditing new orders.
        /// </summary>
        public static AuditBehaviorScope CreateOrderAuditScope(OrderEntity order)
        {
            ConfigurationEntity config = ConfigurationData.Fetch();

            return new AuditBehaviorScope((config.AuditNewOrders || !order.IsNew) ? AuditState.Default : AuditState.NoDetails);
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
                    order.SetNewFieldValue(fieldIndex, AddressCasing.Apply((string)order.GetCurrentFieldValue(fieldIndex)));
                }
            }
        }

        /// <summary>
        /// If the OrderTotal is set, the total is recalculated and verified.  If the verification fails, an assertion is raised.
        /// If the OrderTotal is not set, and the order is new, an error is raised.
        /// </summary>
        protected virtual void VerifyOrderTotal(OrderEntity order)
        {
            if (order.Fields[(int)OrderFieldIndex.OrderTotal].IsChanged)
            {
                decimal total = OrderUtility.CalculateTotal(order);

                Debug.Assert(total == order.OrderTotal);
            }
            else if (order.IsNew)
            {
                Debug.Fail("New order does not have OrderTotal set.");
            }
        }

        /// <summary>
        /// Indicates if this order is already present in the download history.
        /// </summary>
        private bool HasDownloadHistory(OrderIdentifier orderIdentifier)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(DownloadFields.StoreID == store.StoreID);
            bucket.Relations.Add(DownloadDetailEntity.Relations.DownloadEntityUsingDownloadID);

            // We make a prototype history that will allow us to determinate what to search for
            DownloadDetailEntity history = new DownloadDetailEntity();
            orderIdentifier.ApplyTo(history);

            // We are searching for all the fields that have changed (the ones the OrderIdentifier applied)
            foreach (EntityField2 field in history.Fields)
            {
                if (field.IsChanged)
                {
                    bucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(field, null, ComparisonOperator.Equal, field.CurrentValue));
                }
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                int count = adapter.GetDbCount(new DownloadDetailEntityFactory().CreateFields(), bucket);

                return count > 0;
            }
        }

        /// <summary>
        /// Add the given order to the download history
        /// </summary>
        private void AddToDownloadHistory(OrderEntity order, OrderIdentifier orderIdentifier, bool alreadyDownloaded, SqlAdapter adapter)
        {
            DownloadDetailEntity history = new DownloadDetailEntity();
            history.DownloadID = downloadLogID;
            history.OrderID = order.OrderID;
            history.InitialDownload = !alreadyDownloaded;

            orderIdentifier.ApplyTo(history);

            adapter.SaveEntity(history);
        }
    }
}
