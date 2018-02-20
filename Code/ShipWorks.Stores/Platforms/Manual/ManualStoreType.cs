using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Manual
{
    /// <summary>
    /// Store type for Manual
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Manual, ExternallyOwned = false)]
    public class ManualStoreType : StoreType
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ManualStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// The type code of the store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Manual;

        /// <summary>
        /// Creates an instance of StoreEntity 
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            StoreEntity store = new StoreEntity();

            InitializeStoreDefaults(store);

            store.StoreName = "My Manual Store";
            store.AutoDownload = false;

            return store;
        }

         /// <summary>
        /// Create an identifier that uniquely identifies the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new AlphaNumericOrderIdentifier(order.OrderNumberComplete);
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public OrderIdentifier CreateOrderIdentifier(string orderNumber, string prefix, string postfix)
        {
            return new AlphaNumericOrderIdentifier(orderNumber, prefix, postfix);
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => SystemData.Fetch().DatabaseID.ToString();

        /// <summary>
        /// Creates the add store wizard pages.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>();
        }

        /// <summary> 
        /// ManualStore does not support uploading or downloading so we return false to skip this page
        /// </summary> 
        public override bool ShowTaskWizardPage() => false;

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            ManualAccountSettingsControl control = new ManualAccountSettingsControl();

            return control;
        }
    }
}
