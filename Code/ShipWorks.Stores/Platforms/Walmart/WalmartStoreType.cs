using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Store type for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.StoreType" />
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Walmart, ExternallyOwned = true)]
    public class WalmartStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;
        private readonly Func<WalmartStoreEntity, WalmartOnlineUpdateInstanceCommandsFactory> onlineUpdateInstanceCommandsFactory;
        private readonly WalmartStoreEntity walmartStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="downloaderFactory"></param>
        /// <param name="onlineUpdateInstanceCommandsFactory"></param>
        public WalmartStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory,
            Func<WalmartStoreEntity, WalmartOnlineUpdateInstanceCommandsFactory> onlineUpdateInstanceCommandsFactory)
            : base(store)
        {
            this.downloaderFactory = downloaderFactory;
            this.onlineUpdateInstanceCommandsFactory = onlineUpdateInstanceCommandsFactory;

            walmartStore = store as WalmartStoreEntity;
        }

        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Walmart;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();

            InitializeStoreDefaults(store);
            store.ConsumerID = "";
            store.PrivateKey = "";
            store.ChannelType = "";
            store.StoreName = "My Walmart Store";
            store.DownloadModifiedNumberOfDaysBack = 7;

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
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        public override StoreDownloader CreateDownloader() => downloaderFactory[TypeCode](Store);

        /// <summary>
        /// Creates a Walmart Order Entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance() => new WalmartOrderEntity();

        /// <summary>
        /// Creates a Walmart Order Item Entity
        /// </summary>
        /// <returns></returns>
        public override OrderItemEntity CreateOrderItemInstance() => new WalmartOrderItemEntity();

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => walmartStore.ConsumerID;

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return new List<string>() {"Created", "Acknowledged", "Shipped", "Cancelled", "Refund"};
        }

        /// <summary>
        /// Creates the add store wizard online update action control for Walmart
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() => new OnlineUpdateShipmentUpdateActionControl(typeof(WalmartShipmentUploadTask));

        /// <summary>
        /// Create the online update instance commands for Walmart
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
            => onlineUpdateInstanceCommandsFactory(walmartStore).CreateCommands();
    }
}