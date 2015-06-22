using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters;
using ShipWorks.Stores.Management;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShipWorks.Templates.Processing;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Store implementation for ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorStoreType));

        /// <summary>
        /// Store Type 
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.ChannelAdvisor; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// String uniquely identifying a store instance
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get 
            {
                ChannelAdvisorStoreEntity caStore = (ChannelAdvisorStoreEntity)Store;

                return caStore.ProfileID.ToString();
            }
        }

        /// <summary>
        /// Creates a store entity instance
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ChannelAdvisorStoreEntity caStore = new ChannelAdvisorStoreEntity();

            InitializeStoreDefaults(caStore);

            caStore.AccountKey = "";
            caStore.ProfileID = 0;
            caStore.AttributesToDownload = "<Attributes></Attributes>";

            return caStore; 
        }

        /// <summary>
        /// Create the CA order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            ChannelAdvisorOrderEntity entity = new ChannelAdvisorOrderEntity();

            entity.CustomOrderIdentifier = "";
            entity.ResellerID = "";
            entity.OnlinePaymentStatus = (int) ChannelAdvisorPaymentStatus.NoChange;
            entity.OnlineCheckoutStatus = (int) ChannelAdvisorCheckoutStatus.NoChange;
            entity.OnlineShippingStatus = (int) ChannelAdvisorShippingStatus.NoChange;
            entity.FlagStyle = "";
            entity.FlagDescription = "";
            entity.FlagType = (int) ChannelAdvisorFlagType.NoFlag;
            entity.MarketplaceNames = "";

            return entity;
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            ChannelAdvisorOrderItemEntity entity = new ChannelAdvisorOrderItemEntity();

            entity.MarketplaceName = "";
            entity.MarketplaceStoreName = "";
            entity.MarketplaceBuyerID = "";
            entity.MarketplaceSalesID = "";
            entity.Classification = "";
            entity.DistributionCenter = "";
            entity.HarmonizedCode = "";
            entity.IsFBA = false;

            return entity;
        }

        /// <summary>
        /// Creates the order identifier 
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the custom downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new ChannelAdvisorDownloader(Store);
        }

        /// <summary>
        /// Create the wizard pages used to set the store up
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new ChannelAdvisorAccountPage(),
            };
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(ChannelAdvisorShipmentUploadTask));
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            var ShippingStatuses = EnumHelper.GetEnumList<ChannelAdvisorShippingStatus>()
                .Where(status => status.Value != ChannelAdvisorShippingStatus.NoChange && status.Value != ChannelAdvisorShippingStatus.Unknown)
                .Select(statusEnumEntry => statusEnumEntry.Value)
                .ToList();

            List<FilterEntity> filters = new List<FilterEntity>();

            foreach (ChannelAdvisorShippingStatus shippingStatus in ShippingStatuses)
            {
                FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
                definition.RootContainer.JoinType = ConditionGroupJoinType.And;

                ChannelAdvisorShippingStatusCondition shippingStatusCondition = new ChannelAdvisorShippingStatusCondition();
                shippingStatusCondition.Operator = EqualityOperator.Equals;
                shippingStatusCondition.Value = shippingStatus;
                definition.RootContainer.FirstGroup.Conditions.Add(shippingStatusCondition);

                StoreCondition storeCondition = new StoreCondition();
                storeCondition.Operator = EqualityOperator.Equals;
                storeCondition.Value = Store.StoreID;
                definition.RootContainer.FirstGroup.Conditions.Add(storeCondition);

                filters.Add(new FilterEntity()
                {
                    Name =  EnumHelper.GetDescription(shippingStatus),
                    Definition = definition.GetXml(),
                    IsFolder = false,
                    FilterTarget = (int)FilterTarget.Orders
                });
            }

            return filters;
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ChannelAdvisorAccountSettingsControl();
        }

        /// <summary>
        /// Create the CA store settings
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new ChannelAdvisorStoreSettingsControl();
        }

        /// <summary>
        /// Create the condition group for searching on Channel Advisor Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            ChannelAdvisorOrderIDCondition caCondition = new ChannelAdvisorOrderIDCondition();
            caCondition.TargetValue = search;
            caCondition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(caCondition);

            return group;
        }

        /// <summary>
        /// ChannelAdvisor does not have an Online Status
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the CA download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Generate CA specific template order elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<ChannelAdvisorOrderEntity>(() => (ChannelAdvisorOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("ChannelAdvisor");
            outline.AddElement("ResellerID", () => order.Value.ResellerID);
            outline.AddElement("OrderID", () => order.Value.CustomOrderIdentifier);
            outline.AddElement("FlagStyle", () => order.Value.FlagStyle);
            outline.AddElement("FlagDescription", () => order.Value.FlagDescription);
        }

        /// <summary>
        /// Generate CA specific template item elements
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<ChannelAdvisorOrderItemEntity>(() => (ChannelAdvisorOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("ChannelAdvisor");
            outline.AddElement("Classification", () => item.Value.Classification);
            outline.AddElement("DistributionCenter", () => item.Value.DistributionCenter);
            outline.AddElement("HarmonizedCode", () => item.Value.HarmonizedCode);
            outline.AddElement("FulfilledByAmazon", () => item.Value.IsFBA);
            outline.AddElement("MPN", () => item.Value.MPN);

            // These are a child of the "ChannelAdvisor" element
            ElementOutline marketplace = outline.AddElement("Marketplace");
            marketplace.AddElement("Name", () => item.Value.MarketplaceName);
            marketplace.AddElement("StoreName", () => item.Value.MarketplaceStoreName);
            marketplace.AddElement("BuyerID", () => item.Value.MarketplaceBuyerID);
            marketplace.AddElement("SalesID", () => item.Value.MarketplaceSalesID);

            // The legacy ones were a child of the Order element
            ElementOutline legacy = container.AddElement("Marketplace");
            legacy.AddAttributeLegacy2x();
            legacy.AddElement("Name", () => item.Value.MarketplaceName);
            legacy.AddElement("StoreName", () => item.Value.MarketplaceStoreName);
            legacy.AddElement("BuyerID", () => item.Value.MarketplaceBuyerID);
            legacy.AddElement("ItemID", () => item.Value.MarketplaceSalesID);
        }

        /// <summary>
        /// Create menu commands for uploading shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // shipment details
            MenuCommand command = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
                {
                    context.Complete(e.Issues, MenuCommandResult.Error);
                };

            executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    ChannelAdvisorOnlineUpdater updater = new ChannelAdvisorOnlineUpdater((ChannelAdvisorStoreEntity)Store);
                    updater.UploadTrackingNumber(shipment);
                }
                catch (ChannelAdvisorException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment information for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }
    }
}
