using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Manual
{
    /// <summary>
    /// Store type for Manual
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Manual, ExternallyOwned = false)]
    public class ManualStoreType : StoreType
    {
        private readonly StoreEntity manualStore;

        /// <summary>
        /// Initializes a new instance of the ManualStoreType class. 
        /// </summary>
        public ManualStoreType(StoreEntity store) : base(store)
        {
            manualStore = store;
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

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return null;
        }

        protected override string InternalLicenseIdentifier => manualStore.StoreName;

        
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>();
        }
    }
}
