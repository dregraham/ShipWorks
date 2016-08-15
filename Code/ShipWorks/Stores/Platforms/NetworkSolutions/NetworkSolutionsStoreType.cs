using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Autofac;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Store type for NetworkSolutions integration
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what Network Solutions currently uses")]
    class NetworkSolutionsStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Identifying type code
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.NetworkSolutions; }
        }

        /// <summary>
        /// Creates an instance of the NetworkSolutionsStoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            NetworkSolutionsStoreEntity storeEntity = new NetworkSolutionsStoreEntity();

            InitializeStoreDefaults(storeEntity);

            storeEntity.UserToken = "";
            storeEntity.DownloadOrderStatuses = "";
            storeEntity.StatusCodes = "";
            storeEntity.StoreUrl = "";

            return storeEntity;
        }

        /// <summary>
        /// Creates a NetworkSolutions order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new NetworkSolutionsOrderEntity();
        }

        /// <summary>
        /// Creates the order identifier for locating orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            NetworkSolutionsOrderEntity orderEntity = order as NetworkSolutionsOrderEntity;
            if (orderEntity == null)
            {
                throw new InvalidCastException("A non NetworkSolutions order was passed to the NetworkSolutionsStoreType.");
            }

            return new NetworkSolutionsOrderIdentifier(orderEntity.NetworkSolutionsOrderID);
        }

        /// <summary>
        /// Create a new downloader for NetworkSolutions
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new NetworkSolutionsDownloader(Store);
        }

        /// <summary>
        /// Create the wizard pages for the Add Store Wizard
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new WizardPages.NetworkSolutionsAccountPage(),
                new WizardPages.NetworkSolutionsDownloadStatusPage()
            };
        }

        /// <summary>
        /// Create the control used to configure the Online Update tasks in the setup wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.NetworkSolutionsOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new NetworkSolutionsAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new NetworkSolutionsStoreSettingsControl();
        }

        /// <summary>
        /// Get the store identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                NetworkSolutionsStoreEntity store = Store as NetworkSolutionsStoreEntity;

                if (!string.IsNullOrEmpty(store.StoreUrl))
                {
                    return store.StoreUrl;
                }
                else
                {
                    // The only way StoreUrl wouldn't be filled in is if the auto-conversion processes during 2x migration could
                    // not retrieve it due to some error.  In that case we will fall back to the old 2x way of licensing this store.
                    // We can't use the actual token, because its secure.
                    byte[] bytes = Encoding.UTF8.GetBytes(store.UserToken);

                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        // Generate the hash
                        string result = Convert.ToBase64String(md5.ComputeHash(bytes));

                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the possible online order statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            NetworkSolutionsStatusCodeProvider statusCodes = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) Store);

            return statusCodes.CodeNames;
        }

        /// <summary>
        /// Generate NS specific elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<NetworkSolutionsOrderEntity>(() => (NetworkSolutionsOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("NetworkSolutions");
            outline.AddElement("NetworkSolutionsOrderId", () => order.Value.NetworkSolutionsOrderID);
        }

        /// <summary>
        /// Indicates if the display of the given "Online" column is allowed.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        #region Online Update Commands

        /// <summary>
        /// Create the menu commands for updating order status
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // get possible status codes from the provider
            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) Store);

            // create a menu item for each status
            foreach (long codeValue in codeProvider.CodeValues)
            {
                MenuCommand command = new MenuCommand(codeProvider.GetCodeName(codeValue), new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = codeValue;

                commands.Add(command);
            }

            MenuCommand withComments = new MenuCommand("Set with comments...", new MenuCommandExecutor(OnSetOnlineStatus));
            withComments.Tag = -1L;
            commands.Add(withComments);

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            uploadCommand.BreakBefore = true;
            commands.Add(uploadCommand);

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
                    // upload
                    NetworkSolutionsOnlineUpdater updater = new NetworkSolutionsOnlineUpdater((NetworkSolutionsStoreEntity) Store);
                    updater.UploadShipmentDetails(shipment);
                }
                catch (NetworkSolutionsException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to the issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) Store);

            MenuCommand command = context.MenuCommand;
            long statusCode = (long) command.Tag;
            string comments = "";

            // -1 status indicates this is the Set with Comments..
            if (statusCode == -1)
            {
                // get user input on the status and comments
                using (NetworkSolutionsOnlineStatusCommentDlg dlg = new NetworkSolutionsOnlineStatusCommentDlg(codeProvider))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        statusCode = dlg.Code;
                        comments = dlg.Comments;
                    }
                }
            }

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                       "Set Status",
                       "ShipWorks is setting the online status.",
                       "Updating order {0} of {1}...");


            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };
            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, new object[] { statusCode, comments });
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            log.Debug(Store.StoreName);

            object[] state = (object[]) userState;

            long statusCode = (long) state[0];
            string comments = (string) state[1];
            try
            {
                NetworkSolutionsOnlineUpdater updater = new NetworkSolutionsOnlineUpdater((NetworkSolutionsStoreEntity) Store);
                updater.UpdateOrderStatus(orderID, statusCode, comments);
            }
            catch (NetworkSolutionsException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        #endregion
    }
}
