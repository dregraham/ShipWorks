﻿using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Rakuten.CoreExtensions.Actions;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Store type for Rakuten
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Rakuten, ExternallyOwned = true)]
    public class RakutenStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;

        /// <summary>
        /// The walmart store
        /// </summary>
        private readonly RakutenStoreEntity RakutenStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="RakutenStoreType"/> class.
        /// </summary>
        public RakutenStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory)
            : base(store)
        {
            this.downloaderFactory = downloaderFactory;
            RakutenStore = (RakutenStoreEntity) store;
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Rakuten;

        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            RakutenStoreEntity store = new RakutenStoreEntity();

            InitializeStoreDefaults(store);

            store.AuthKey = string.Empty;
            store.ShopURL = string.Empty;
            store.MarketplaceID = string.Empty;
            store.StoreName = "My Rakuten Store";

            return store;
        }

        /// <summary>
        /// Get the store-specific OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new AlphaNumericOrderIdentifier(order.OrderNumberComplete);
        }

        /// <summary>
        /// Create the CA order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance() =>
            new RakutenOrderEntity();

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            RakutenOrderItemEntity entity = new RakutenOrderItemEntity
            {
                SalesChannelLineNumber = -1
            };

            return entity;
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => RakutenStore.MarketplaceID;

        /// <summary>
        /// Do we support online columns
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column) =>
            column == OnlineGridColumnSupport.OnlineStatus;

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => new[]
        {
            "Acknowledged",
            "Accepted",
            "Canceled",
            "Cancelled",
            "Complete",
            "Entered",
            "Processing",
        };

        /// <summary>
        /// Specifies the download policy for the online store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy =>
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 30, MaxDaysBack = 30 };

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<RakutenOrderEntity>(() => (RakutenOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Rakuten");
            outline.AddElement("Warehouse", () => order.Value.WarehouseCode);
            outline.AddElement("Channel", () => order.Value.SalesChannelName);
            outline.AddElement("SOFSCreatedDate", () => order.Value.SofsCreatedDate);
        }

        /// <summary>
        /// Creates the add store wizard online update action control for Rakuten
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new OnlineUpdateShipmentUpdateActionControl(typeof(RakutenShipmentUploadTask));
    }
}
