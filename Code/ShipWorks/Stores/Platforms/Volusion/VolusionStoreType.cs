using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Volusion.WizardPages;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using log4net;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Volusion integration type
    /// </summary>
    public class VolusionStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Indentifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Volusion; }
        }

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                VolusionStoreEntity store = (VolusionStoreEntity)Store;

                string identifier = store.StoreUrl;
                if (identifier.EndsWith("/"))
                {
                    identifier = identifier.Substring(0, identifier.Length - 1);
                }

                return identifier;
            }
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            VolusionStoreEntity store = new VolusionStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreUrl = "";
            store.WebUserName = "";
            store.WebPassword = "";
            store.ApiPassword = "";
            store.ShipmentMethods = "";
            store.PaymentMethods = "";
            store.DownloadOrderStatuses = "Ready to Ship, New";

            return store;
        }

        /// <summary>
        /// Creates the OrderIdentifier for locating Volusion orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Creates the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new VolusionDownloader(Store);
        }

        /// <summary>
        /// Create the wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new VolusionAccountPage(),
                new VolusionImportPage(),
                new VolusionTimeZonePage()
            };
        }

        /// <summary>
        /// Create the control to appear in the setup wizard for configuring 
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new VolusionOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new VolusionAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        /// <returns></returns>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new VolusionStoreSettingsControl();
        }

        /// <summary>
        /// Gets the collection of online status values
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return new List<string>
            {
                "New",
                "Pending",
                "Processing", 
                "Payment Declined",
                "Awaiting Payment", 
                "Ready to Ship",
                "Pending Shipment",
                "Partially Shipped",
                "Shipped",
                "Partially Backordered",
                "Backordered",
                "See Line Items",
                "See Order Notes",
                "Partially Returned",
                "Returned"
            };
        }

        #region Online Update Commands

        /// <summary>
        /// Create online update commands
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            return new List<MenuCommand>
            {
                new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails))
            };
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Upload Shipment",
               "ShipWorks is uploading shipment information",
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
            // get the store from the order
            VolusionStoreEntity store = (VolusionStoreEntity) StoreManager.GetRelatedStore(orderID);

            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    // upload
                    VolusionOnlineUpdater updater = new VolusionOnlineUpdater(store);
                    updater.UploadShipmentDetails(shipment, true);
                }
                catch (VolusionException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to the issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        #endregion
    }
}
