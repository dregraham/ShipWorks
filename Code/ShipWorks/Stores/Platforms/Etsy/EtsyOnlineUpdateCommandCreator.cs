using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Etsy.Dialog;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Create online update commands for Etsy
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Etsy)]
    public class EtsyOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        public EtsyOnlineUpdateCommandCreator(
            IMessageHelper messageHelper, 
            Func<Type, ILog> createLogger)
        {
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() => 
            Enumerable.Empty<IMenuCommand>();

        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            EtsyStoreEntity typedStore = (EtsyStoreEntity) store;

            IMenuCommand uploadShipped = new AsyncMenuCommand("Marked as Shipped", context => OnUploadTrackingDetailsShippedStatus(context, typedStore));
            IMenuCommand uploadNotShipped = new AsyncMenuCommand("Marked as Not Shipped", context => OnUploadTrackingDetailsNotShippedStatus(context, typedStore));
            IMenuCommand uploadPaid = new AsyncMenuCommand("Mark as Paid", context => OnUploadPaidStatus(context, typedStore));
            IMenuCommand uploadUnpaid = new AsyncMenuCommand("Mark as Not Paid", context => OnUploadNotPaidStatus(context, typedStore));
            IMenuCommand uploadTrackingInformation = new AsyncMenuCommand("Upload Tracking Information", context => OnUploadTrackingDetails(context, typedStore));

            return new []{uploadShipped, uploadNotShipped, uploadPaid, uploadUnpaid, uploadTrackingInformation};
        }

        /// <summary>
        /// Command handler for uploading shipment details with "shipped" status
        /// </summary>
        private async Task OnUploadTrackingDetailsShippedStatus(MenuCommandExecutionContext context, EtsyStoreEntity store)
        {
            using (GetCommentTokenDlg getToken = new GetCommentTokenDlg())
            {
                if (getToken.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    string comment = getToken.Comment;

                    var results = await context.SelectedKeys
                        .SelectWithProgress(messageHelper, "Upload Shipment Tracking", "ShipWorks is uploading shipment information.", "Updating order {0} of {1}...",
                            orderID => UploadShipmentDetailsCallback(orderID, comment, store))
                        .ConfigureAwait(true);

                    var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
                    context.Complete(exceptions, MenuCommandResult.Error);
                }
            }
        }

        /// <summary>
        /// Command handler for uploading shipment details with "not shipped" status
        /// </summary>
        private async Task OnUploadTrackingDetailsNotShippedStatus(MenuCommandExecutionContext context, EtsyStoreEntity store)
        {
            using (GetCommentTokenDlg getToken = new GetCommentTokenDlg())
            {
                if (getToken.ShowDialog(context.Owner) == DialogResult.OK)
                {
                    string comment = getToken.Comment;

                    var results = await context.SelectedKeys
                        .SelectWithProgress(messageHelper, "Upload Shipment Tracking", "ShipWorks is uploading shipment information.", "Updating order {0} of {1}...",
                            orderID => UpdateOrderAsNotShippedCallback(orderID, comment, store))
                        .ConfigureAwait(true);

                    var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
                    context.Complete(exceptions, MenuCommandResult.Error);
                }
            }
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, string comment, EtsyStoreEntity store)
        {
            try
            {
                EtsyOnlineUpdater updater = new EtsyOnlineUpdater(store);

                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

                if (shipment == null)
                {
                    string msg = string.Format("Not uploading tracking number since no shipment associated with order {0}", orderID.ToString());
                    log.WarnFormat(msg);
                    return Result.FromError(msg);
                }

                await updater.UpdateOnlineStatus(shipment, null, true, comment).ConfigureAwait(false);

                return Result.FromSuccess();
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error upload shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Worker thread method for uploading false shipment details
        /// </summary>
        private async Task<IResult> UpdateOrderAsNotShippedCallback(long orderID, string comment, EtsyStoreEntity store)
        {
            try
            {
                EtsyOnlineUpdater updater = new EtsyOnlineUpdater(store);
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

                if (shipment == null)
                {
                    string msg = string.Format("Not uploading tracking number since no shipment associated with order {0}", orderID.ToString());
                    log.WarnFormat(msg);
                    return Result.FromError(msg);
                }

                await updater.UpdateOnlineStatus(shipment, null, false, comment).ConfigureAwait(false);

                return Result.FromSuccess();
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error upload shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Uploads Paid Status
        /// </summary>
        private async Task OnUploadPaidStatus(MenuCommandExecutionContext context, EtsyStoreEntity store)
        {
            await UploadPaymentDetails(context, true, store).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads Not Paid Status.
        /// </summary>
        private async Task OnUploadNotPaidStatus(MenuCommandExecutionContext context, EtsyStoreEntity store)
        {
            await UploadPaymentDetails(context, false, store).ConfigureAwait(false);
        }

        private async Task UploadPaymentDetails(MenuCommandExecutionContext context, bool wasPaid, EtsyStoreEntity store)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Upload Shipment Payment Status", "ShipWorks is uploading payment information.", "Updating order {0} of {1}...",
                    orderID => PaymentUploadCallback(orderID, wasPaid, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading payment details
        /// </summary>
        public async Task<IResult> PaymentUploadCallback(long orderID, bool paidStatus, EtsyStoreEntity store)
        {
            try
            {
                EtsyOrderEntity order = (EtsyOrderEntity) DataProvider.GetEntity(orderID);
                EtsyOnlineUpdater etsyUpdater = new EtsyOnlineUpdater(store);

                bool wasPaid = paidStatus;
                
                await etsyUpdater.UpdateOnlineStatus(order, wasPaid, null).ConfigureAwait(false);
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error uploading payment information for orders {0}", ex.Message);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Called to upload tracking information to Etsy
        /// </summary>
        private async Task OnUploadTrackingDetails(MenuCommandExecutionContext context, EtsyStoreEntity store)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Upload Shipment Tracking", "ShipWorks is uploading shipment information.", "Updating order {0} of {1}...",
                    orderID => TrackingUploadCallback(orderID, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Tracking the upload callback.
        /// </summary>
        private async Task<IResult> TrackingUploadCallback(long orderID, EtsyStoreEntity store)
        {
            try
            {
                EtsyOnlineUpdater etsyUpdater = new EtsyOnlineUpdater(store);

                await etsyUpdater.UploadShipmentDetails(orderID).ConfigureAwait(false);
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error uploading tracking information for orders {0}", ex.Message);
            }

            return Result.FromSuccess();
        }
    }
}