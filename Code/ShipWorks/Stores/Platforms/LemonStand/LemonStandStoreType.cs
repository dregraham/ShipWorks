using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.LemonStand.WizardPages;
//using ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Actions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Net;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    /// LemonStand integration
    /// </summary>
    class LemonStandStoreType : StoreType
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Indentifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.LemonStand; }
        }

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                LemonStandStoreEntity store = (LemonStandStoreEntity)Store;

                string identifier = store.StoreURL;

                return identifier;
            }
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            LemonStandAccountSettingsControl settingsControl = new LemonStandAccountSettingsControl();

            return settingsControl;
        }


        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new LemonStandAccountPage()
            };
        }

        /// <summary>
        /// Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return null;
            //return new GrouponDownloader(Store);
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            LemonStandStoreEntity store = new LemonStandStoreEntity();

            InitializeStoreDefaults(store);
            store.StoreURL = "";
            store.APIKey = "";
            store.Token = "";
            store.StoreName = "LemonStand";

            return store;
        }

        /// <summary>
        /// Creates the OrderIdentifier for locating Volusion orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return null;
            //return new GrouponOrderIdentifier(((GrouponOrderEntity)order).GrouponOrderID);
        }


    }
}
