using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Infopia.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Integration with the Infopia platform
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what Infopia currently uses")]
    public class InfopiaStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(InfopiaStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates the entity for the store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            InfopiaStoreEntity storeEntity = new InfopiaStoreEntity();
            InitializeStoreDefaults(storeEntity);

            // infopia initialization
            storeEntity.ApiToken = "";

            return storeEntity;
        }

        /// <summary>
        /// Returns an identifier for finding infopia orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new InfopiaOrderItemEntity();
        }

        /// <summary>
        /// Instantiate the downloader used to retrieve orders
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new InfopiaDownloader((InfopiaStoreEntity) Store);
        }

        /// <summary>
        /// Instantiate and return the setup wizard pages for Infopia
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>()
            {
                new InfopiaTokenWizardPage()
            };
        }

        /// <summary>
        /// Create the control for creating online update actions in the wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new InfopiaOnlineUpdateActionControl();
        }

        /// <summary>
        /// Instantiate and return the control for editing the Infopia account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new InfopiaAccountSettingsControl();
        }

        /// <summary>
        /// Returns the identifying code for Infopia stores
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.Infopia; }
        }

        /// <summary>
        /// Return value that uniquely identifies this store instance
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                InfopiaStoreEntity store = (InfopiaStoreEntity) Store;

                // We can't use the actual token, because its secure.
                byte[] bytes = Encoding.UTF8.GetBytes(store.ApiToken);

                MD5 md5 = new MD5CryptoServiceProvider();

                // Generate the hash
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }

        /// <summary>
        /// Get the online status choices for the store. This is used to populate the Online Status filter dropdown.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return InfopiaUtility.StatusValues;
        }

        /// <summary>
        /// Infopia supports both "Online" columns
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus ||
                column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Get the policy for the initial order download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Generate Infopia specific template item elements
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<InfopiaOrderItemEntity>(() => (InfopiaOrderItemEntity) itemSource());

            // This is legacy format.  If we want to add it for real for v3, I think maybe we should create a new element "Infopia"
            ElementOutline outline = container.AddElement("Marketplace");
            outline.AddAttributeLegacy2x();
            outline.AddElement("Name", () => item.Value.Marketplace);
            outline.AddElement("BuyerID", () => item.Value.BuyerID);
            outline.AddElement("ItemID", () => item.Value.MarketplaceItemID);
        }

        /// <summary>
        /// Online Update commands for Infopia
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // statuses
            MenuCommand command;
            foreach (string status in InfopiaUtility.StatusValues)
            {
                command = new MenuCommand(status, new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = status;

                commands.Add(command);
            }

            commands.Add(new MenuCommand("Upload shipment details", new MenuCommandExecutor(OnUploadShipmentDetails))
            {
                BreakBefore = true
            });

            return commands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
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
                    // upload
                    InfopiaOnlineUpdater updater = new InfopiaOnlineUpdater((InfopiaStoreEntity) Store);
                    updater.UploadShipmentDetails(shipment);
                }
                catch (InfopiaException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to the issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Set the online status for the selected orders
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Set Status",
                "ShipWorks is setting the online status.",
                "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            string status = command.Tag as string;

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);

            };
            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, status);
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            log.Debug(Store.StoreName);

            string status = (string) userState;
            try
            {
                InfopiaOnlineUpdater updater = new InfopiaOnlineUpdater((InfopiaStoreEntity) Store);
                updater.UpdateOrderStatus(orderID, status);
            }
            catch (InfopiaException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
