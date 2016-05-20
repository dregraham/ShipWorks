using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;
using log4net;
using ShipWorks.Stores.Platforms.Sears.CoreExtensions.Actions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.Sears.CoreExtensions.Filters;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Sears specific ShipWorks store type implementation
    /// </summary>
    public class SearsStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SearsStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Returns the identifying code for Infopia stores
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Sears;
            }
        }

        /// <summary>
        /// Create a new instance of a sears store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            SearsStoreEntity store = new SearsStoreEntity();
            InitializeStoreDefaults(store);

            store.StoreName = "Sears";
            store.SearsEmail = "";
            store.Password = "";

            return store;
        }

        /// <summary>
        /// Create a Sears specific order instance
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            SearsOrderEntity order = new SearsOrderEntity();

            return order;
        }

        /// <summary>
        /// Create a sears specific item instance
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            SearsOrderItemEntity item = new SearsOrderItemEntity();

            return item;
        }

        /// <summary>
        /// Create an identifier to uniquely identify a sears order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            SearsOrderEntity searsOrder = order as SearsOrderEntity;
            if (searsOrder == null)
            {
                throw new InvalidOperationException("A non sears order was passed to the SearsStoreType.");
            }

            return new SearsOrderIdentifier(searsOrder.OrderNumber, searsOrder.PoNumber);
        }

        /// <summary>
        /// Create a downloader for pulling in Sears.com orders
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new SearsDownloader((SearsStoreEntity) Store);
        }

        /// <summary>
        /// Create the wizard pages for adding sears.com stores to ShipWorks
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>()
                {
                    new WizardPages.SearsAccountPage()
                };
        }

        /// <summary>
        /// Create the account settings control for managing sears.com account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new SearsAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(SearsShipmentUploadTask));
        }

        /// <summary>
        /// The initial download restriction policy for Sears
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { MaxDaysBack = 90 };
            }
        }

        /// <summary>
        /// Determines if certain columns should be visible or not in the grid
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the license identifier for uniquely identifiying Sears.com stores
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                return ((SearsStoreEntity) Store).SearsEmail;
            }
        }

        /// <summary>
        /// Create the condition group for searching on Amazon Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            SearsPoNumberCondition condition = new SearsPoNumberCondition();
            condition.TargetValue = search;
            condition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(condition);

            return group;
        }

        /// <summary>
        /// Create the order elements for the order provided
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<SearsOrderEntity>(() => (SearsOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Sears");
            outline.AddElement("PoNumber", () => order.Value.PoNumber);
            outline.AddElement("PoNumberWithDate", () => order.Value.PoNumberWithDate);
            outline.AddElement("LocationID", () => order.Value.LocationID);
            outline.AddElement("Commission", () => order.Value.Commission);
            outline.AddElement("CustomerPickup", () => order.Value.CustomerPickup);
        }

        /// <summary>
        /// Create the Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<SearsOrderItemEntity>(() => (SearsOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Sears");
            outline.AddElement("OnlineStatus", () => item.Value.OnlineStatus);
            outline.AddElement("Shipping", () => item.Value.Shipping);
            outline.AddElement("Commission", () => item.Value.Commission);
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand command = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
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
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    SearsOnlineUpdater updater = new SearsOnlineUpdater();
                    updater.UploadShipmentDetails(shipment);
                }
                catch (SearsException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to the issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return new[] { "New", "Open", "Closed", "Overdue" };
        }
    }
}
