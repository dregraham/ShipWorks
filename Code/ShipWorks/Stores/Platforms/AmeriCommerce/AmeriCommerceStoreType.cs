using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Common.Threading;
using log4net;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Grid;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Implementation of the AmeriCommerce store integration
    /// </summary>
    public class AmeriCommerceStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceStoreType));
						  
        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceStoreType(StoreEntity store)
            : base(store)
        {
#pragma warning disable 168,219
            // This is just to prove RewardsPoints and PaymentOrder exists. If it doesn't the WSDL has been updated and RewardPoints and PaymentOrder aren't
            // hacked into the reference.cs file any more.
            var w = PaymentTypes.BillMeLater;
            var x = PaymentTypes.RewardPoints;
            var y = OrderType.PaymentOrder;
            var z = OrderItemType.OrderPayment;
#pragma warning restore 168,219
        }

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.AmeriCommerce; }
        }

        /// <summary>
        /// License Identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                AmeriCommerceStoreEntity amcStore = (AmeriCommerceStoreEntity)Store;

                // combination of the store url and chosen store code
                return String.Format("{0}?{1}", amcStore.StoreUrl, amcStore.StoreCode);
            }
        }

        /// <summary>
        /// The initial download restriction policy for AmeriCommerce
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Determines if certain columns should be visible or not in the grid
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
        /// Create a new store entity instance
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            AmeriCommerceStoreEntity store = new AmeriCommerceStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreCode = 0;
            store.StoreUrl = "";
            store.Username = "";
            store.Password = "";
            store.StatusCodes = "";

            return store;
        }

        /// <summary>
        /// Returns the colleciton of possible online status codes
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            AmeriCommerceStatusCodeProvider provider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity)Store);

            return provider.CodeNames;
        }

        /// <summary>
        /// Create the setup wizard pages needed to configure the store 
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                new WizardPages.AmeriCommerceAccountPage(),
                new WizardPages.AmeriCommerceStoreSelectionPage()
            };
        }

        /// <summary>
        /// Create the online update action creation control that will get shown in the wizard
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.AmeriCommerceOnlineUpdateActionControl();
        }

        /// <summary>
        /// Order identifier/locator
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the store downloader which pulls orders from AmeriCommerce
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new AmeriCommerceDownloader(Store);
        }

        /// <summary>
        /// Create the control for editing account information
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new AmeriCommerceAccountSettingsControl();
        }

        #region Online Update Commands

        /// <summary>
        /// Create the menu commands for updating order status
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // get possible status codes from the provider
            AmeriCommerceStatusCodeProvider codeProvider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity)Store);

            // create a menu item for each status 
            bool isOne = false;
            foreach (int codeValue in codeProvider.CodeValues)
            {
                isOne = true;

                MenuCommand command = new MenuCommand(codeProvider.GetCodeName(codeValue), new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = codeValue;

                commands.Add(command);
            }

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            uploadCommand.BreakBefore = isOne;
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
                    AmeriCommerceOnlineUpdater updater = new AmeriCommerceOnlineUpdater((AmeriCommerceStoreEntity)Store);
                    updater.UploadShipmentDetails(shipment);
                }
                catch (AmeriCommerceException ex)
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
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Set Status",
               "ShipWorks is setting the online status.",
               "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            int statusCode = (int)command.Tag;

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };
            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, statusCode);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            log.Debug(Store.StoreName);

            int statusCode = (int)userState;
            try
            {
                AmeriCommerceOnlineUpdater updater = new AmeriCommerceOnlineUpdater((AmeriCommerceStoreEntity)Store);
                updater.UpdateOrderStatus(orderID, statusCode);
            }
            catch (AmeriCommerceException ex)
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
