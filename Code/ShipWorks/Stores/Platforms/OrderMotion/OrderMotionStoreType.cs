using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.OrderMotion.WizardPages;
using ShipWorks.Email.Accounts;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid.Paging;
using log4net;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Grid;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Store integration for OrderMotion
    /// </summary>
    public class OrderMotionStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderMotionStoreType));

        /// <summary>
        /// Identifying typecode for OrderMotion
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.OrderMotion; }
        }

        /// <summary>
        /// Identifying account information for licensing
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                EmailAccountEntity account = EmailAccountManager.GetAccount(((OrderMotionStoreEntity)Store).OrderMotionEmailAccountID);

                // If the account was deleted we have to create a made up license that obviously will not be activated to them
                if (account == null)
                {
                    return string.Format("{0}@noaccount.com", Guid.NewGuid());
                }

                return account.EmailAddress;
            }
        }

        /// <summary>
        /// Creates a store entity for this store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            OrderMotionStoreEntity store = new OrderMotionStoreEntity();

            InitializeStoreDefaults(store);

            // set specific defaults
            store.OrderMotionBizID = "";
            store.OrderMotionEmailAccountID = 0;

            return store;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Create an OrderMotion orderj
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new OrderMotionOrderEntity();
        }

        /// <summary>
        /// Create the order identifier to locate orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            OrderMotionOrderEntity omOrder = order as OrderMotionOrderEntity;

            return new OrderMotionOrderIdentifier(omOrder.OrderNumber, omOrder.OrderMotionShipmentID);
        }

        /// <summary>
        /// Create the store downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new OrderMotionDownloader(Store);
        }

        /// <summary>
        /// Create the Add Store wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
            {
                new OrderMotionAccountPage(),
                new OrderMotionEmailPage()
            };
        }

        /// <summary>
        /// Create Account Settings Control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new OrderMotionAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for configuring task creation during the setup wizard.
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OrderMotionOnlineUpdateActionControl();
        }

        /// <summary>
        /// Generate OrderMotion specific template xml elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<OrderMotionOrderEntity>(() => (OrderMotionOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("OrderMotion");

            // Outputting both elements despite using the same value for historical purposes so existing templates
            // do not break. The shipment number value was originally built by combining the order motion ID and 
            // the order motion shipment ID values which were parsed out of the INVOICE_NO field from the order download.
            outline.AddElementLegacy2x("ShipmentNumber", () => order.Value.OrderMotionInvoiceNumber);
            outline.AddElement("InvoiceNumber", () => order.Value.OrderMotionInvoiceNumber);
            
            outline.AddElement("Promotion", () => order.Value.OrderMotionPromotion);
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
            OrderMotionStoreEntity store = (OrderMotionStoreEntity) StoreManager.GetRelatedStore(orderID);

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
                    OrderMotionOnlineUpdater updater = new OrderMotionOnlineUpdater(store);
                    updater.UploadShipmentDetails(shipment);
                }
                catch (OrderMotionException ex)
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
