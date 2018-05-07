using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Store type for Overstock
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Overstock, ExternallyOwned = true)]
    public class OverstockStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;

        /// <summary>
        /// The walmart store
        /// </summary>
        private readonly OverstockStoreEntity overstockStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverstockStoreType"/> class.
        /// </summary>
        public OverstockStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory)
            : base(store)
        {
            this.downloaderFactory = downloaderFactory;
            overstockStore = (OverstockStoreEntity) store;
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Overstock;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            OverstockStoreEntity store = new OverstockStoreEntity();

            InitializeStoreDefaults(store);

            store.Username = string.Empty;
            store.Password = string.Empty;
            store.StoreName = "My Overstock Store";

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new OverstockOrderIdentifier(((OverstockOrderEntity) order).OverstockOrderID);
        }

        /// <summary>
        /// Create the CA order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            OverstockOrderEntity entity = new OverstockOrderEntity { OverstockOrderID = -1 };

            return entity;
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            OverstockOrderItemEntity entity = new OverstockOrderItemEntity();
            entity.ChannelLineId = -1;
            entity.LineId = -1;
            entity.ItemID = -1;

            return entity;
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => overstockStore.Username;

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => new[] { "Acknowledged", "Processing", "Complete" };
    }
}
