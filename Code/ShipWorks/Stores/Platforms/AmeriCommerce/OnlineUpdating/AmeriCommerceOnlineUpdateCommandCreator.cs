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
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for AmeriCommerce
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.AmeriCommerce)]
    public class AmeriCommerceOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceOnlineUpdateCommandCreator));
        private readonly IMessageHelper messageHelper;
        private readonly IAmeriCommerceOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOnlineUpdateCommandCreator(IMessageHelper messageHelper, IAmeriCommerceOnlineUpdater onlineUpdater)
        {
            this.messageHelper = messageHelper;
            this.onlineUpdater = onlineUpdater;
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
            AmeriCommerceStoreEntity typedStore = (AmeriCommerceStoreEntity) store;

            // get possible status codes from the provider
            AmeriCommerceStatusCodeProvider codeProvider = new AmeriCommerceStatusCodeProvider(typedStore);

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
        public async Task OnUploadDetails(IMenuCommandExecutionContext context, AmeriCommerceStoreEntity store)
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
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, AmeriCommerceStoreEntity store)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                await onlineUpdater.UploadOrderShipmentDetails(store, new[] { orderID }).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (AmeriCommerceException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        public async Task OnSetOnlineStatus(IMenuCommandExecutionContext context, AmeriCommerceStoreEntity store)
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
        private async Task<IResult> SetOnlineStatusCallback(long orderID, int statusCode, AmeriCommerceStoreEntity store)
        {
            try
            {
                await onlineUpdater.UpdateOrderStatus(store, orderID, statusCode).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (AmeriCommerceException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
