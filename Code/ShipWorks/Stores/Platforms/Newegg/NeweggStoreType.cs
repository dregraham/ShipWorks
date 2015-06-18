using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.Miva.WizardPages;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Content;
using ShipWorks.Filters.Content;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Platforms.Newegg.WizardPages;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Newegg store type
    /// </summary>
    public class NeweggStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(NeweggStoreType));

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public NeweggStoreType(StoreEntity store)
            : base(store)
        { }
               
        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.NeweggMarketplace; }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.  Like the eBay user ID for ebay,  or store URL for Yahoo!
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((NeweggStoreEntity)Store).SellerID; }
        }


        /// <summary>
        /// Creates a store-specific instance of a StoreEntity
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            NeweggStoreEntity store = new NeweggStoreEntity();
            
            InitializeStoreDefaults(store);

            store.StoreName = "My Newegg Store";
            store.SellerID = string.Empty;
            store.SecretKey = string.Empty;
            store.ExcludeFulfilledByNewegg = false;
            store.Channel = (int)NeweggChannelType.US;

            

            return store;
        }

        /// <summary>
        /// Get the store-specifc OrderIdentifier that can be used to identify the specified order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override Content.OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            if (order == null)
            {
                string message = "A null order value was provided to CreateOrderIdentifier.";

                log.ErrorFormat(message);
                throw new InvalidOperationException(message);
            }
            
            return new Content.OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderEntity
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity CreateOrderInstance()
        {
            NeweggOrderEntity neweggOrderEntity = new NeweggOrderEntity();
            return neweggOrderEntity;
        }

        /// <summary>
        /// Creates a store-specific instance of an OrderItemEntity
        /// </summary>
        /// <returns></returns>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new NeweggOrderItemEntity();
        }
                
        /// <summary>
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        /// <returns></returns>
        public override StoreDownloader CreateDownloader()
        {
            return new NeweggDownloader(this.Store);
        }

        /// <summary>
        /// Create the pages, in order, that will be displayed in the Add Store Wizard
        /// </summary>
        /// <returns></returns>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> wizardPages = new List<WizardPage>()
            {
                new NeweggAccountPage(),
                new NeweggDownloadCriteriaPage()
            };

            return wizardPages;
        }

        /// <summary>
        /// Create the control to use on the Store Settings dialog, for custom store-specific
        /// configuration options.
        /// </summary>
        /// <returns></returns>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new NeweggDownloadStoreSettingsControl();
        }

        /// <summary>
        /// Controls the store's policy for restricting the range of an initial download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get { return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack); }
        }

        /// <summary>
        /// Indicates of the StoreType supports the display of the given "Online" column.  By default it always returns false.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        /// <returns></returns>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new NeweggAccountSettingsControl();
        }



        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        /// <param name="container"></param>
        /// <param name="orderSource"></param>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<NeweggOrderEntity>(() => (NeweggOrderEntity)orderSource());

            ElementOutline outline = container.AddElement("Newegg");
            outline.AddElement("InvoiceNumber", () => order.Value.InvoiceNumber);
        }


        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        /// <returns></returns>
        public override List<ApplicationCore.Interaction.MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand uploadShipmentCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            commands.Add(uploadShipmentCommand);

            return commands;
        }

        /// <summary>
        /// Called when [upload shipment details].
        /// </summary>
        /// <param name="context">The context.</param>
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
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    NeweggOnlineUpdater updater = new NeweggOnlineUpdater((NeweggStoreEntity)Store);
                    updater.UploadShippingDetails(shipment);
                }
                catch (NeweggException ex)
                {
                    // log it
                    log.ErrorFormat("Error uploading shipment information for orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }    
    }
}
