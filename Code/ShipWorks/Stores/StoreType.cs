using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.UI;
using ShipWorks.Stores.Content;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Amazon;
using log4net;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores
{
    /// <summary>
	/// Base class for all store types.  All points of extension to shipworks via store types are here.
	/// </summary>
    public abstract class StoreType
    {
        // Store this instance is wrapping
        private StoreEntity store;

        /// <summary>
        /// Construction
        /// </summary>
        protected StoreType(StoreEntity store)
        {
            if (store != null)
            {
                if ((StoreTypeCode) store.TypeCode != this.TypeCode)
                {
                    throw new InvalidOperationException("Store type mismatch.");
                }
            }
            
            this.store = store;
        }

        /// <summary>
        /// Initialize the default values for the given store
        /// </summary>
        protected virtual void InitializeStoreDefaults(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            store.Enabled = true;
            store.SetupComplete = false;
            store.Edition = "";

            store.TypeCode = (int) TypeCode;
            store.CountryCode = "US";

            store.AutoDownload = false;
            store.AutoDownloadMinutes = Math.Max(10, AutoDownloadMinimumMinutes);
            store.AutoDownloadOnlyAway = true;

            store.ComputerDownloadPolicy = "";

            store.ManualOrderPrefix = "";
            store.ManualOrderPostfix = "-M";

            store.DefaultEmailAccountID = -1;
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
            return this.StoreTypeName;
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
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public abstract StoreEntity CreateStoreInstance();

        /// <summary>
        /// Creates a store-specific instance of an OrderEntity
        /// </summary>
        public virtual OrderEntity CreateOrderInstance()
        {
            return new OrderEntity();
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderItemEntity
        /// </summary>
        public virtual OrderItemEntity CreateOrderItemInstance()
        {
            return new OrderItemEntity();
        }

        /// <summary>
        /// Creates a store-specific instance of OrderItemAttributeEntity
        /// </summary>
        public virtual OrderItemAttributeEntity CreateOrderItemAttributeInstance()
        {
            return new OrderItemAttributeEntity();
        }

        /// <summary>
        /// Get the store-specifc OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public abstract OrderIdentifier CreateOrderIdentifier(OrderEntity order);

        /// <summary>
        /// Get the store-specific fields that are used to unqiuely identifiy an online cusotmer record.  Such
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
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        public abstract StoreDownloader CreateDownloader();

        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        public abstract List<WizardPage> CreateAddStoreWizardPages();

        /// <summary>
        /// Create the control that will be used to show the options for automatic creation of Online Update actions.  Return null
        /// if no such page is needed.
        /// </summary>
        public virtual OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return null;
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public virtual List<FilterEntity> CreateInitialFilters()
        {
            return null;
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public abstract AccountSettingsControlBase CreateAccountSettingsControl();

        /// <summary>
        /// Create the control used to edit the manual order number settings for the store
        /// </summary>
        public virtual ManualOrderSettingsControl CreateManualOrderSettingsControl()
        {
            return new ManualOrderSettingsControl();
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public virtual StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return null;
        }

        /// <summary>
        /// Create the search conditions that are specific to this store type that should be applied when user is doing a Basic Search.
        /// </summary>
        public virtual ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            return null;
        }

        /// <summary>
        /// Create the search conditions that are specific to this store type that should be applied when user is doing a Basic Search.
        /// </summary>
        public virtual ConditionGroup CreateBasicSearchCustomerConditions(string search)
        {
            return null;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public virtual List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            return new List<MenuCommand>();
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public virtual List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            return new List<MenuCommand>();
        }

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
                object result = adapter.GetScalar(OrderFields.OrderNumber, null, AggregateFunction.Max, OrderFields.StoreID == Store.StoreID);
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
        public virtual bool GridHyperlinkSupported(EntityField2 field)
        {
            return false;
        }

        /// <summary>
        /// Handle a hyperlink click for the givnen field and entity from the grid
        /// </summary>
        public virtual void GridHyperlinkClick(EntityField2 field, EntityBase2 entity, IWin32Window owner)
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
        /// This calls the abstract function to get the identifider, and then normalizes it.
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
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }

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

            // Dont allow quotes
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

        public virtual bool IsShippingAddressEditable(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // Can't edit the address of a shipment that has been processed
            return !shipment.Processed;
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
        public virtual bool IsCustomsRequired(ShipmentEntity shipment, bool customsRequiredRecommendation)
        {
            // Just accept whatever the recommendation is by default.
            return customsRequiredRecommendation;
        }
    }
}
