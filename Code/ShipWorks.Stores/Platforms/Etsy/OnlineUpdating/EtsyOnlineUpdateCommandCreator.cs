using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Etsy
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Etsy)]
    public class EtsyOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly Func<EtsyStoreEntity, IEtsyOnlineUpdater> createOnlineUpdater;
        private readonly IEtsyUserInteraction userInteraction;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        public EtsyOnlineUpdateCommandCreator(
            Func<EtsyStoreEntity, IEtsyOnlineUpdater> createOnlineUpdater,
            IEtsyUserInteraction userInteraction,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.createOnlineUpdater = createOnlineUpdater;
            this.userInteraction = userInteraction;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            EtsyStoreEntity typedStore = (EtsyStoreEntity) store;

            return new[]
            {
                new AsyncMenuCommand("Mark as Shipped", context => OnUploadShippedStatus(context, typedStore, true)),
                new AsyncMenuCommand("Mark as Not Shipped", context => OnUploadShippedStatus(context, typedStore, false)),
                new AsyncMenuCommand("Mark as Paid", context => OnUploadPaidStatus(context, typedStore, true)),
                new AsyncMenuCommand("Mark as Not Paid", context => OnUploadPaidStatus(context, typedStore, false)),
                new AsyncMenuCommand("Upload Tracking Information", context => OnUploadTrackingDetails(context, typedStore))
            };
        }

        /// <summary>
        /// Command handler for uploading shipment details with "shipped" status
        /// </summary>
        public async Task OnUploadShippedStatus(IMenuCommandExecutionContext context, EtsyStoreEntity store, bool shipped)
        {
            var comment = userInteraction.GetComment(context.Owner);
            if (comment.Failure)
            {
                context.Complete();
                return;
            }

            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}",
                    orderID => UploadShippedStatusCallback(orderID, comment.Value, store, shipped))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShippedStatusCallback(long orderID, string comment, EtsyStoreEntity store, bool shipped)
        {
            try
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

                if (shipment == null)
                {
                    string msg = "Not uploading shipment information since no shipment associated with order " + orderID;
                    log.Warn(msg);
                    return Result.FromError(msg);
                }

                await createOnlineUpdater(store).UpdateOnlineStatus(shipment, null, shipped, comment).ConfigureAwait(false);

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
        public async Task OnUploadPaidStatus(IMenuCommandExecutionContext context, EtsyStoreEntity store, bool paid)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Payment Status",
                    "ShipWorks is uploading payment information.",
                    "Updating order {0} of {1}...",
                    orderID => PaymentUploadCallback(orderID, paid, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading payment details
        /// </summary>
        private async Task<IResult> PaymentUploadCallback(long orderID, bool paidStatus, EtsyStoreEntity store)
        {
            try
            {
                EtsyOrderEntity order = (EtsyOrderEntity) DataProvider.GetEntity(orderID);

                await createOnlineUpdater(store).UpdateOnlineStatus(order, paidStatus, null).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error uploading payment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Called to upload tracking information to Etsy
        /// </summary>
        public async Task OnUploadTrackingDetails(IMenuCommandExecutionContext context, EtsyStoreEntity store)
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
                await createOnlineUpdater(store).UploadShipmentDetails(orderID).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (EtsyException ex)
            {
                log.ErrorFormat("Error uploading tracking information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}