﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Filters.Content;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Filters;

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
        private readonly Func<WalmartStoreEntity, IWalmartOnlineUpdateInstanceCommands> onlineUpdateInstanceCommandsFactory;
        private readonly WalmartStoreEntity walmartStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="downloaderFactory"></param>
        /// <param name="onlineUpdateInstanceCommandsFactory"></param>
        public WalmartStoreType(StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory,
            Func<WalmartStoreEntity, IWalmartOnlineUpdateInstanceCommands> onlineUpdateInstanceCommandsFactory)
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
        public override OrderItemEntity CreateOrderItemInstance() => new WalmartOrderItemEntity();

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => walmartStore.ConsumerID;

        /// <summary>
        /// Creates the add store wizard online update action control for Walmart
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new OnlineUpdateShipmentUpdateActionControl(typeof(WalmartShipmentUploadTask));

        /// <summary>
        /// Create the online update instance commands for Walmart
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands() =>
            onlineUpdateInstanceCommandsFactory(walmartStore).Create().ToList();

        /// <summary>
        /// Generate the walmart node for the order template
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            Lazy<WalmartOrderEntity> order = new Lazy<WalmartOrderEntity>(() => (WalmartOrderEntity)orderSource());

            ElementOutline outline = container.AddElement("Walmart");
            outline.AddElement("CustomerOrderID", () => order.Value.CustomerOrderID);
            outline.AddElement("PurchaseOrderID", () => order.Value.PurchaseOrderID);
            outline.AddElement("EstimatedDeliveryDate", () => order.Value.EstimatedDeliveryDate);
            outline.AddElement("EstimatedShipDate", () => order.Value.EstimatedShipDate);
        }

        /// <summary>
        /// Generate the walmart node for the order item template
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            Lazy<WalmartOrderItemEntity> item = new Lazy<WalmartOrderItemEntity>(() => (WalmartOrderItemEntity)itemSource());

            ElementOutline outline = container.AddElement("Walmart");
            outline.AddElement("OnlineStatus", () => item.Value.OnlineStatus);
        }

        /// <summary>
        /// Create the initial filters
        /// </summary>
        /// <returns></returns>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<FilterEntity> filters = new List<FilterEntity>
            {
                CreateItemStatusFilter("Ready To Ship", "Acknowledged"),
                CreateItemStatusFilter("Shipped", "Shipped"),
                CreateItemStatusFilter("Cancelled", "Cancelled"),
                CreatePartialItemStatusFilter("Partially Acknowledged", "Acknowledged"),
                CreatePartialItemStatusFilter("Partially Shipped", "Shipped"),
                CreatePartialItemStatusFilter("Partially Cancelled", "Cancelled")
            };

            return filters;
        }

        /// <summary>
        /// Create a filter for any item of the given status but not all the same status
        /// </summary>
        private FilterEntity CreatePartialItemStatusFilter(string filterName, string status)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;

            // [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // For any item
            ForAnyItemCondition anyItem = new ForAnyItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(anyItem);

            WalmartItemOnlineStatusCondition itemStatusCondition = new WalmartItemOnlineStatusCondition();
            itemStatusCondition.Operator = StringOperator.Equals;
            itemStatusCondition.TargetValue = status;

            anyItem.Container.FirstGroup.Conditions.Add(itemStatusCondition);

            // For any item
            ForAnyItemCondition notAnyItem = new ForAnyItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(notAnyItem);

            WalmartItemOnlineStatusCondition notItemStatusCondition = new WalmartItemOnlineStatusCondition();
            notItemStatusCondition.Operator = StringOperator.NotEqual;
            notItemStatusCondition.TargetValue = status;

            notAnyItem.Container.FirstGroup.Conditions.Add(notItemStatusCondition);

            ConditionGroupContainer secondContainer = new ConditionGroupContainer();
            secondContainer.FirstGroup = new ConditionGroup();
            secondContainer.FirstGroup.JoinType = ConditionJoinType.All;
            secondContainer.FirstGroup.Conditions.Add(notAnyItem);

            definition.RootContainer.SecondGroup = secondContainer;


            return new FilterEntity
            {
                Name = filterName,
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        /// Create a filter for all items of the given status
        /// </summary>
        private FilterEntity CreateItemStatusFilter(string filterName, string status)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;

            // [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            ForEveryItemCondition everyItem = new ForEveryItemCondition();
            definition.RootContainer.FirstGroup.Conditions.Add(everyItem);

            WalmartItemOnlineStatusCondition itemStatusCondition = new WalmartItemOnlineStatusCondition();
            itemStatusCondition.Operator = StringOperator.Equals;
            itemStatusCondition.TargetValue = status;

            everyItem.Container.FirstGroup.Conditions.Add(itemStatusCondition);

            return new FilterEntity
            {
                Name = filterName,
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }
    }
}