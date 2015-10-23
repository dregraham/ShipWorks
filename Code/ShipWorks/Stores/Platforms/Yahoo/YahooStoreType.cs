using System;
using System.Collections.Generic;
using log4net;
using Quartz.Util;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Email;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// StoreType for Yahoo! stores
    /// </summary>
    public class YahooStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is YahooStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of YahooStoreEntity.");
            }
        }

        /// <summary>
        /// The type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Yahoo;

        /// <summary>
        /// License identifier to uniquely identify the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                string storeID = ((YahooStoreEntity) Store).YahooStoreID;

                if (storeID.IsNullOrWhiteSpace())
                {
                    EmailAccountEntity account =
                        EmailAccountManager.GetAccount(((YahooStoreEntity) Store).YahooEmailAccountID);

                    // If the account was deleted we have to create a made up license that obviously will not be activated to them
                    if (account == null)
                    {
                        return $"{Guid.NewGuid()}@noaccount.com";
                    }

                    return account.IncomingUsername;
                }
                else
                {
                    return storeID;
                }
            }
        }

        public string AccountSettingsHelpUrl => "http://www.shipworks.com/shipworks/help/Yahoo_Email_Account.html";

        /// <summary>
        /// Create a new default initialized instance of the store type
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            YahooStoreEntity store = new YahooStoreEntity();

            InitializeStoreDefaults(store);

            store.YahooEmailAccountID = 0;
            store.TrackingUpdatePassword = "";
            store.YahooStoreID = "";
            store.AccessToken = "";
            
            return store;
        }

        /// <summary>
        /// Create the wizard pages that are used to create the store
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
                {
                    new YahooAccountPageHost()
                };
        }

        /// <summary>
        /// Create the control for creating online update tasks for the Yahoo add store wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new YahooOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the identifier to uniquely identify the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new YahooOrderIdentifier(((YahooOrderEntity) order).YahooOrderID);
        }

        /// <summary>
        /// Create a new instance of a yahoo order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new YahooOrderEntity();
        }

        /// <summary>
        /// Create a new instance of a yahoo order item
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new YahooOrderItemEntity();
        }

        /// <summary>
        /// Create the downloader for the store
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new YahooEmailDownloader((YahooStoreEntity) Store);
        }

        /// <summary>
        /// Create the control for editing the account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new YahooEmailAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new YahooEmailStoreSettingsControl();
        }

        /// <summary>
        /// Delete any additional data associated with the store
        /// </summary>
        public override void DeleteStoreAdditionalData(SqlAdapter adapter)
        {
            base.DeleteStoreAdditionalData(adapter);

            // Delete the email account used to check orders
            adapter.DeleteEntity(new EmailAccountEntity(((YahooStoreEntity) Store).YahooEmailAccountID));
        }

        /// <summary>
        /// Generate ProStores specific template XML output
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<YahooOrderEntity>(() => (YahooOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("OrderID", () => order.Value.YahooOrderID);
        }

        /// <summary>
        /// Create specialized template output for yahoo
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<YahooOrderItemEntity>(() => (YahooOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Yahoo");
            outline.AddElement("ProductID", () => item.Value.YahooProductID);
        }

        /// <summary>
        /// Create the commands for updating online
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateCommonCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading the tracking number.",
                "Updating order {0} of {1}...");

            List<EmailOutboundEntity> generatedEmail = new List<EmailOutboundEntity>();

            executor.ExecuteCompleted += (o, e) =>
            {
                // Start emailing for the yahoo email account after generating the online update emails
                EmailCommunicator.StartEmailingMessages(generatedEmail);

                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys, generatedEmail);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            List<EmailOutboundEntity> generatedEmail = (List<EmailOutboundEntity>) userState;

            try
            {
                YahooEmailOnlineUpdater updater = new YahooEmailOnlineUpdater();
                EmailOutboundEntity email = updater.GenerateOrderShipmentUpdateEmail(orderID);

                if (email != null)
                {
                    generatedEmail.Add(email);
                }
            }
            catch (YahooException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }
    }
}
