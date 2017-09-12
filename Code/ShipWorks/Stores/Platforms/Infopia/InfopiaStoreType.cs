using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Infopia.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Integration with the Infopia platform
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what Infopia currently uses")]
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Infopia)]
    [Component(RegistrationType.Self)]
    public class InfopiaStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(InfopiaStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates the entity for the store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            InfopiaStoreEntity storeEntity = new InfopiaStoreEntity();
            InitializeStoreDefaults(storeEntity);

            // infopia initialization
            storeEntity.ApiToken = "";

            return storeEntity;
        }

        /// <summary>
        /// Returns an identifier for finding infopia orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order) =>
            new OrderNumberIdentifier(order.OrderNumber);

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance() =>
            new InfopiaOrderItemEntity();

        /// <summary>
        /// Instantiate and return the setup wizard pages for Infopia
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new InfopiaTokenWizardPage()
            };
        }

        /// <summary>
        /// Create the control for creating online update actions in the wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new InfopiaOnlineUpdateActionControl();

        /// <summary>
        /// Instantiate and return the control for editing the Infopia account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() =>
            new InfopiaAccountSettingsControl();

        /// <summary>
        /// Returns the identifying code for Infopia stores
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Infopia;

        /// <summary>
        /// Return value that uniquely identifies this store instance
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                InfopiaStoreEntity store = (InfopiaStoreEntity) Store;

                // We can't use the actual token, because its secure.
                byte[] bytes = Encoding.UTF8.GetBytes(store.ApiToken);

                MD5 md5 = new MD5CryptoServiceProvider();

                // Generate the hash
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }

        /// <summary>
        /// Get the online status choices for the store. This is used to populate the Online Status filter dropdown.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => InfopiaUtility.StatusValues;

        /// <summary>
        /// Infopia supports both "Online" columns
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus ||
                column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Get the policy for the initial order download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy =>
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Generate Infopia specific template item elements
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<InfopiaOrderItemEntity>(() => (InfopiaOrderItemEntity) itemSource());

            // This is legacy format.  If we want to add it for real for v3, I think maybe we should create a new element "Infopia"
            ElementOutline outline = container.AddElement("Marketplace");
            outline.AddAttributeLegacy2x();
            outline.AddElement("Name", () => item.Value.Marketplace);
            outline.AddElement("BuyerID", () => item.Value.BuyerID);
            outline.AddElement("ItemID", () => item.Value.MarketplaceItemID);
        }
    }
}
