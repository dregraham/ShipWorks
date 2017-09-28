using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// StoreType for Yahoo! stores
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Yahoo)]
    [Component(RegistrationType.Self)]
    public class YahooStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is YahooStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of YahooStoreEntity.");
            }
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Yahoo;

        /// <summary>
        /// License identifier to uniquely identify the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                YahooStoreEntity store = (YahooStoreEntity) Store;

                if (store == null)
                {
                    throw new YahooException("Attempted to get Yahoo Internal License Identifier for a non Yahoo store");
                }

                if (!store.YahooStoreID.IsNullOrWhiteSpace())
                {
                    return store.YahooStoreID;
                }

                EmailAccountEntity account =
                    EmailAccountManager.GetAccount(store.YahooEmailAccountID);

                // If the account was deleted we have to create a made up license that obviously will not be activated to them
                return account == null ? $"{Guid.NewGuid()}@noaccount.com" : account.IncomingUsername;
            }
        }

        /// <summary>
        /// Link to article on adding a yahoo store
        /// </summary>
        public string AccountSettingsHelpUrl => "http://support.shipworks.com/solution/articles/4000068682-adding-a-yahoo-store-using-api";

        /// <summary>
        /// Link to article explaining how to renew an expired access token.
        /// </summary>
        public string InvalidAccessTokenHelpUrl => "http://support.shipworks.com/solution/articles/4000069621-renewing-a-yahoo-access-token";

        /// <summary>
        /// Create a new default initialized instance of the store type
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            YahooStoreEntity store = new YahooStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreName = "My Yahoo Store";
            store.YahooEmailAccountID = 0;
            store.TrackingUpdatePassword = "";
            store.YahooStoreID = "";
            store.AccessToken = "";

            return store;
        }

        /// <summary>
        /// Create the wizard pages that are used to create the store
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
                {
                    IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<WizardPage>(StoreTypeCode.Yahoo)
                };
        }

        /// <summary>
        /// Create the identifier to uniquely identify the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            IYahooOrderEntity yahooOrder = order as IYahooOrderEntity;

            if (yahooOrder == null)
            {
                throw new YahooException("Attempted to create a Yahoo order identifier for a non-Yahoo order");
            }

            return new YahooOrderIdentifier(yahooOrder.YahooOrderID);
        }

        /// <summary>
        /// Create a new instance of a yahoo order
        /// </summary>
        protected override OrderEntity CreateOrderInstance() => new YahooOrderEntity();

        /// <summary>
        /// Create a new instance of a yahoo order item
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance() => new YahooOrderItemEntity();

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            YahooStoreEntity store = (YahooStoreEntity) Store;

            return store.YahooStoreID.IsNullOrWhiteSpace() ?
                new YahooEmailStoreSettingsControl() :
                null;
        }

        /// <summary>
        /// Delete any additional data associated with the store
        /// </summary>
        public override void DeleteStoreAdditionalData(SqlAdapter adapter)
        {
            base.DeleteStoreAdditionalData(adapter);

            // Delete the email account used to check orders
            adapter.DeleteEntity(new EmailAccountEntity(((YahooStoreEntity) Store).YahooEmailAccountID));
        }

        /// <summary>
        /// Generate ProStores specific template XML output
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<YahooOrderEntity>(() => (YahooOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("OrderID", () => order.Value.YahooOrderID);
        }

        /// <summary>
        /// Create specialized template output for yahoo
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<YahooOrderItemEntity>(() => (YahooOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("ProductID", () => item.Value.YahooProductID);
        }

        #region Api Integration

        /// <summary>
        /// Creates the initial filters from yahoo's online statuses, if using the Api integration
        /// </summary>
        /// <returns>The list of initial filters</returns>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<FilterEntity> filters = new List<FilterEntity>();

            if (((YahooStoreEntity) Store).YahooStoreID.IsNullOrWhiteSpace())
            {
                return filters;
            }

            foreach (EnumEntry<YahooApiOrderStatus> status in EnumHelper.GetEnumList<YahooApiOrderStatus>())
            {
                filters.Add(CreateOrderStatusFilter(status.Description));
            }

            return filters;
        }

        /// <summary>
        /// Creates an order status filter for the given order status
        /// </summary>
        /// <param name="orderStatus">The order status to create a filter for</param>
        /// <returns>A filter entity for the given order status</returns>
        private FilterEntity CreateOrderStatusFilter(string orderStatus)
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            // [Store] == this store
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

            // [Any]
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
        ///     Indicates if the StoreType supports the display of the given "Online" column.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            YahooStoreEntity store = (YahooStoreEntity) Store;

            if (store.YahooStoreID.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(IStoreEntity store, EntityBase2 entity, EntityField2 field)
        {
            IYahooStoreEntity yahooStore = (IYahooStoreEntity) store;

            if (yahooStore.YahooStoreID.IsNullOrWhiteSpace())
            {
                return false;
            }

            bool isSameField = EntityUtility.IsSameField(field, OrderItemFields.Name);

            YahooOrderItemEntity item = entity as YahooOrderItemEntity;

            if (item != null && isSameField)
            {
                return !item.Url.IsNullOrWhiteSpace();
            }

            return isSameField;
        }

        /// <summary>
        ///     Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(IStoreEntity store, EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            YahooOrderItemEntity item = entity as YahooOrderItemEntity;

            if (item != null && !item.Url.IsNullOrWhiteSpace())
            {
                WebHelper.OpenUrl(item.Url, owner);
            }
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            YahooStoreEntity store = (YahooStoreEntity) Store;

            if (store == null)
            {
                throw new YahooException("Attempted to create Yahoo online update action control for a non Yahoo store");
            }

            return store.YahooStoreID.IsNullOrWhiteSpace() ? null : new OnlineUpdateShipmentUpdateActionControl(typeof(YahooShipmentUploadTask));
        }
        #endregion
    }
}
