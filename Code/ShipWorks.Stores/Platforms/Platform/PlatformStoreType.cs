using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Base class for PlatformStoreType
    /// </summary>
    public abstract class PlatformStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is PlatformStoreEntity))
            {
                throw new ArgumentException("PlatformStoretype - StoreEntity is not instance of PlatformStoreEntity.");
            }
        }

        /// <summary>
        /// Platform store type must be added via the HUB
        /// </summary>
        public override bool CanAddStoreType => false;

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                PlatformStoreEntity store = (PlatformStoreEntity) Store;
                return store.OrderSourceID;
            }
        }

        /// <summary>
        /// Creates the OrderIdentifier for locating Platform orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new AlphaNumericOrderIdentifier(order.OrderNumberComplete);
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            PlatformStoreEntity store = new PlatformStoreEntity();

            InitializeStoreDefaults(store);

            store.OrderSourceID = "";

            return store;
        }
    }
}