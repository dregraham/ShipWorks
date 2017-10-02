using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// 3dcart Store Type implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ThreeDCart)]
    [Component(RegistrationType.Self)]
    public class ThreeDCartStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value for ThreeD Cart Store Types
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ThreeDCart;

        /// <summary>
        /// Whether or not the user is using the REST API
        /// </summary>
        private bool RestUser => ((ThreeDCartStoreEntity) Store).RestUser;

        /// <summary>
        /// Link to article on adding a 3dcart store
        /// </summary>
        public string AccountSettingsHelpUrl
        {
            get
            {
                return RestUser ?
                    "http://support.shipworks.com/solution/articles/4000076906-adding-3dcart-using-rest-api" :
                    "http://support.shipworks.com/support/solutions/articles/167787-adding-a-3dcart-store-";
            }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// Since current customers can have the legacy implementation of 3dcart, we need to support
        /// the old identifier as well, so use the same algorithm as before.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                ThreeDCartStoreEntity store = (ThreeDCartStoreEntity) Store;

                string identifier = store.StoreUrl.ToLowerInvariant();

                identifier = Regex.Replace(identifier, "(admin/)?[^/]*(\\.)?[^/]+$", "", RegexOptions.IgnoreCase);

                // The regex above will return just the scheme if there's no ending /, so check for that and
                // reset to the StoreUrl
                if (identifier.EndsWith(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
                {
                    identifier = store.StoreUrl.ToLowerInvariant();
                }

                if (string.IsNullOrWhiteSpace(identifier))
                {
                    throw new ThreeDCartException("ShipWorks was unable to validate your unique license identifier for your store.");
                }

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Creates a new instance of an ThreeDCart store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ThreeDCartStoreEntity threeDCartStore = new ThreeDCartStoreEntity();

            // Set the base store defaults
            InitializeStoreDefaults(threeDCartStore);

            // Set the threeD cart store specific defaults
            threeDCartStore.StoreUrl = string.Empty;
            threeDCartStore.ApiUserKey = string.Empty;
            threeDCartStore.StoreName = "3dcart Store";
            threeDCartStore.DownloadModifiedNumberOfDaysBack = 7;

            // Default to the Eastern time zone, as that is the default when creating a new store on 3dcart
            threeDCartStore.TimeZoneID = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").Id;

            // For any newly created stores, we want to use the REST API, so this is defaulted to true
            threeDCartStore.RestUser = true;

            return threeDCartStore;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            string orderNumberComplete = order.OrderNumberComplete;
            string orderNumber = order.OrderNumber.ToString(CultureInfo.InvariantCulture);

            string invoicePrefix = orderNumberComplete.Substring(0, orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase));

            string invoicePostfix = string.Empty;
            if (orderNumberComplete.Length > (orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1))
            {
                invoicePostfix = orderNumberComplete.Substring(orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1);
            }

            return new ThreeDCartOrderIdentifier(order.OrderNumber, invoicePrefix, invoicePostfix);
        }

        /// <summary>
        /// Creates a 3dcart store-specific instance of an ThreeDCartOrderItemEntity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance() => new ThreeDCartOrderItemEntity();

        /// <summary>
        /// Creates the order instance.
        /// </summary>
        protected override OrderEntity CreateOrderInstance() => new ThreeDCartOrderEntity();

        /// <summary>
        /// Get a list of supported online ThreeDCart statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            if (RestUser)
            {
                IEnumerable<EnumEntry<Enums.ThreeDCartOrderStatus>> statuses = EnumHelper.GetEnumList<Enums.ThreeDCartOrderStatus>();
                return statuses.Select(s => s.Description).ToList();
            }

            ThreeDCartStatusCodeProvider statusCodeProvider = new ThreeDCartStatusCodeProvider((ThreeDCartStoreEntity) Store);
            return statusCodeProvider.CodeNames;
        }

        /// <summary>
        /// Get the store-specific fields that are used to uniquely identify an online customer record.  Such
        /// as the eBay User ID or the osCommerce CustomerID.  If a particular store does not have any concept
        /// of a unique online customer, than this can return null.  If multiple fields are returned, they
        /// will be tested using OR.  If customer identifiers are unique per store instance,
        /// set instanceLookup to true.  If they are unique per store type, set instanceLookup to false;
        ///
        /// As per 3dcart tech support chat, each store has a unique set of IDs, all starting at 1
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> pages = new List<WizardPage>
                {
                    new WizardPages.ThreeDCartAccountPage(),
                    new WizardPages.ThreeDCartDownloadCriteriaPage(),
                    new WizardPages.ThreeDCartTimeZonePage()
                };

            return pages;
        }

        /// <summary>
        /// Create the control used to configured the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.ThreeDCartOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ThreeDCartAccountSettingsControl();
        }

        /// <summary>
        /// Indicates if the StoreType supports the display of the given "Online" column.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Generate the template XML output for the given order item
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<ThreeDCartOrderItemEntity>(() => (ThreeDCartOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("ThreeDCart");
            outline.AddElement("ShipmentID", () => item.Value.ThreeDCartShipmentID);
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new ThreeDCartStoreSettingsControl();
        }
    }
}
