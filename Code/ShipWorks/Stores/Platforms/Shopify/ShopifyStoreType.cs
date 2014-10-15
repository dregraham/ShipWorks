using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify.CoreExtensions.Filters;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.UI.Wizard;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class ShopifyStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is ShopifyStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not an instance of ShopifyStoreEntity.");
            }
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Shopify;
            }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                string identifier = ((ShopifyStoreEntity) Store).ShopifyShopUrlName.ToLowerInvariant();

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Creates a new instance of an Shopify store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ShopifyStoreEntity shopifyStore = new ShopifyStoreEntity();

            InitializeStoreDefaults(shopifyStore);

            shopifyStore.ShopifyAccessToken = string.Empty;
            shopifyStore.ShopifyShopUrlName = string.Empty;
            shopifyStore.ShopifyShopDisplayName = string.Empty;
            shopifyStore.ShopifyRequestedShippingOption = (int) ShopifyRequestedShippingField.Title;

            return shopifyStore;
        }

        /// <summary>
        /// Get the identifier object that is used to uniquely identify the specified order for the store.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            return new ShopifyOrderIdentifier(((ShopifyOrderEntity)order).ShopifyOrderID);
        }

        /// <summary>
        /// Create the custom Shopify entity
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            ShopifyOrderEntity order = new ShopifyOrderEntity();
            return order;
        }

        /// <summary>
        /// Creates the custom OrderItem entity.
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance()
        {
            return new ShopifyOrderItemEntity();
        }

        /// <summary>
        /// Get a list of supported online Shopify statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return EnumHelper.GetEnumList<ShopifyStatus>().Select(status => status.Description).ToList();
        }

        /// <summary>
        /// Create the fields required to uniquely identify the online customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create a downloader for our current store instance
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new ShopifyDownloader((ShopifyStoreEntity) Store);
        }

        /// <summary>
        /// Create the pages to be displayed in the Add Store Wizard
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage>();

            pages.Add(new WizardPages.ShopifyAssociateAccountPage());

            return pages;
        }

        /// <summary>
        /// Create the control used to configurd the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(Shopify.CoreExtensions.Actions.ShopifyShipmentUploadTask));
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new ShopifyAccountSettingsControl();
        }

        /// <summary>
        /// Indicates what columns are supported for this store type
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Generate the template XML output for the given order
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var order = new Lazy<ShopifyOrderEntity>(() => (ShopifyOrderEntity)orderSource());

            ElementOutline outline = container.AddElement("Shopify");
            outline.AddElement("OrderID", () => order.Value.ShopifyOrderID);
            outline.AddElement("FulfillmentStatus", () => EnumHelper.GetDescription((ShopifyFulfillmentStatus) order.Value.FulfillmentStatusCode));
            outline.AddElement("PaymentStatus", () => EnumHelper.GetDescription((ShopifyPaymentStatus)order.Value.PaymentStatusCode));
        }

        /// <summary>
        /// Generate the template XML output for the given order item
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var item = new Lazy<ShopifyOrderItemEntity>(() => (ShopifyOrderItemEntity)itemSource());
            
            ElementOutline outline = container.AddElement("Shopify");
            outline.AddElement("ItemID", () => item.Value.ShopifyOrderItemID);
            outline.AddElement("ProductID", () => item.Value.ShopifyProductID);
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadDetails));
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            // kick off the execution
            executor.ExecuteAsync(UploadDetailsCallback, context.SelectedKeys, null);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void UploadDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                ShopifyOrderEntity order = (ShopifyOrderEntity) DataProvider.GetEntity(orderID);
                if (order == null)
                {
                    log.WarnFormat("Not uploading shipment details for order {0} as it went away.", orderID);
                    return;
                }

                ShopifyOnlineUpdater updater = new ShopifyOnlineUpdater((ShopifyStoreEntity) Store);
                updater.UpdateOnlineStatus(order);
            }
            catch (ShopifyException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Create and return the Shopify store settings control.
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new ShopifyStoreSettingsControl();
        }
    }
}
