using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Jet.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Store type for Jet
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Jet, ExternallyOwned = true)]
    public class JetStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;
        private readonly Func<JetStoreEntity, JetOnlineUpdateInstanceCommands> onlineUpdateInstanceCommandsFactory;

        /// <summary>
        /// The walmart store
        /// </summary>
        private readonly JetStoreEntity jetStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetStoreType"/> class.
        /// </summary>
        public JetStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory,
            Func<JetStoreEntity, JetOnlineUpdateInstanceCommands> onlineUpdateInstanceCommandsFactory)
            : base(store)
        {
            this.downloaderFactory = downloaderFactory;
            this.onlineUpdateInstanceCommandsFactory = onlineUpdateInstanceCommandsFactory;
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
            return new JetOrderIdentifier(((JetOrderEntity) order).MerchantOrderId);
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
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => jetStore.ApiUser;

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => new[] {"Acknowledged", "Complete" };

        /// <summary>
        /// Creates the upload menu commands for Jet
        /// </summary>
        /// <returns></returns>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands() => onlineUpdateInstanceCommandsFactory(jetStore).Create().ToList();
        
        /// <summary>
        /// Creates the add store wizard online update action control for Jet
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new OnlineUpdateShipmentUpdateActionControl(typeof(JetShipmentUploadTask));
    }
}
