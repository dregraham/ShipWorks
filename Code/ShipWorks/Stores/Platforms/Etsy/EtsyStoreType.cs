using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Etsy.Dialog;
using ShipWorks.Stores.Platforms.Etsy.WizardPages;
using ShipWorks.UI.Wizard;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Etsy.Enums;
using System.Linq;
using ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Etsy Store specific integration into ShipWorks
    /// </summary>
    public class EtsyStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyStoreType));

        /// <summary>
        /// Static Constructor - initializes the OnlineStatusAlgorithm available in the Entity model.
        /// </summary>
        static EtsyStoreType()
        {
            EtsyOrderEntity.SetEffectiveOnlineStatusAlgorithm(e => EtsyOrderStatusUtility.UpdateEntityOrderStatus(e));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is EtsyStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of EtsyStoreEntity.");
            }
        }

        /// <summary>
        /// Etsy Store entity
        /// </summary>
        public EtsyStoreEntity EtsyStore
        {
            get
            {
                return Store as EtsyStoreEntity;
            }
        }

        /// <summary>
        /// Creates a new instance of an Etsy store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            EtsyStoreEntity etsyStore = new EtsyStoreEntity();

            InitializeStoreDefaults(etsyStore);

            return etsyStore;
        }

        /// <summary>
        /// Get the identifier object that is used to uniquely identify the specified order for the store.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "orders required");
            }

            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the fields required to uniquely identify the online customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = false;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Creates a new Etsy Order
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity CreateOrderInstance()
        {
            EtsyOrderEntity order = new EtsyOrderEntity();

            return order;
        }

        /// <summary>
        /// Create a downloader for our current store instance
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new EtsyDownloader(this, (EtsyStoreEntity)Store);
        }

        /// <summary>
        /// Create the pages to be displayed in the Add Store Wizard
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage>();

            pages.Add(new WizardPages.EtsyAssociateAccountPage());

            return pages;
        }

        /// <summary>
        /// Create the control used to configurd the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(EtsyShipmentUploadTask));
        }

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            var control = new EtsyAccountSettingsControl();

            return control;
        }

        /// <summary>
        /// Get the available online status choices
        /// </summary>
        /// <returns></returns>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            return EnumHelper.GetEnumList<EtsyOrderStatus>().Select(status => status.Description).ToList();
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommand uploadShipped = new MenuCommand("Mark as Shipped", new MenuCommandExecutor(OnUploadShipmentDetails));
            MenuCommand uploadNotShipped = new MenuCommand("Mark as Not Shipped", new MenuCommandExecutor(OnSetShipmentStatusToFalse));

            MenuCommand uploadPaid = new MenuCommand("Mark as Paid", new MenuCommandExecutor(OnUploadPaidStatus)) { BreakBefore = true };
            MenuCommand uploadUnpaid = new MenuCommand("Mark as Not Paid", new MenuCommandExecutor(OnUploadNotPaidStatus));

            MenuCommand uploadTrackingInformation = new MenuCommand("Upload Tracking Information", OnUploadTrackingDetails);

            commands.Add(uploadTrackingInformation);
            commands.Add(uploadShipped);
            commands.Add(uploadNotShipped);
            commands.Add(uploadPaid);
            commands.Add(uploadUnpaid);

            return commands;
        }

        /// <summary>
        /// Called to upload tracking information to Etsy
        /// </summary>
        private void OnUploadTrackingDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Upload Shipment Tracking",
               "ShipWorks is uploading shipment information.",
               "Updating order {0} of {1}");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(TrackingUploadCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Uploads Paid Status
        /// </summary>
        private void OnUploadPaidStatus(MenuCommandExecutionContext context)
        {
            UploadPaymentStatus(context, true);
        }

        /// <summary>
        /// Uploads Not Paid Status.
        /// </summary>
        /// <param name="context"></param>
        private void OnUploadNotPaidStatus(MenuCommandExecutionContext context)
        {
            UploadPaymentStatus(context, false);
        }

        /// <summary>
        /// Sets payment status to True or False
        /// </summary>
        private void UploadPaymentStatus(MenuCommandExecutionContext context, bool wasPaid)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Payment Status",
                "ShipWorks is uploading payment information.",
                "Updating order {0} of {1}");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(PaymentUploadCallback, context.SelectedKeys, wasPaid);
        }

        /// <summary>
        /// Sets shipped status to False
        /// </summary>
        private void OnSetShipmentStatusToFalse(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Status",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UpdateOrdersAsNotShippedCallback, context.SelectedKeys);
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            // kick off the execution
            using (GetCommentTokenDlg getToken = new GetCommentTokenDlg())
            {
                if (getToken.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    string comment = getToken.Comment;

                    executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, comment);
                }
            }
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                string comment = (string)userState;

                EtsyOnlineUpdater onlineUpdater = new EtsyOnlineUpdater((EtsyStoreEntity)Store);

                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

                if (shipment == null)
                {
                    string msg = string.Format("Not uploading tracking number since no shipment associated with order {0}", orderID.ToString());
                    log.WarnFormat(msg);
                    return;
                }

                onlineUpdater.UpdateOnlineStatus(shipment, null, true, comment);
            }
            catch (EtsyException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Worker thread method for marking an order as not shipped
        /// </summary>
        private void UpdateOrdersAsNotShippedCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                EtsyOrderEntity order = (EtsyOrderEntity)DataProvider.GetEntity(orderID);
                EtsyOnlineUpdater etsyUpdater = new EtsyOnlineUpdater((EtsyStoreEntity)Store);

                etsyUpdater.UpdateOnlineStatus(order, null, false);
            }
            catch (EtsyException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Worker thread method for uploading payment details
        /// </summary>
        private void PaymentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                EtsyOrderEntity order = (EtsyOrderEntity)DataProvider.GetEntity(orderID);

                bool wasPaid = (bool)userState;

                EtsyOnlineUpdater etsyUpdater = new EtsyOnlineUpdater((EtsyStoreEntity)Store);

                etsyUpdater.UpdateOnlineStatus(order, wasPaid, null);
            }
            catch (EtsyException ex)
            {
                // log it
                log.ErrorFormat("Error uploading payment information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Trackings the upload callback.
        /// </summary>
        private void TrackingUploadCallback(long orderID, object userstate, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                EtsyOnlineUpdater etsyUpdater = new EtsyOnlineUpdater((EtsyStoreEntity)Store);

                etsyUpdater.UploadShipmentDetails(orderID);
            }
            catch (EtsyException ex)
            {
                // log it
                log.ErrorFormat("Error uploading tracking information for orders {0}", ex.Message);

                // add the error to issues for the user
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Indicates what basic grid fields we support hyperlinking for
        /// </summary>
        public override bool GridHyperlinkSupported(EntityField2 field)
        {
            return
                EntityUtility.IsSameField(field, OrderItemFields.Code) ||
                EntityUtility.IsSameField(field, OrderItemFields.Name);
        }

        /// <summary>
        /// Handle a link click for the given field
        /// </summary>
        public override void GridHyperlinkClick(EntityField2 field, EntityBase2 entity, IWin32Window owner)
        {
            OrderItemEntity item = entity as OrderItemEntity;
            if (entity == null || owner == null || item == null)
            {
                MessageHelper.ShowError(owner, "Unable to open link");
            }

            if (item != null)
            {
                Uri uri = EtsyEndpoints.GetItemUrl(item.Code);
                WebHelper.OpenUrl(uri, owner);
            }
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Etsy;
            }
        }

        /// <summary>
        /// Etsy has Online Status
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
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                return EtsyStore.EtsyShopID.ToString();
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
    }
}