using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Shopify)]
    [Component(RegistrationType.Self)]
    public class ShopifyStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is ShopifyStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not an instance of ShopifyStoreEntity.");
            }
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Shopify;

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                string identifier = ((ShopifyStoreEntity) Store).ShopifyShopUrlName.ToLowerInvariant();

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Creates a new instance of an Shopify store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ShopifyStoreEntity shopifyStore = new ShopifyStoreEntity();

            InitializeStoreDefaults(shopifyStore);

            shopifyStore.ShopifyAccessToken = string.Empty;
            shopifyStore.ShopifyShopUrlName = string.Empty;
            shopifyStore.ShopifyShopDisplayName = string.Empty;
            shopifyStore.ShopifyRequestedShippingOption = (int) ShopifyRequestedShippingField.Title;
            shopifyStore.ApiKey = string.Empty;
            shopifyStore.Password = string.Empty;

            return shopifyStore;
        }

        /// <summary>
        /// Get the identifier object that is used to uniquely identify the specified order for the store.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            return new ShopifyOrderIdentifier(((ShopifyOrderEntity) order).ShopifyOrderID);
        }

        /// <summary>
        /// Create the custom Shopify entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance() => new ShopifyOrderEntity();

        /// <summary>
        /// Creates the custom OrderItem entity.
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new ShopifyOrderItemEntity();
        }

        /// <summary>
        /// Get a list of supported online Shopify statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return EnumHelper.GetEnumList<ShopifyStatus>().Select(status => status.Description).ToList();
        }

        /// <summary>
        /// Create the fields required to uniquely identify the online customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the pages to be displayed in the Add Store Wizard
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> pages = new List<WizardPage>();

            pages.Add(new WizardPages.ShopifyAssociateAccountPage());

            return pages;
        }

        /// <summary>
        /// Create the control used to configured the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(Shopify.CoreExtensions.Actions.ShopifyShipmentUploadTask));
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ShopifyAccountSettingsControl();
        }

        /// <summary>
        /// Indicates what columns are supported for this store type
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
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            MethodConditions.EnsureArgumentIsNotNull(container, nameof(container));

            var order = new Lazy<ShopifyOrderEntity>(() => (ShopifyOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Shopify");
            outline.AddElement("OrderID", () => order.Value.ShopifyOrderID);
            outline.AddElement("FulfillmentStatus", () => EnumHelper.GetDescription((ShopifyFulfillmentStatus) order.Value.FulfillmentStatusCode));
            outline.AddElement("PaymentStatus", () => EnumHelper.GetDescription((ShopifyPaymentStatus) order.Value.PaymentStatusCode));
        }

        /// <summary>
        /// Generate the template XML output for the given order item
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            MethodConditions.EnsureArgumentIsNotNull(container, nameof(container));

            var item = new Lazy<ShopifyOrderItemEntity>(() => (ShopifyOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Shopify");
            outline.AddElement("ItemID", () => item.Value.ShopifyOrderItemID);
            outline.AddElement("ProductID", () => item.Value.ShopifyProductID);
        }

        /// <summary>
        /// Create and return the Shopify store settings control.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new ShopifyStoreSettingsControl();
        }
    }
}
