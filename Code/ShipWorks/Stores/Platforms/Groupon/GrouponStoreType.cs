using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Management;
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
        public override OrderEntity CreateOrderInstance()
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
    }
}
