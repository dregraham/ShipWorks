using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BuyDotCom.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.BuyDotCom.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// StoreType instance for Buy.com
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.BuyDotCom)]
    [Component(RegistrationType.Self)]
    public class BuyDotComStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComStoreType));

        BuyDotComStoreEntity buyDotComStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComStoreType(StoreEntity store)
            : base(store)
        {
            buyDotComStore = store as BuyDotComStoreEntity;
            if (store != null && buyDotComStore == null)
            {
                throw new ArgumentException("StoreEntity is not instance of BuyDotComStoreEntity.");
            }
        }

        /// <summary>
        /// Returns Buy.com TypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.BuyDotCom;

        /// <summary>
        /// Creates a new buy.com store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            BuyDotComStoreEntity newStore = new BuyDotComStoreEntity();

            InitializeStoreDefaults(newStore);

            newStore.StoreName = "Buy.com Store";

            return newStore;
        }

        /// <summary>
        /// Create a Buy.com specific orderitem entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new BuyDotComOrderItemEntity();
        }

        /// <summary>
        /// Create an identifier that uniquely identifies the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new AlphaNumericOrderIdentifier(order.OrderNumberComplete);
        }

        /// <summary>
        /// Returns setup wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new BuyDotComCredentialsPage()
            };
        }

        /// <summary>
        /// Returns the CreateAccountSettings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new BuyDotComAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(BuyDotComShipmentUploadTask));
        }

        /// <summary>
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<BuyDotComOrderItemEntity>(() => (BuyDotComOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("BuyDotCom");
            outline.AddElement("ReceiptItemID", () => item.Value.ReceiptItemID);
            outline.AddElement("ListingID", () => item.Value.ListingID);
            outline.AddElement("Shipping", () => item.Value.Shipping);
            outline.AddElement("Tax", () => item.Value.Tax);
            outline.AddElement("Commission", () => item.Value.Commission);
            outline.AddElement("ItemFee", () => item.Value.ItemFee);
        }

        /// <summary>
        /// LicenseIdentifier
        /// </summary>
        protected override string InternalLicenseIdentifier => buyDotComStore.FtpUsername;
    }
}
