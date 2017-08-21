using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Newegg store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.NeweggMarketplace)]
    [Component(RegistrationType.Self)]
    public class NeweggStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NeweggStoreType));

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public NeweggStoreType(StoreEntity store)
            : base(store)
        { }

        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.NeweggMarketplace;

        /// <summary>
        /// This is a string that uniquely identifies the store.  Like the eBay user ID for ebay,  or store URL for Yahoo!
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((NeweggStoreEntity) Store).SellerID; }
        }


        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            NeweggStoreEntity store = new NeweggStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreName = "My Newegg Store";
            store.SellerID = string.Empty;
            store.SecretKey = string.Empty;
            store.ExcludeFulfilledByNewegg = false;
            store.Channel = (int) NeweggChannelType.US;

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override Content.OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            if (order == null)
            {
                string message = "A null order value was provided to CreateOrderIdentifier.";

                log.ErrorFormat(message);
                throw new InvalidOperationException(message);
            }

            return new Content.OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderEntity
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity CreateOrderInstance()
        {
            NeweggOrderEntity neweggOrderEntity = new NeweggOrderEntity();
            return neweggOrderEntity;
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderItemEntity
        /// </summary>
        /// <returns></returns>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new NeweggOrderItemEntity();
        }

        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            List<WizardPage> wizardPages = new List<WizardPage>()
            {
                new NeweggAccountPage(),
                new NeweggDownloadCriteriaPage()
            };

            return wizardPages;
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        /// <returns></returns>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new NeweggDownloadStoreSettingsControl();
        }

        /// <summary>
        /// Controls the store's policy for restricting the range of an initial download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get { return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack); }
        }

        /// <summary>
        /// Indicates of the StoreType supports the display of the given "Online" column.  By default it always returns false.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        /// <returns></returns>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new NeweggAccountSettingsControl();
        }



        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        /// <param name="container"></param>
        /// <param name="orderSource"></param>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<NeweggOrderEntity>(() => (NeweggOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Newegg");
            outline.AddElement("InvoiceNumber", () => order.Value.InvoiceNumber);
        }




        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            IEnumerable<NeweggOrderStatus> enums = Enum.GetValues(typeof(NeweggOrderStatus)).Cast<NeweggOrderStatus>();

            return enums.Select(s => Enum.GetName(typeof(NeweggOrderStatus), s)).ToList();
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(NeweggShipmentUploadTask));
        }
    }
}
