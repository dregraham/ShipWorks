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
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.NetworkSolutions)]
    public class NetworkSolutionsOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly ILog log;
        readonly IOrderStatusUpdater orderStatusUpdater;
        readonly IShipmentDetailsUpdater shipmentDetailsUpdater;
        readonly INetworkSolutionsUserInteraction userInteraction;
        readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOnlineUpdateCommandCreator(
            INetworkSolutionsUserInteraction userInteraction,
            IOrderStatusUpdater orderStatusUpdater,
            IShipmentDetailsUpdater shipmentDetailsUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.messageHelper = messageHelper;
            this.userInteraction = userInteraction;
            this.shipmentDetailsUpdater = shipmentDetailsUpdater;
            this.orderStatusUpdater = orderStatusUpdater;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create the menu commands for updating order status
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            var typedStore = (NetworkSolutionsStoreEntity) store;

            // get possible status codes from the provider
            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) store);

            List<IMenuCommand> commands = new List<IMenuCommand>();

            // create a menu item for each status
            foreach (long codeValue in codeProvider.CodeValues)
            {
                IMenuCommand command = new AsyncMenuCommand(codeProvider.GetCodeName(codeValue), context => OnSetOnlineStatus(typedStore, context));
                command.Tag = codeValue;

                commands.Add(command);
            }

            IMenuCommand withComments = new AsyncMenuCommand("Set with comments...", context => OnSetOnlineStatus(typedStore, context));
            withComments.Tag = -1L;
            commands.Add(withComments);

            IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(typedStore, context));
            uploadCommand.BreakBefore = true;
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadShipmentDetails(NetworkSolutionsStoreEntity store, IMenuCommandExecutionContext context)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    x => UploadShipmentDetails(store, x))
                .ConfigureAwait(true);

            context.Complete(results.Select(x => x.Exception).Where(x => x != null), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetails(NetworkSolutionsStoreEntity store, long orderID)
        {
            try
            {
                await shipmentDetailsUpdater.UploadShipmentDetailsForOrder(store, orderID).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (NetworkSolutionsException ex)
            {
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        public async Task OnSetOnlineStatus(NetworkSolutionsStoreEntity store, IMenuCommandExecutionContext context)
        {
            IMenuCommand command = context.MenuCommand;
            long statusCode = (long) command.Tag;
            string comments = "";

            // -1 status indicates this is the Set with Comments..
            if (statusCode == -1)
            {
                var details = userInteraction.GetMessagingDetails(context.Owner, store);
                if (details.Success)
                {
                    statusCode = details.Value.Code;
                    comments = details.Value.Comments;
                }
            }

            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Set Status",
                    "ShipWorks is setting the online status.",
                    "Updating order {0} of {1}...",
                    x => SetOnlineStatusCallback(store, x, statusCode, comments))
                .ConfigureAwait(true);

            context.Complete(results.Select(x => x.Exception).Where(x => x != null), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private async Task<IResult> SetOnlineStatusCallback(NetworkSolutionsStoreEntity store, long orderID, long statusCode, string comments)
        {
            try
            {
                await orderStatusUpdater.UpdateOrderStatus(store, orderID, statusCode, comments).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (NetworkSolutionsException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
