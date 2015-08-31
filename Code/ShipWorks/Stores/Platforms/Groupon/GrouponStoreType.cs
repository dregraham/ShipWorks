using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon.WizardPages;
using ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Actions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Net;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Indentifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Groupon; }
        }

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GrouponStoreEntity store = (GrouponStoreEntity)Store;

                string identifier = store.SupplierID;

                return identifier;
            }
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand command = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<IEnumerable<long>> executor = new BackgroundExecutor<IEnumerable<long>>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                string.Format("Updating {0} orders...", context.SelectedKeys.Count()));

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            // kick off the execution
            executor.ExecuteAsync(ShipmentUploadCallback, new IEnumerable<long>[] { context.SelectedKeys }, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(IEnumerable<long> headers, object userState, BackgroundIssueAdder<IEnumerable<long>> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                GrouponOnlineUpdater shipmentUpdater = new GrouponOnlineUpdater((GrouponStoreEntity)Store);
                shipmentUpdater.UpdateShipmentDetails(headers);
            }
            catch (GrouponException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(headers, ex);
            }
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(GrouponShipmentUploadTask));
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            GrouponAccountSettingsControl settingsControl = new GrouponAccountSettingsControl();

            return settingsControl;
        }


        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new GrouponStoreAccountPage()
            };
        }

        /// <summary>
        /// Indicates if the StoreType supports the display of the given "Online" column.  
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
        /// Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new GrouponDownloader(Store);
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GrouponStoreEntity store = new GrouponStoreEntity();

            InitializeStoreDefaults(store);

            store.SupplierID = "";
            store.Token = "";
            store.StoreName = "Groupon";

            return store;
        }

        /// <summary>
        /// Create the Groupon order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new GrouponOrderEntity { GrouponOrderID = string.Empty};
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            GrouponOrderItemEntity entity = new GrouponOrderItemEntity();

            entity.Permalink = "";
            entity.ChannelSKUProvided = "";
            entity.FulfillmentLineItemID = "";
            entity.BomSKU = "";
            entity.GrouponLineItemID = "";

            return entity;
        }

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<GrouponOrderEntity>(() => (GrouponOrderEntity)orderSource());

            ElementOutline outline = container.AddElement("Groupon");
            outline.AddElement("GrouponOrderID", () => order.Value.GrouponOrderID);
        }

        /// <summary>
        /// Creates the OrderIdentifier for locating Volusion orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new GrouponOrderIdentifier(((GrouponOrderEntity)order).GrouponOrderID);
        }

        /// <summary>
        /// Indicates what basic grid fields we support hyperlinking for
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
            string grouponURL = "http://www.groupon.com/deals";
            string itemPermalink = ((GrouponOrderItemEntity)entity).Permalink;

            string itemURL = string.Format("{0}/{1}", grouponURL, itemPermalink);

            WebHelper.OpenUrl(itemURL, owner);
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public static string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/support/solutions/articles/4000046208"; }
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            return new List<FilterEntity>
            {
                CreateFilterReadyToShip(),
                CreateFilterShipped()
            };
        }

        /// <summary>
        /// Creates the filter shipped.
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

            // ChannelAdvisor Shipping Status == Shipped
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
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        /// Creates the filter ready to ship.
        /// </summary>
        /// <returns></returns>
        private FilterEntity CreateFilterReadyToShip()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.JoinType = ConditionGroupJoinType.And;
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.All;

            // For this store
            StoreCondition storeCondition = new StoreCondition();
            storeCondition.Operator = EqualityOperator.Equals;
            storeCondition.Value = Store.StoreID;
            definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

            // Channel advisor says it has to be unshipped
            OnlineStatusCondition onlineStatus = new OnlineStatusCondition();
            onlineStatus.Operator = StringOperator.Equals;
            onlineStatus.TargetValue = "open";
            definition.RootContainer.FirstGroup.Conditions.Add(onlineStatus);

            definition.RootContainer.SecondGroup = InitialDataLoader.CreateDefinitionNotShipped().RootContainer;

            return new FilterEntity
            {
                Name = "Ready to Ship",
                Definition = definition.GetXml(),
                IsFolder = false,
                FilterTarget = (int)FilterTarget.Orders
            };
        }

        /// <summary>
        /// Create the search conditions for searching on Amazon Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            GrouponOrderIdCondition condition = new GrouponOrderIdCondition();
            condition.TargetValue = search;
            condition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(condition);

            return group;
        }
    }
}
