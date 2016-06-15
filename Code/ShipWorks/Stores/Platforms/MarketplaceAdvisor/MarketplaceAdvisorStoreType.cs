using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using log4net;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Filters;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// StoreType instance for MarketplaceAdvisor stores
    /// </summary>
    public class MarketplaceAdvisorStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is MarketplaceAdvisorStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of MarketplaceAdvisorStoreEntity.");
            }
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.MarketplaceAdvisor; }
        }

        /// <summary>
        /// License identifier to uniquely identify the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((MarketplaceAdvisorStoreEntity) Store).Username; }
        }

        /// <summary>
        /// Create a new default initialized instance of the store type
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            MarketplaceAdvisorStoreEntity store = new MarketplaceAdvisorStoreEntity();

            InitializeStoreDefaults(store);

            store.AccountType = (int) MarketplaceAdvisorAccountType.OMS;
            store.DownloadFlags = (int) MarketplaceAdvisorOmsFlagTypes.None;

            return store;
        }

        /// <summary>
        /// Create the wizard pages that are used to create the store
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
                {
                    new MarketplaceAdvisorAccountPage(),
                    new MarketplaceAdvisorOmsFlagsPage()
                };
        }

        /// <summary>
        /// Create the control for creating online action tasks in the add store wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new MarketplaceAdvisorOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the identifier to uniquely identify the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            int parcelNumber = 1;

            // See if it contains a parcel postfix
            if (order.OrderNumberComplete.Contains("-"))
            {
                parcelNumber = int.Parse(order.OrderNumberComplete.Substring(order.OrderNumberComplete.IndexOf("-") + 1));
            }

            return new MarketplaceAdvisorOrderNumberIdentifier(order.OrderNumber, parcelNumber);
        }

        /// <summary>
        /// Create the downloader for the store
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) Store;

            if (mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS)
            {
                return new MarketplaceAdvisorOmsDownloader(mwStore);
            }
            else
            {
                return new MarketplaceAdvisorLegacyDownloader(mwStore);
            }
        }

        /// <summary>
        /// Create the control for editing the account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new MarketplaceAdvisorAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) Store;

            if (mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS)
            {
                return new MarketplaceAdvisorStoreSettingsControl();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a MarketplaceAdvisor specific order entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new MarketplaceAdvisorOrderEntity();
        }

        /// <summary>
        /// Create additional conditions that can be used for basic search
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();
            group.JoinType = ShipWorks.Filters.Content.ConditionJoinType.Any;

            MarketplaceAdvisorInvoiceNumberCondition invoiceCondition = new MarketplaceAdvisorInvoiceNumberCondition();
            invoiceCondition.Operator = StringOperator.BeginsWith;
            invoiceCondition.TargetValue = search;
            group.Conditions.Add(invoiceCondition);

            MarketplaceAdvisorSellerOrderNumberCondition sellerCondition = new MarketplaceAdvisorSellerOrderNumberCondition();
            sellerCondition.IsNumeric = false;
            sellerCondition.StringOperator = StringOperator.BeginsWith;
            sellerCondition.StringValue = search;
            group.Conditions.Add(sellerCondition);

            return group;
        }

        /// <summary>
        /// Generate MA specific template order level elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<MarketplaceAdvisorOrderEntity>(() => (MarketplaceAdvisorOrderEntity) orderSource());

            ElementOutline elementV3 = container.AddElement("MarketplaceAdvisor");
            AddTemplateOrderElements(elementV3, order);

            ElementOutline elementLegacy = container.AddElement("Marketworks");
            elementLegacy.AddAttributeLegacy2x();
            AddTemplateOrderElements(elementLegacy, order);
        }

        /// <summary>
        /// Add the standard xml element output for mw
        /// </summary>
        private static void AddTemplateOrderElements(ElementOutline outline, Lazy<MarketplaceAdvisorOrderEntity> order)
        {
            outline.AddElement("SellerOrderNumber", () => order.Value.SellerOrderNumber);
            outline.AddElement("InvoiceNumber", () => order.Value.InvoiceNumber);
            outline.AddElement("ParcelID", () => order.Value.ParcelID);
        }

        /// <summary>
        /// Create the list of commands for MarketplaceAdvisor stores.  These actually could be done as Common commands, but its
        /// a little easier to do them as instance commands, and since there will likely only be one MW account per ShipWorks user,
        /// this is fine.
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> menuCommands = new List<MenuCommand>();

            MenuCommand shipmentCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            shipmentCommand.BreakAfter = true;
            menuCommands.Add(shipmentCommand);

            if (((MarketplaceAdvisorStoreEntity) Store).AccountType == (int) MarketplaceAdvisorAccountType.OMS)
            {
                menuCommands.Add(new MenuCommand("Promote Order", new MenuCommandExecutor(OnPromoteOrder)));
                menuCommands.Add(new MenuCommand("Change Order Flags", new MenuCommandExecutor(OnChangeOrderFlags)));
            }

            return menuCommands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is updating the order online.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // Upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    MarketplaceAdvisorOnlineUpdater updater = new MarketplaceAdvisorOnlineUpdater((MarketplaceAdvisorStoreEntity) Store);
                    updater.UpdateShipmentStatus(shipment);
                }
                catch (MarketplaceAdvisorException ex)
                {
                    // log it
                    log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnPromoteOrder(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Promote Order",
                "ShipWorks is promoting MarketplaceAdvisor orders.",
                "Promoting order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(PromoteOrderCallback, context.SelectedKeys);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void PromoteOrderCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            MarketplaceAdvisorOrderEntity order = DataProvider.GetEntity(orderID) as MarketplaceAdvisorOrderEntity;

            if (order == null)
            {
                log.WarnFormat("Could not load MarketplaceAdvisor order {0} for promoting order.", orderID);
            }
            else
            {
                try
                {
                    MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) Store;

                    if (mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS)
                    {
                        MarketplaceAdvisorOmsClient.Create(mwStore).PromoteOrder(new MarketplaceAdvisorOrderDto(order));
                    }
                    else
                    {
                        issueAdder.Add(orderID, new MarketplaceAdvisorException("Only orders from OMS accounts can be promoted."));
                    }
                }
                catch (MarketplaceAdvisorException ex)
                {
                    // log it
                    log.ErrorFormat("Error promoting order for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnChangeOrderFlags(MenuCommandExecutionContext context)
        {
            MarketplaceAdvisorOmsFlagTypes flagsOn = MarketplaceAdvisorOmsFlagTypes.None;
            MarketplaceAdvisorOmsFlagTypes flagsOff = MarketplaceAdvisorOmsFlagTypes.None;

            // Determine what flags to turn on and off
            using (MarketplaceAdvisorOmsChangeFlagsDlg dlg = new MarketplaceAdvisorOmsChangeFlagsDlg((MarketplaceAdvisorStoreEntity) Store))
            {
                if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    flagsOn = dlg.FlagsOn;
                    flagsOff = dlg.FlagsOff;
                }

                if (flagsOn == MarketplaceAdvisorOmsFlagTypes.None && flagsOff == MarketplaceAdvisorOmsFlagTypes.None)
                {
                    context.Complete();
                    return;
                }
            }

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Change Flags",
                "ShipWorks is changing flags on the orders.",
                "Changing order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(
                ChangeOrderFlagsCallback, 
                context.SelectedKeys,
                new MarketplaceAdvisorOmsFlagTypes[] { flagsOn, flagsOff });
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void ChangeOrderFlagsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            MarketplaceAdvisorOmsFlagTypes[] flagChanges = (MarketplaceAdvisorOmsFlagTypes[]) userState;

            MarketplaceAdvisorOrderEntity order = DataProvider.GetEntity(orderID) as MarketplaceAdvisorOrderEntity;

            if (order == null)
            {
                log.WarnFormat("Could not load MarketplaceAdvisor order {0} for changing flags.", orderID);
            }
            else
            {
                try
                {
                    MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) Store;

                    if (mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS)
                    {
                        MarketplaceAdvisorOmsClient.Create(mwStore).ChangeOrderFlags(new MarketplaceAdvisorOrderDto(order), flagChanges[0], flagChanges[1]);
                    }
                    else
                    {
                        issueAdder.Add(orderID, new MarketplaceAdvisorException("Only orders from OMS accounts have flags."));
                    }
                }
                catch (MarketplaceAdvisorException ex)
                {
                    // log it
                    log.ErrorFormat("Error changing order flags of orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }
    }
}
