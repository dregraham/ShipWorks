using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Store type for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.StoreType" />
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Walmart, ExternallyOwned = true)]
    public class WalmartStoreType : StoreType
    {
        public WalmartStoreType(StoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Walmart;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();

            InitializeStoreDefaults(store);
            store.ConsumerID = "";
            store.PrivateKey = "";
            store.ChannelType = "";
            store.StoreName = "My Walmart Store";

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => ((WalmartStoreEntity) Store).ConsumerID;

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return new List<string>() {"Created", "Acknowledged", "Shipped", "Cancelled"};
        }
    }
}