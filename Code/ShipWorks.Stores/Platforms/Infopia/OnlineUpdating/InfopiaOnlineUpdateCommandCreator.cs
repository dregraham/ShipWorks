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
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Infopia.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Buy.com stores
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Infopia)]
    public class InfopiaOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;
        private readonly IInfopiaOnlineUpdater onlineUpdater;
        private readonly IOrderManager orderUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOnlineUpdateCommandCreator(IInfopiaOnlineUpdater onlineUpdater,
            IMessageHelper messageHelper,
            IOrderManager orderUtility,
            Func<Type, ILog> createLogger)
        {
            this.orderUtility = orderUtility;
            this.onlineUpdater = onlineUpdater;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Online Update commands for Infopia
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            IInfopiaStoreEntity typedStore = store as IInfopiaStoreEntity;
            if (typedStore == null)
            {
                log.Warn("Cannot create instance command for non-Infopia store");
                return Enumerable.Empty<IMenuCommand>();
            }

            return InfopiaUtility.StatusValues
                .Select(x => new AsyncMenuCommand(x, context => OnSetOnlineStatus(typedStore, context)) { Tag = x })
                .Append(new AsyncMenuCommand("Upload shipment details", context => OnUploadShipmentDetails(typedStore, context))
                {
                    BreakBefore = true
                });
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private async Task OnUploadShipmentDetails(IInfopiaStoreEntity store, MenuCommandExecutionContext context)
        {
            List<long> orderKeys = context.SelectedKeys.ToList();
            List<IResult> results = new List<IResult>(orderKeys.Count);

            using (var dialog = messageHelper.ShowProgressDialog("Upload Shipment Details", "ShipWorks is uploading shipment information."))
            {
                var progressUpdater = dialog.ToUpdater(orderKeys, "Updating order {0} of {1}...");

                foreach (long orderID in orderKeys)
                {
                    var result = await ShipmentUploadCallback(store, orderID).ConfigureAwait(true);
                    results.Add(result);
                    progressUpdater.Update();
                }
            }

            context.Complete(results.Where(x => x.Failure).Select(x => x.Exception), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> ShipmentUploadCallback(IInfopiaStoreEntity store, long orderID)
        {
            ShipmentEntity shipment = await orderUtility.GetLatestActiveShipmentAsync(orderID).ConfigureAwait(false);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Result.FromSuccess();
            }

            try
            {
                await onlineUpdater.UploadShipmentDetails(store, shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (InfopiaException ex)
            {
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Set the online status for the selected orders
        /// </summary>
        private async Task OnSetOnlineStatus(IInfopiaStoreEntity store, MenuCommandExecutionContext context)
        {
            string status = context.MenuCommand.Tag as string;
            List<long> orderKeys = context.SelectedKeys.ToList();
            List<IResult> results = new List<IResult>(orderKeys.Count);

            using (var dialog = messageHelper.ShowProgressDialog("Set Status", "ShipWorks is setting the online status."))
            {
                var progressUpdater = dialog.ToUpdater(orderKeys, "Updating order {0} of {1}...");

                foreach (long orderID in orderKeys)
                {
                    var result = await SetOnlineStatusCallback(store, orderID, status).ConfigureAwait(true);
                    results.Add(result);
                    progressUpdater.Update();
                }
            }

            context.Complete(results.Where(x => x.Failure).Select(x => x.Exception), MenuCommandResult.Error);
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private async Task<IResult> SetOnlineStatusCallback(IInfopiaStoreEntity store, long orderID, string status)
        {
            try
            {
                await onlineUpdater.UpdateOrderStatus(store, orderID, status).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (InfopiaException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
