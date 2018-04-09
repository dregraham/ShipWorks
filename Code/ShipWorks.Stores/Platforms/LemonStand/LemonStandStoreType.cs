﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using log4net;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     LemonStand integration
    /// </summary>
    public class LemonStandStoreType : StoreType
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandStoreType(StoreEntity store) : base(store)
        {
            log = LogManager.GetLogger(typeof(LemonStandStoreType));
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandStoreType(StoreEntity store, Func<Type, ILog> logFactory) : base(store)
        {
            log = logFactory(typeof(LemonStandStoreType));
        }

        /// <summary>
        /// Identifying type code
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.LemonStand;

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                LemonStandStoreEntity store = (LemonStandStoreEntity) Store;

                string identifier = store.StoreURL;

                return identifier;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public static Uri AccountSettingsHelpUrl => new Uri("http://support.shipworks.com/support/solutions/articles/4000062623");

        /// <summary>
        /// The initial download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy =>
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 7, MaxDaysBack = 30 };

        /// <summary>
        ///     Indicates if the StoreType supports the display of the given "Online" column.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        ///     Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            LemonStandStoreEntity store = new LemonStandStoreEntity();

            InitializeStoreDefaults(store);
            store.StoreURL = "";
            store.Token = "";
            store.StoreName = "My LemonStand Store";

            return store;
        }

        /// <summary>
        ///     Create the LemonStand order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new LemonStandOrderEntity();
        }

        /// <summary>
        ///     Creates an instance of an Order Item Entity
        /// </summary>
        /// <returns></returns>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            LemonStandOrderItemEntity entity = new LemonStandOrderItemEntity
            {
                UrlName = "",
                ShortDescription = "",
                Category = ""
            };

            return entity;
        }

        /// <summary>
        ///     Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<LemonStandOrderEntity>(() => (LemonStandOrderEntity) orderSource());

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            ElementOutline outline = container.AddElement("LemonStand");
            outline.AddElement("LemonStandOrderID", () => order.Value.LemonStandOrderID);
        }

        /// <summary>
        ///     Create the Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container,
            Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<LemonStandOrderItemEntity>(() => (LemonStandOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("LemonStand");
            outline.AddElement("OrderItemID", () => item.Value.OrderItemID);
            outline.AddElement("UrlName", () => item.Value.UrlName);
            outline.AddElement("ShortDescription", () => item.Value.ShortDescription);
            outline.AddElement("Category", () => item.Value.Category);
        }

        /// <summary>
        ///     Creates the OrderIdentifier for locating orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new LemonStandOrderIdentifier(((ILemonStandOrderEntity) order).LemonStandOrderID);
        }

        /// <summary>
        /// Get a description for use when auditing an order
        /// </summary>
        public override string GetAuditDescription(IOrderEntity order) =>
            (order as ILemonStandOrderEntity)?.LemonStandOrderID ?? string.Empty;

        /// <summary>
        ///     Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(IStoreEntity store, EntityBase2 entity, EntityField2 field)
        {
            return EntityUtility.IsSameField(field, OrderItemFields.Name);
        }

        /// <summary>
        ///     Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(IStoreEntity store, EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            ILemonStandStoreEntity lemonStandStore = (ILemonStandStoreEntity) store;
            string lemonStandUrl = lemonStandStore.StoreURL + "/product";
            string itemUrl = ((LemonStandOrderItemEntity) entity).UrlName;

            string url = lemonStandUrl + "/" + itemUrl;

            WebHelper.OpenUrl(url, owner);
        }

        /// <summary>
        ///     Get any filters that should be created as an initial filter set when the store is first created.  If the list is
        ///     non-empty they will be automatically put in a folder that is filtered on the store... so there is no need to test
        ///     for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<string> statuses = GetOnlineStatusChoices().ToList();

            return statuses.Select(status => string.Equals(status, "shipped", StringComparison.InvariantCultureIgnoreCase) ?
                CreateFilterShipped() :
                (CreateOrderStatusFilter(status))).ToList();
        }

        /// <summary>
        ///     Creates the filter shipped.
        /// </summary>
        private FilterEntity CreateFilterShipped()
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition
            {
                Operator = EqualityOperator.Equals,
                Value = Store.StoreID
            };
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup { JoinType = ConditionJoinType.Any };

            // LemonStand Shipping Status == Shipped
            OnlineStatusCondition onlineStatus = new OnlineStatusCondition
            {
                Operator = StringOperator.Equals,
                TargetValue = "Shipped"
            };
            shippedDefinition.FirstGroup.Conditions.Add(onlineStatus);

            // [OR]
            shippedDefinition.JoinType = ConditionGroupJoinType.Or;

            // Shipped within Shipworks
            shippedDefinition.SecondGroup = InitialDataLoader.CreateDefinitionShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Shipped",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Create order status filter
        /// </summary>
        private FilterEntity CreateOrderStatusFilter(string orderStatus)
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition
            {
                Operator = EqualityOperator.Equals,
                Value = Store.StoreID
            };
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup { JoinType = ConditionJoinType.Any };

            OnlineStatusCondition onlineStatus = new OnlineStatusCondition
            {
                Operator = StringOperator.Equals,
                TargetValue = orderStatus
            };
            shippedDefinition.FirstGroup.Conditions.Add(onlineStatus);

            return new FilterEntity
            {
                Name = orderStatus,
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        /// <summary>
        /// Get online status choices
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            LemonStandWebClient client = new LemonStandWebClient((LemonStandStoreEntity) Store);

            List<string> list;
            try
            {
                List<JToken> statuses = client.GetOrderStatuses().SelectToken("data").Children().ToList();
                list = statuses.Select(status => status.SelectToken("name").ToString()).ToList();

                list.Sort();
            }
            catch (LemonStandException ex)
            {
                // If we can't get online status choices, just return empty list instead of crashing.
                list = new List<string>();
                log.Error($"Error retrieving order status list from LemonStand: {ex.Message}");
            }

            return list;
        }
    }
}