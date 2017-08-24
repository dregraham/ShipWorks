using System;
using System.Collections.Generic;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    /// <summary>
    /// Creates the menu commands for spark pay stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SparkPay)]
    public class SparkPayOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;
        private readonly Func<SparkPayStoreEntity, SparkPayOnlineUpdater> onlineUpdaterFactory;
        private StatusCodeProvider<int> statusCodeProvider;
        private SparkPayOnlineUpdater onlineUpdater;

        public SparkPayOnlineUpdateCommandCreator(
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory,
            Func<SparkPayStoreEntity, SparkPayOnlineUpdater> onlineUpdaterFactory,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger
            )
        {
            this.statusCodeProviderFactory = statusCodeProviderFactory;
            this.onlineUpdaterFactory = onlineUpdaterFactory;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            SparkPayStoreEntity typedStore = (SparkPayStoreEntity) store;

            statusCodeProvider = statusCodeProviderFactory(typedStore);

            List<IMenuCommand> commands = statusCodeProvider.CodeNames
                .Select(x =>
                    new AsyncMenuCommand(x, context => OnSetOnlineStatus(context, typedStore))
                    {
                        Tag = statusCodeProvider.GetCodeValue(x)
                    })
                .OfType<IMenuCommand>()
                .ToList();

            IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, typedStore));
            uploadCommand.BreakBefore = commands.Any();
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        private async Task OnUploadDetails(MenuCommandExecutionContext context, SparkPayStoreEntity store)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Upload Shipment Details", 
                    "ShipWorks is uploading shipment information.", 
                    "Updating order {0} of {1}...",
                    orderID => UploadShipmentDetailsCallback(orderID, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, SparkPayStoreEntity store)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                return Result.FromSuccess();
            }

            try
            {
                onlineUpdater = onlineUpdaterFactory(store);
                await onlineUpdater.UpdateShipmentDetails(shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private async Task OnSetOnlineStatus(MenuCommandExecutionContext context, SparkPayStoreEntity store)
        {
            int statusCode = (int) context.MenuCommand.Tag;

            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Set Status", "ShipWorks is setting the online status.", "Updating order {0} of {1}...",
                    orderID => SetOnlineStatusCallback(orderID, statusCode, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private async Task<IResult> SetOnlineStatusCallback(long orderID, int statusCode, SparkPayStoreEntity store)
        {
            try
            {
                onlineUpdater = onlineUpdaterFactory(store);
                await onlineUpdater.UpdateOrderStatus(orderID, statusCode).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (SparkPayException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
