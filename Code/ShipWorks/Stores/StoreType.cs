using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Base class for all store types.  All points of extension to shipworks via store types are here.
    /// </summary>
    public abstract class StoreType
    {
        // Store this instance is wrapping
        private readonly StoreEntity store;
        private IStoreEntity storeReadOnly;

        protected delegate void StoreAddedHandler(StoreEntity store, ILifetimeScope lifetimeScope);
        protected event StoreAddedHandler StoreAdded;

        /// <summary>
        /// Construction
        /// </summary>
        protected StoreType() : this(null)
        {

        }

        /// <summary>
        /// Construction
        /// </summary>
        protected StoreType(StoreEntity store)
        {
            if (store != null)
            {
                if (store.StoreTypeCode != this.TypeCode)
                {
                    throw new InvalidOperationException("Store type mismatch.");
                }
            }

            this.store = store;
            this.storeReadOnly = store;
        }

        /// <summary>
        /// Initialize the default values for the given store
        /// </summary>
        protected virtual void InitializeStoreDefaults(StoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            store.Enabled = true;
            store.SetupComplete = false;
            store.Edition = "";

            store.TypeCode = (int) TypeCode;
            store.CountryCode = "US";

            store.AutoDownload = true;
            store.AutoDownloadMinutes = 15;
            store.AutoDownloadOnlyAway = false;

            store.ComputerDownloadPolicy = "";

            store.ManualOrderPrefix = "";
            store.ManualOrderPostfix = "-M";

            store.DefaultEmailAccountID = -1;

            store.DomesticAddressValidationSetting = GetDefaultDomesticValidationSetting();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndNotify;

            store.OrderSourceID = "";
            store.ShouldMigrate = true;
        }

        /// <summary>
        /// Gets the default domestic validation setting.
        /// </summary>
        protected virtual AddressValidationStoreSettingType GetDefaultDomesticValidationSetting()
        {
            return AddressValidationStoreSettingType.ValidateAndApply;
        }

        /// <summary>
        /// Gets and sets a read-only store to use with this type
        /// </summary>
        /// <remarks>
        /// This will only take effect if the Store property is null, which means a read only store
        /// cannot be set on a store type that already has a full store attached to it.  This is useful
        /// in situations where we need store data to make decisions but don't want to weight of doing
        /// the full StoreEntity collection clone.
        /// </remarks>
        public IStoreEntity StoreReadOnly
        {
            get => storeReadOnly;
            set
            {
                if (Store == null)
                {
                    if (value.StoreTypeCode != this.TypeCode)
                    {
                        throw new InvalidOperationException("Store type mismatch.");
                    }

                    storeReadOnly = value;
                }
            }
        }

        /// <summary>
        /// The Store instance, if any, that this type represents.
        /// </summary>
        public StoreEntity Store
        {
            get
            {
                return store;
            }
        }

        /// <summary>
        /// Display the StoreName
        /// </summary>
        public override string ToString()
        {
            return StoreTypeName;
        }

        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public abstract StoreTypeCode TypeCode { get; }

        /// <summary>
        /// User visible name of the store.
        /// </summary>
        public string StoreTypeName
        {
            get { return StoreTypeIdentity.FromCode(TypeCode).Name; }
        }

        /// <summary>
        /// Should this store type auto download
        /// </summary>
        public virtual bool IsOnDemandDownloadEnabled => false;

        /// <summary>
        /// Can this store be added via the SW client
        /// </summary>
        public virtual bool CanAddStoreType => true;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public abstract StoreEntity CreateStoreInstance();

        /// <summary>
        /// Create an order for this store
        /// </summary>
        public OrderEntity CreateOrder()
        {
            OrderEntity newOrder = CreateOrderInstance();

            newOrder.ShipAddressValidationError = string.Empty;
            newOrder.ShipResidentialStatus = (int) ValidationDetailStatusType.Unknown;
            newOrder.ShipPOBox = (int) ValidationDetailStatusType.Unknown;
            newOrder.ShipUSTerritory = (int) ValidationDetailStatusType.Unknown;
            newOrder.ShipMilitaryAddress = (int) ValidationDetailStatusType.Unknown;
            newOrder.ShipAddressValidationStatus = (int) AddressValidationStatusType.NotChecked;
            newOrder.ShipAddressValidationSuggestionCount = 0;
            newOrder.BillAddressValidationError = string.Empty;
            newOrder.BillResidentialStatus = (int) ValidationDetailStatusType.Unknown;
            newOrder.BillPOBox = (int) ValidationDetailStatusType.Unknown;
            newOrder.BillUSTerritory = (int) ValidationDetailStatusType.Unknown;
            newOrder.BillMilitaryAddress = (int) ValidationDetailStatusType.Unknown;
            newOrder.BillAddressValidationStatus = (int) AddressValidationStatusType.NotChecked;
            newOrder.BillAddressValidationSuggestionCount = 0;
            newOrder.ShipAddressType = (int) AddressType.NotChecked;

            newOrder.RequestedShipping = string.Empty;
            newOrder.CombineSplitStatus = CombineSplitStatusType.None;

            return newOrder;
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderEntity
        /// </summary>
        protected virtual OrderEntity CreateOrderInstance()
        {
            return new OrderEntity();
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderItemEntity
        /// </summary>
        /// <returns></returns>
        public virtual OrderItemEntity CreateOrderItemInstance() =>
            new OrderItemEntity();

        /// <summary>
        /// Creates a store-specific instance of OrderItemAttributeEntity
        /// </summary>
        public virtual OrderItemAttributeEntity CreateOrderItemAttributeInstance()
        {
            return new OrderItemAttributeEntity();
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public abstract OrderIdentifier CreateOrderIdentifier(IOrderEntity order);

        /// <summary>
        /// Get a description for use when auditing an order
        /// </summary>
        public virtual string GetAuditDescription(IOrderEntity order) => order?.OrderNumberComplete ?? string.Empty;

        /// <summary>
        /// Get the store-specific fields that are used to uniquely identify an online customer record.  Such
        /// as the eBay User ID or the osCommerce CustomerID.  If a particular store does not have any concept
        /// of a unique online customer, than this can return null.  If multiple fields are returned, they
        /// will be tested using OR.  If customer identifiers are unique per store instance, set instanceLookup to true.  If
        /// they are unique per store type, set instanceLookup to false;
        /// </summary>
        public virtual IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = false;

            return null;
        }


        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        public virtual List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return CreateAddStoreWizardPagesViaIoC(scope);
        }

        /// <summary>
        /// Creates the add store wizard pages via IoC.
        /// </summary>
        protected List<WizardPage> CreateAddStoreWizardPagesViaIoC(ILifetimeScope scope)
        {
            if (scope.IsRegisteredWithKey<WizardPage>(TypeCode))
            {
                return scope.ResolveKeyed<IEnumerable<WizardPage>>(TypeCode).SortByOrder().ToList();
            }

            throw new InvalidOperationException("Invalid store type. " + TypeCode);
        }

        /// <summary>
        /// Create the control that will be used to show the options for automatic creation of Online Update actions.  Return null
        /// if no such page is needed.
        /// </summary>
        public virtual OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            if (IoC.UnsafeGlobalLifetimeScope.IsRegisteredWithKey<OnlineUpdateActionControlBase>(TypeCode))
            {
                return IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<OnlineUpdateActionControlBase>(TypeCode);
            }

            return null;
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public virtual List<FilterEntity> CreateInitialFilters()
        {
            ICollection<string> onlineStatusChoices = IsShipEngine() ? GetShipEngineStatusChoices() : GetOnlineStatusChoices();
            List<FilterEntity> filters = new List<FilterEntity>();

            foreach (string onlineStatus in onlineStatusChoices)
            {
                FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
                definition.RootContainer.JoinType = ConditionGroupJoinType.And;

                StoreCondition storeCondition = new StoreCondition();
                storeCondition.Operator = EqualityOperator.Equals;
                storeCondition.Value = store.StoreID;
                definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

                OnlineStatusCondition onlineStatusCondition = new OnlineStatusCondition();
                onlineStatusCondition.Operator = StringOperator.Equals;
                onlineStatusCondition.TargetValue = onlineStatus;
                definition.RootContainer.FirstGroup.Conditions.Add(onlineStatusCondition);

                filters.Add(new FilterEntity()
                {
                    Name = onlineStatus,
                    Definition = definition.GetXml(),
                    IsFolder = false,
                    FilterTarget = (int) FilterTarget.Orders
                });
            }

            return filters;
        }

        protected List<FilterEntity> CreateInitialFilters<TStatus, TCondition>(List<TStatus> statusesForFilters)
            where TCondition : EnumCondition<TStatus>, new()
            where TStatus : struct
        {
            List<FilterEntity> filters = new List<FilterEntity>();

            foreach (TStatus shippingStatus in statusesForFilters)
            {
                FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
                definition.RootContainer.JoinType = ConditionGroupJoinType.And;

                TCondition shippingStatusCondition = new TCondition();
                shippingStatusCondition.Operator = EnumEqualityOperator.Equals;
                shippingStatusCondition.Value = shippingStatus;
                definition.RootContainer.FirstGroup.Conditions.Add(shippingStatusCondition);

                StoreCondition storeCondition = new StoreCondition();
                storeCondition.Operator = EqualityOperator.Equals;
                storeCondition.Value = Store.StoreID;
                definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

                filters.Add(new FilterEntity
                {
                    Name = EnumHelper.GetDescription((Enum) (object) shippingStatus),
                    Definition = definition.GetXml(),
                    IsFolder = false,
                    FilterTarget = (int) FilterTarget.Orders
                });
            }

            return filters;
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public virtual AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return CreateAccountSettingsControlViaIoC();
        }

        /// <summary>
        /// Create the control that is used for editing the account settings via IoC.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid store type.  + TypeCode</exception>
        protected AccountSettingsControlBase CreateAccountSettingsControlViaIoC()
        {
            if (!IoC.UnsafeGlobalLifetimeScope.IsRegisteredWithKey<Func<StoreEntity, AccountSettingsControlBase>>(TypeCode))
            {
                throw new InvalidOperationException("Invalid store type. " + TypeCode);
            }

            Func<StoreEntity, AccountSettingsControlBase> controlFactory = IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<Func<StoreEntity, AccountSettingsControlBase>>(TypeCode);

            return controlFactory(Store);
        }

        /// <summary>
        /// Create the control used to edit the manual order number settings for the store
        /// </summary>
        public virtual ManualOrderSettingsControl CreateManualOrderSettingsControl()
        {
            return new ManualOrderSettingsControl();
        }

        /// <summary>
        /// Create the control used to edit the manual order number settings for the store
        /// </summary>
        public virtual IDownloadSettingsControl CreateDownloadSettingsControl()
        {
            return new DownloadSettingsControl();
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public virtual StoreSettingsControlBase CreateStoreSettingsControl()
        {
            if (IoC.UnsafeGlobalLifetimeScope.IsRegisteredWithKey<StoreSettingsControlBase>(TypeCode))
            {
                return IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<StoreSettingsControlBase>(TypeCode);
            }

            return null;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public virtual IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public virtual IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public virtual ICollection<string> GetOnlineStatusChoices()
        {
            return new string[0];
        }

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public virtual void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {

        }

        /// <summary>
        /// Generate the template XML output for the given order item
        /// </summary>
        public virtual void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {

        }

        /// <summary>
        /// Generate the template XML output for the given order item attribute
        /// </summary>
        public virtual void GenerateTemplateOrderItemAttributeElements(ElementOutline container, Func<OrderItemAttributeEntity> optionSource)
        {

        }

        /// <summary>
        /// Generate a new manual order number and apply it to the given order.  Derived classes should throw
        /// NotSupportedException if an error occurs during order number generation.
        /// </summary>
        public virtual void GenerateManualOrderNumber(OrderEntity order)
        {
            long orderNumber = 1;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(OrderFields.OrderNumber, null, AggregateFunction.Max, OrderFields.StoreID == Store.StoreID & OrderFields.OrderNumber >= 0);
                if (!(result is DBNull))
                {
                    orderNumber = Convert.ToInt64(result) + 1;
                }
            }

            order.OrderNumber = orderNumber;
            order.ApplyOrderNumberPrefix(Store.ManualOrderPrefix);
            order.ApplyOrderNumberPostfix(Store.ManualOrderPostfix);
        }

        /// <summary>
        /// Indicates if the StoreType supports hyperlinking the grid for the given field
        /// </summary>
        public virtual bool GridHyperlinkSupported(IStoreEntity store, EntityBase2 entity, EntityField2 field)
        {
            return false;
        }

        /// <summary>
        /// Handle a hyperlink click for the given field and entity from the grid
        /// </summary>
        public virtual void GridHyperlinkClick(IStoreEntity store, EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {

        }

        /// <summary>
        /// Indicates of the StoreType supports the display of the given "Online" column.  By default it always returns false.
        /// </summary>
        public virtual bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            return false;
        }

        /// <summary>
        /// Create any dashboard messages that should be displayed as issues or messages based on the store of the store type
        /// </summary>
        public virtual IEnumerable<DashboardStoreItem> CreateDashboardMessages()
        {
            return null;
        }

        /// <summary>
        /// Called during store deletion to give derived store types the chance to cleanup any additional data rows\tables.
        /// </summary>
        public virtual void DeleteStoreAdditionalData(SqlAdapter adapter)
        {

        }

        /// <summary>
        /// This is used as a safe and friendly internal code for storing information.  For instance,
        /// registry key values and template exclusion names.
        /// </summary>
        virtual public string StoreSafeName
        {
            get { return StoreTypeName; }
        }

        /// <summary>
        /// The code of the store type that is used to uniquely identify the type with tango.
        /// </summary>
        public string TangoCode
        {
            get { return StoreTypeIdentity.FromCode(TypeCode).TangoCode; }
        }

        /// <summary>
        /// Used for decrypting license keys.
        /// </summary>
        public string LicenseSalt
        {
            get { return StoreTypeIdentity.FromCode(TypeCode).LicenseSalt; }
        }

        /// <summary>
        /// This calls the abstract function to get the identifier, and then normalizes it.
        /// </summary>
        public string LicenseIdentifier
        {
            get
            {
                return NormalizeIdentifier(InternalLicenseIdentifier);
            }
        }

        /// <summary>
        /// The minimum minutes required to wait between automatic downloads.
        /// </summary>
        public virtual int AutoDownloadMinimumMinutes
        {
            get { return 2; }
        }

        /// <summary>
        /// Controls the store's policy for restricting the range of an initial download
        /// </summary>
        public virtual InitialDownloadPolicy InitialDownloadPolicy
        {
            get { return new InitialDownloadPolicy(InitialDownloadRestrictionType.None); }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.  Like the eBay user ID for ebay,  or store URL for Yahoo!
        /// </summary>
        protected abstract string InternalLicenseIdentifier { get; }

        /// <summary>
        /// Normalize the identifier to try to eliminate cases where the same url \ id could
        /// be represented in multiple ways.
        /// </summary>
        public static string NormalizeIdentifier(string identifier)
        {
            MethodConditions.EnsureArgumentIsNotNull(identifier, nameof(identifier));

            // All lower, no extra spaces
            identifier = identifier.ToLowerInvariant().Trim();

            if (identifier.Length == 0)
            {
                throw new ArgumentException("Identifier cannot be empty.");
            }

            // Any slashes would be from URL's... make sure they all face the same way
            identifier = identifier.Replace(@"\", "/");

            // Get rid of any prefix
            identifier = identifier.Replace("https://", "");
            identifier = identifier.Replace("http://", "");

            // Don't allow quotes
            identifier = identifier.Replace("'", "");
            identifier = identifier.Replace("\"", "");

            return identifier;
        }

        /// <summary>
        /// Intended to be called during the processing of a shipment to allow the store to the
        /// chance to override the shipment details for any functionality that may be specific
        /// to a store's shipment processing scenario(s).
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of ShipmentFieldIndex objects indicating to the caller which fields
        /// of the shipment were changed.</returns>
        public virtual List<ShipmentFieldIndex> OverrideShipmentDetails(ShipmentEntity shipment)
        {
            // Do nothing here
            return new List<ShipmentFieldIndex>();
        }

        /// <summary>
        /// Added ShippingAddressEditStateType enum to replace the IsShipmentAddressEditable code.  We needed to display a message in the shipping panel as to why an address is not editable.
        /// Added EnumDescriptionConverter to display the description of an enum in XAML.
        /// </summary>
        public virtual ShippingAddressEditStateType ShippingAddressEditableState(OrderEntity order, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (shipment.Processed)
            {
                return ShippingAddressEditStateType.Processed;
            }

            if (!UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID))
            {
                return ShippingAddressEditStateType.PermissionDenied;
            }

            return ShippingAddressEditStateType.Editable;
        }

        /// <summary>
        /// Determines whether customs is required for the specified shipment. A pre-determined recommendation
        /// whether to require customs is provided to provide the store context for any precursory checks that
        /// have already occurred.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsRequiredRecommendation">The recommendation to the store as to whether customs is required.</param>
        /// <returns>
        ///   <c>true</c> if [is customs required] [the specified shipment]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsCustomsRequired(IShipmentEntity shipment, bool customsRequiredRecommendation)
        {
            // Just accept whatever the recommendation is by default.
            return customsRequiredRecommendation;
        }

        /// <summary>
        /// Will calling OverrideShipmentDetails change the specified shipment
        /// </summary>
        public virtual bool WillOverrideShipmentDetailsChangeShipment(IShipmentEntity shipment) => false;

        /// <summary>
        /// Determines whether or not to show the wizard page that configures the upload/download settings for the store
        /// </summary>
        /// <remarks>For example Generic File has no upload or download settings so we skip showing the page.</remarks>
        public virtual bool ShowTaskWizardPage()
        {
            return true;
        }

        /// <summary>
        /// Should the Hub be used for this store?
        /// </summary>
        public virtual bool ShouldUseHub(IStoreEntity store) => false;

        /// <summary>
        /// Raise the StoreAdded event
        /// </summary>
        public void RaiseStoreAdded(StoreEntity store, ILifetimeScope lifetimeScope) =>
            StoreAdded?.Invoke(store, lifetimeScope);

        /// <summary>
        /// Does this store type use a ShipEngine integration?
        /// </summary>
        public virtual bool IsShipEngine() => false;

        /// <summary>
        /// Returns the possible ShipEngine Online Status values
        /// </summary>
        private ICollection<string> GetShipEngineStatusChoices()
        {
            return new List<string>
            {
                "Paid",
                "Unpaid",
                "Partially Paid",
                "Shipped",
                "Unshipped",
                "Partially Shipped",
                "Cancelled",
                "Unknown"
            };
        }
    }
}
