using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using System;

namespace ShipWorks.Stores.Platforms.LemonStand.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for LemonStand
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.LemonStand)]
    public class LemonStandCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LemonStandCommandCreator));
        private readonly IMessageHelper messageHelper;
        private readonly Func<LemonStandStoreEntity, LemonStandOnlineUpdater> onlineUpdaterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandCommandCreator(IMessageHelper messageHelper, Func<LemonStandStoreEntity, LemonStandOnlineUpdater> onlineUpdaterFactory)
        {
            this.messageHelper = messageHelper;
            this.onlineUpdaterFactory = onlineUpdaterFactory;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create online update commands
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            LemonStandStoreEntity typedStore = (LemonStandStoreEntity) store;

            // get possible status codes from the provider
            LemonStandStatusCodeProvider codeProvider = new LemonStandStatusCodeProvider(typedStore);

            List<IMenuCommand> commands = codeProvider.CodeNames
                .Select(x =>
                    new AsyncMenuCommand(x, context => OnSetOnlineStatus(context, typedStore))
                    {
                        Tag = codeProvider.GetCodeValue(x)
                    })
                .OfType<IMenuCommand>()
                .ToList();

            IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, typedStore));
            uploadCommand.BreakBefore = commands.Any();
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(IMenuCommandExecutionContext context, LemonStandStoreEntity store)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Upload Shipment Details", "ShipWorks is uploading shipment information.", "Updating order {0} of {1}...",
                    orderID => UploadShipmentDetailsCallback(orderID, store))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, LemonStandStoreEntity store)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                LemonStandOnlineUpdater updater = onlineUpdaterFactory(store);

                await updater.UpdateShipmentDetails(new []{ orderID }).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (LemonStandException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        public async Task OnSetOnlineStatus(IMenuCommandExecutionContext context, LemonStandStoreEntity store)
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
        private async Task<IResult> SetOnlineStatusCallback(long orderID, int statusCode, LemonStandStoreEntity store)
        {
            try
            {
                LemonStandOnlineUpdater updater = onlineUpdaterFactory(store);
                await updater.UpdateOrderStatus(orderID, statusCode).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (LemonStandException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
