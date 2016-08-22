﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.WizardPages;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento integration
    /// </summary>
    public class MagentoStoreType : GenericModuleStoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(MagentoStoreType));
						  
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Identifies the store type
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Magento;

        /// <summary>
        /// Log request/responses as Magento
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Magento;

        /// <summary>
        /// The url to support article
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000049745";

        /// <summary>
        /// Create the magento-specific store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            MagentoStoreEntity magentoStore = new MagentoStoreEntity
            {
                MagentoTrackingEmails = false,
                MagentoVersion = (int) MagentoVersion.PhpFile
            };

            InitializeStoreDefaults(magentoStore);

            return magentoStore;
        }

        /// <summary>
        /// Create an order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new MagentoOrderEntity
            {
                MagentoOrderID = 0
            };
        }

        /// <summary>
        /// Creates an order identifier that will locate the order provided in the database.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            string[] splitCompleteOrderNumber = order.OrderNumberComplete.Split(new [] {order.OrderNumber.ToString()}, StringSplitOptions.None);

            return new MagentoOrderIdentifier(order.OrderNumber, splitCompleteOrderNumber[0], splitCompleteOrderNumber[1]);
        }

        /// <summary>
        /// Create the custom wizard pages for Magento
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage> 
            {
                new MagentoAccountPage()
            };
        }

        /// <summary>
        /// Create the control used to create online update actions in the add store wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            MagentoStoreEntity magentoStore = Store as MagentoStoreEntity;

            if (magentoStore == null)
            {
                throw new InvalidOperationException("Not a magento store.");
            }

            return new MagentoOnlineUpdateActionControl(magentoStore.MagentoVersion == (int)MagentoVersion.MagentoTwo);
        }

        /// <summary>
        /// Create the custom account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new MagentoAccountSettingsControl(); 
        }

        /// <summary>
        /// Create the magento-custom control for store options
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new MagentoStoreSettingsControl();
        }

        /// <summary>
        /// Create the menu commands for updating status
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // take actions to Cancel the order
            MenuCommand command = new MenuCommand("Cancel", OnOrderCommand) {Tag = "cancel"};
            commands.Add(command);

            // try to complete the shipment - which creates an invoice (online), uploads shipping details if they exist, and 
            // sets the order "state" online to complete
            command = new MenuCommand("Complete", OnOrderCommand) {Tag = "complete"};
            commands.Add(command);

            // place the order into Hold status
            command = new MenuCommand("Hold", OnOrderCommand) {Tag = "hold"};
            commands.Add(command);

            command = new MenuCommand("With Comments...", OnOrderCommand) {BreakBefore = true};
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for executing order commands
        /// </summary>
        private void OnOrderCommand(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Online Order Action",
                "ShipWorks is executing an action on the order.",
                "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            string action;
            string comments = "";
            if (command.Tag == null)
            {
                // open a window for the user to select an action and comments
                using (MagentoActionCommentsDlg dlg = new MagentoActionCommentsDlg())
                {
                    if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                    {
                        action = dlg.Action;
                        comments = dlg.Comments;
                    }
                    else
                    {
                        // cancel now
                        context.Complete();
                        return;
                    }
                }
            }
            else
            {
                action = (string)command.Tag;
            }

            executor.ExecuteCompleted += (o, e) =>
                {
                    context.Complete(e.Issues, MenuCommandResult.Error);
                };

            executor.ExecuteAsync(ExecuteOrderCommandCallback, context.SelectedKeys, 
                new Dictionary<string, string> { { "action", action }, { "comments", comments } });
        }

        /// <summary>
        /// The worker thread function for executing commands on Magento orders online
        /// </summary>
        private void ExecuteOrderCommandCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            Dictionary<string, string> state = (Dictionary<string, string>)userState;
            string action = state["action"];
            string comments = state["comments"];

            // create the updater and execute the command
            MagentoOnlineUpdater updater = (MagentoOnlineUpdater)CreateOnlineUpdater();

            try
            {
                // lookup the store to get the email sending preference
                MagentoStoreEntity store = StoreManager.GetStore(DataProvider.GetOrderHeader(orderID).StoreID) as MagentoStoreEntity;
                if (store != null)
                {
                    updater.ExecuteOrderAction(orderID, action, comments, store.MagentoTrackingEmails);
                }
                else
                {
                    log.WarnFormat("Cannot execute online command for Magento order id {0}, the store was deleted.", orderID);
                }
            }
            catch (GenericStoreException ex)
            {
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Create a magento online updater
        /// </summary>
        public override GenericStoreOnlineUpdater CreateOnlineUpdater()
        {
            return new MagentoOnlineUpdater((GenericModuleStoreEntity)Store);
        }

        /// <summary>
        /// Create a custom magento downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new MagentoDownloader(Store);
        }

        /// <summary>
        /// Create a custom web client for magento
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            MagentoStoreEntity magentoStore = Store as MagentoStoreEntity;

            if (magentoStore == null)
            {
                throw new InvalidOperationException("Not a magento store.");
            }

            switch ((MagentoVersion)magentoStore.MagentoVersion)
            {
                case MagentoVersion.PhpFile:
                    return new MagentoWebClient(magentoStore);

                case MagentoVersion.MagentoConnect:
                    // for connecting to our Magento Connect Extension via SOAP
                    return new MagentoConnectWebClient(magentoStore);

                case MagentoVersion.MagentoTwo:
                    return new MagentoTwoWebClient(magentoStore);

                default:
                    throw new NotImplementedException("Magento Version not supported");
            }
        }
    }
}
