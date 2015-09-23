using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     LemonStand integration
    /// </summary>
    public class LemonStandStoreType : StoreType
    {
        // Logger 
        private readonly ILog log;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandStoreType(StoreEntity store)
            : this(store, LogManager.GetLogger(typeof(LemonStandStoreType)))
        {
        }

        public LemonStandStoreType(StoreEntity store, ILog log)
            : base(store)
        {
            this.log = log;
        }

        /// <summary>
        ///     Indentifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.LemonStand; }
        }

        /// <summary>
        ///     Gets the license identifier for this store
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
        ///     Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();
            MenuCommand command = new MenuCommand("Upload Shipment Details", OnUploadDetails);
            commands.Add(command);

            return commands;
        }

        /// <summary>
        ///     Command handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<IEnumerable<long>> executor = new BackgroundExecutor<IEnumerable<long>>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                string.Format("Updating {0} orders...", context.SelectedKeys.Count()));

            executor.ExecuteCompleted += (o, e) => { context.Complete(e.Issues, MenuCommandResult.Error); };

            // kick off the execution
            executor.ExecuteAsync(ShipmentUploadCallback, new[] {context.SelectedKeys}, null);
        }

        /// <summary>
        ///     Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(IEnumerable<long> headers, object userState,
            BackgroundIssueAdder<IEnumerable<long>> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                LemonStandOnlineUpdater shipmentUpdater = new LemonStandOnlineUpdater((LemonStandStoreEntity) Store);
                shipmentUpdater.UpdateShipmentDetails(headers);
            }
            catch (LemonStandException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(headers, ex);
            }
        }

        /// <summary>
        ///     Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof (LemonStandShipmentUploadTask));
        }

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
        ///     Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new LemonStandDownloader(Store);
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
            return new LemonStandOrderEntity {LemonStandOrderID = string.Empty};
        }

        /// <summary>
        ///     Creates an instance of an Order Item Entity
        /// </summary>
        /// <returns></returns>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            LemonStandOrderItemEntity entity = new LemonStandOrderItemEntity();

            entity.UrlName = "";
            entity.Cost = "";
            entity.IsOnSale = "";
            entity.SalePriceOrDiscount = "";
            entity.ShortDescription = "";
            entity.Category = "";

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
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<LemonStandOrderItemEntity>(() => (LemonStandOrderItemEntity)itemSource());

            ElementOutline outline = container.AddElement("BigCommerce");
            outline.AddElement("OrderItemID", () => item.Value.OrderItemID);
            outline.AddElement("UrlName", () => item.Value.UrlName);
            outline.AddElement("Cost", () => item.Value.Cost);
            outline.AddElement("IsOnSale", () => item.Value.IsOnSale);
            outline.AddElement("SalePriceOrDiscount", () => item.Value.SalePriceOrDiscount);
            outline.AddElement("ShortDescription", () => item.Value.ShortDescription);
            outline.AddElement("Category", () => item.Value.Category);
        }

        

        /// <summary>
        ///     Creates the OrderIdentifier for locating orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new LemonStandOrderIdentifier(((LemonStandOrderEntity) order).LemonStandOrderID);
        }

        /// <summary>
        ///     Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(EntityField2 field)
        {
            return EntityUtility.IsSameField(field, OrderItemFields.Name);
        }

        /// <summary>
        /// Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            LemonStandStoreEntity store = (LemonStandStoreEntity)Store;
            string lemonStandUrl =  store.StoreURL + "/product";
            string itemUrl = ((LemonStandOrderItemEntity)entity).UrlName;

            string url = string.Format("{0}/{1}", lemonStandUrl, itemUrl);

            WebHelper.OpenUrl(url, owner);
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public static Uri AccountSettingsHelpUrl
        {
            get { return new Uri("http://support.shipworks.com/support/solutions/articles/4000062623"); }
        }

        /// <summary>
        ///     Get any filters that should be created as an initial filter set when the store is first created.  If the list is
        ///     non-empty they will
        ///     be automatically put in a folder that is filtered on the store... so their is no need to test for that in the
        ///     generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            return new List<FilterEntity>
            {
                CreateFilterPaid(),
                CreateFilterShipped(),
                CreateFilterCancelled(),
                CreateFilterQuote()
            };
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
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup();
            shippedDefinition.FirstGroup.JoinType = ConditionJoinType.Any;

            // LemonStand Shipping Status == Shipped
            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "Shipped";
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
        ///     Creates the filter ready to ship.
        /// </summary>
        /// <returns></returns>
        private FilterEntity CreateFilterPaid()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            // For this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "Paid";
            definition.RootContainer.FirstGroup.Conditions.Add(onlineStatus);

            definition.RootContainer.SecondGroup = InitialDataLoader.CreateDefinitionNotShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Paid",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int) FilterTarget.Orders
            };
        }

        private FilterEntity CreateFilterCancelled()
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup();
            shippedDefinition.FirstGroup.JoinType = ConditionJoinType.Any;

            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "Cancelled";
            shippedDefinition.FirstGroup.Conditions.Add(onlineStatus);

            return new FilterEntity
            {
                Name = "Cancelled",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        private FilterEntity CreateFilterQuote()
        {
            // [All]
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            //      [Store] == this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // [AND]
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            ConditionGroupContainer shippedDefinition = new ConditionGroupContainer();
            definition.RootContainer.SecondGroup = shippedDefinition;

            //      [Any]
            shippedDefinition.FirstGroup = new ConditionGroup();
            shippedDefinition.FirstGroup.JoinType = ConditionJoinType.Any;

            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "Quote";
            shippedDefinition.FirstGroup.Conditions.Add(onlineStatus);

            return new FilterEntity
            {
                Name = "Quote",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        ///     Create the search conditions for searching on LemonStand Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            LemonStandOrderIdCondition condition = new LemonStandOrderIdCondition();
            condition.TargetValue = search;
            condition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(condition);

            return group;
        }

        /// <summary>
        /// The initial download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => 
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 7, MaxDaysBack = 30 };
    }
}