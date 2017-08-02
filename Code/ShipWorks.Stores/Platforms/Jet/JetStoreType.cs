using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Store type for Jet
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Jet, ExternallyOwned = true)]
    public class JetStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;

        /// <summary>
        /// The walmart store
        /// </summary>
        private readonly JetStoreEntity jetStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetStoreType"/> class.
        /// </summary>
        public JetStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory)
            : base(store)
        {
            this.downloaderFactory = downloaderFactory;
            jetStore = (JetStoreEntity) store;
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Jet;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            JetStoreEntity store = new JetStoreEntity();

            InitializeStoreDefaults(store);

            store.ApiUser = string.Empty;
            store.Secret = string.Empty;
            store.StoreName = "My Jet Store";

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the CA order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            JetOrderEntity entity = new JetOrderEntity {MerchantOrderId = ""};

            return entity;
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            JetOrderItemEntity entity = new JetOrderItemEntity();
            entity.MerchantSku = "";
            entity.JetOrderItemID = "";

            return entity;
        }

        /// <summary>
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        public override StoreDownloader CreateDownloader() => downloaderFactory[TypeCode](Store);

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => jetStore.ApiUser;
    }
}
