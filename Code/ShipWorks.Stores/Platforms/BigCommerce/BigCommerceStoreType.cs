using System;
using System.Collections.Generic;
using System.Globalization;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce Store Type implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.BigCommerce)]
    public class BigCommerceStoreType : StoreType
    {
        readonly Func<BigCommerceStoreEntity, IBigCommerceStatusCodeProvider> createStatusCodeProvider;
        readonly IIndex<StoreTypeCode, StoreSettingsControlBase> storeSettingsControlIndex;
        readonly IIndex<StoreTypeCode, OnlineUpdateActionControlBase> updateActionIndex;
        readonly IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex;
        readonly IBigCommerceIdentifier identifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// We're ignoring the parameter count because these dependencies were not added,
        /// they were just made explicit in the constructor.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        public BigCommerceStoreType(StoreEntity store,
            Func<BigCommerceStoreEntity, IBigCommerceStatusCodeProvider> createStatusCodeProvider,
            IIndex<StoreTypeCode, StoreSettingsControlBase> storeSettingsControlIndex,
            IIndex<StoreTypeCode, OnlineUpdateActionControlBase> updateActionIndex,
            IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex,
            IBigCommerceIdentifier identifier)
            : base(store)
        {
            this.identifier = identifier;
            this.accountSettingsControlIndex = accountSettingsControlIndex;
            this.updateActionIndex = updateActionIndex;
            this.storeSettingsControlIndex = storeSettingsControlIndex;
            this.createStatusCodeProvider = createStatusCodeProvider;
        }

        /// <summary>
        /// StoreTypeCode enum value for BigCommerce Store Types
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.BigCommerce;

        /// <summary>
        /// Gets a typed version of the store
        /// </summary>
        public BigCommerceStoreEntity TypedStore => Store as BigCommerceStoreEntity;

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// Since current customers can have the legacy implementation of BigCommerce, we need to support
        /// the old identifier as well, so use the same algorithm as before.
        /// </summary>
        protected override string InternalLicenseIdentifier => identifier.Get(TypedStore);

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Creates a new instance of an BigCommerce store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            BigCommerceStoreEntity bigCommerceStore = new BigCommerceStoreEntity();

            // Set the base store defaults
            InitializeStoreDefaults(bigCommerceStore);

            // Set the BigCommerce store specific defaults
            bigCommerceStore.ApiUrl = string.Empty;
            bigCommerceStore.ApiUserName = string.Empty;
            bigCommerceStore.ApiToken = string.Empty;
            bigCommerceStore.StoreName = "BigCommerce Store";
            bigCommerceStore.WeightUnitOfMeasure = (int) WeightUnitOfMeasure.Pounds;
            bigCommerceStore.DownloadModifiedNumberOfDaysBack = 7;
            bigCommerceStore.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;
            bigCommerceStore.OauthClientId = string.Empty;
            bigCommerceStore.OauthToken = string.Empty;

            return bigCommerceStore;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            string orderNumberComplete = order.OrderNumberComplete;
            string orderNumber = order.OrderNumber.ToString(CultureInfo.InvariantCulture);

            string invoicePostfix = string.Empty;
            if (orderNumberComplete.Length > (orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1))
            {
                invoicePostfix = orderNumberComplete.Substring(orderNumberComplete.IndexOf(orderNumber, StringComparison.OrdinalIgnoreCase) + orderNumber.Length + 1);
            }

            return new BigCommerceOrderIdentifier(order.OrderNumber, invoicePostfix);
        }

        /// <summary>
        /// Creates a BigCommerce store-specific instance of an BigCommerceOrderItemEntity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance() => new BigCommerceOrderItemEntity();

        /// <summary>
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<BigCommerceOrderItemEntity>(() => (BigCommerceOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("BigCommerce");
            outline.AddElement("DigitalItem", () => item.Value.IsDigitalItem);
            outline.AddElement("EventDate", () => item.Value.EventDate);
            outline.AddElement("EventName", () => item.Value.EventName);
        }

        /// <summary>
        /// Get a list of supported online BigCommerce statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            IBigCommerceStatusCodeProvider statusCodeProvider = createStatusCodeProvider(TypedStore);

            return statusCodeProvider.CodeNames;
        }

        /// <summary>
        /// Get the store-specific fields that are used to uniquely identify an online customer record.  Such
        /// as the eBay User ID or the osCommerce CustomerID.  If a particular store does not have any concept
        /// of a unique online customer, than this can return null.  If multiple fields are returned, they
        /// will be tested using OR.  If customer identifiers are unique per store instance,
        /// set instanceLookup to true.  If they are unique per store type, set instanceLookup to false;
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the control used to configured the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            updateActionIndex[StoreTypeCode.BigCommerce];

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() =>
            accountSettingsControlIndex[StoreTypeCode.BigCommerce];

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl() =>
            storeSettingsControlIndex[StoreTypeCode.BigCommerce];

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
    }
}
