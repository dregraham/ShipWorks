using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon.WizardPages;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using log4net;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponStoreType : StoreType
    {

        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Indentifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Groupon; }
        }

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GrouponStoreEntity store = (GrouponStoreEntity)Store;

                string identifier = store.StoreUrl;

                return identifier;
            }
        }


        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            GrouponStoreAccountSettingsControl settingsControl = new GrouponStoreAccountSettingsControl();
            //settingsControl.Initialize(this);

            return settingsControl;
        }


        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new GrouponStoreAccountPage()
            };
        }

        /// <summary>
        /// Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new GrouponStoreDownloader(Store);
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GrouponStoreEntity store = new GrouponStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreUrl = "";
            store.SupplierID = "";
            store.Token = "";

            return store;
        }

        /// <summary>
        /// Create the Groupon order entity
        /// </summary>
        public override OrderEntity CreateOrderInstance()
        {
            GrouponOrderEntity entity = new GrouponOrderEntity();

            entity.GrouponOrderID = "";
            
            return entity;
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            GrouponOrderItemEntity entity = new GrouponOrderItemEntity();

            entity.Permalink = "";
            entity.ChannelSKUProvided = "";
            entity.FulfillmentLineitemID = "";
            entity.BomSKU = "";
            entity.CILineItemID = "";

            return entity;
        }


        /// <summary>
        /// Creates the OrderIdentifier for locating Volusion orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new GrouponStoreOrderIdentifier(((GrouponOrderEntity)order).GrouponOrderID);
        }
    }
}
