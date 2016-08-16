using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BuyDotCom.WizardPages;
using ShipWorks.FileTransfer;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BuyDotCom.CoreExtensions.Actions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using log4net;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// StoreType instance for Buy.com
    /// </summary>
    public class BuyDotComStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComStoreType));

        BuyDotComStoreEntity buyDotComStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComStoreType(StoreEntity store) 
            : base(store)
        {
            buyDotComStore = store as BuyDotComStoreEntity;
            if (store != null && buyDotComStore == null)
            {
                throw new ArgumentException("StoreEntity is not instance of BuyDotComStoreEntity.");
            }
        }

        /// <summary>
        /// Returns Buy.com TypeCode
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.BuyDotCom;
            }
        }

        /// <summary>
        /// Creates a new buy.com store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            BuyDotComStoreEntity newStore = new BuyDotComStoreEntity();
            
            InitializeStoreDefaults(newStore);

            newStore.StoreName = "Buy.com Store";

            return newStore;
        }

        /// <summary>
        /// Create a Buy.com specific orderitem entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new BuyDotComOrderItemEntity();
        }

        /// <summary>
        /// Returns buy.com order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Return buy.com downloader.
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new BuyDotComDownloader(buyDotComStore);
        }

        /// <summary>
        /// Returns setup wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new BuyDotComCredentialsPage()
            };
        }

        /// <summary>
        /// Returns the CreateAccountSettings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new BuyDotComAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(BuyDotComShipmentUploadTask));
        }

        /// <summary>
        /// Create the customer Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<BuyDotComOrderItemEntity>(() => (BuyDotComOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("BuyDotCom");
            outline.AddElement("ReceiptItemID", () => item.Value.ReceiptItemID);
            outline.AddElement("ListingID", () => item.Value.ListingID);
            outline.AddElement("Shipping", () => item.Value.Shipping);
            outline.AddElement("Tax", () => item.Value.Tax);
            outline.AddElement("Commission", () => item.Value.Commission);
            outline.AddElement("ItemFee", () => item.Value.ItemFee);
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
                BuyDotComOnlineUpdater shipmentUpdater = new BuyDotComOnlineUpdater(buyDotComStore);
                shipmentUpdater.UploadOrderShipmentDetails(headers);
            }
            catch (BuyDotComException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(headers, ex);
            }
        }

        /// <summary>
        /// LicenseIdentifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                return buyDotComStore.FtpUsername;
            }
        }
    }
}
