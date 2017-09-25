using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Magento
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Magento)]
    public class MagentoOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly IMagentoOnlineUpdaterFactory updaterFactory;
        private readonly IUserInteraction userInteraction;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOnlineUpdateCommandCreator(
            IMessageHelper messageHelper,
            IMagentoOnlineUpdaterFactory updaterFactory,
            IUserInteraction userInteraction,
            Func<Type, ILog> createLogger)
        {
            this.userInteraction = userInteraction;
            this.updaterFactory = updaterFactory;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create the menu commands for updating status
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            MagentoStoreEntity magentoStore = store as MagentoStoreEntity;
            List<IMenuCommand> commands = new List<IMenuCommand>();

            // take actions to Cancel the order
            AsyncMenuCommand command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Cancel),
                context => OnOrderCommand(context, magentoStore))
            { Tag = MagentoUploadCommand.Cancel };
            commands.Add(command);

            // try to complete the shipment - which creates an invoice (online), uploads shipping details if they exist, and
            // sets the order "state" online to complete
            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Complete),
                context => OnOrderCommand(context, magentoStore))
            { Tag = MagentoUploadCommand.Complete };
            commands.Add(command);

            // place the order into Hold status
            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Hold),
                context => OnOrderCommand(context, magentoStore))
            { Tag = MagentoUploadCommand.Hold };
            commands.Add(command);

            if (magentoStore.MagentoVersion == (int) MagentoVersion.MagentoTwoREST)
            {
                // take the order out of Hold status
                command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Unhold),
                    context => OnOrderCommand(context, magentoStore))
                { Tag = MagentoUploadCommand.Unhold };
                commands.Add(command);
            }

            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Comments),
                context => OnOrderCommand(context, magentoStore))
            { Tag = MagentoUploadCommand.Comments, BreakBefore = true };
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for executing order commands
        /// </summary>
        public async Task OnOrderCommand(IMenuCommandExecutionContext context, MagentoStoreEntity magentoStore)
        {
            MagentoUploadCommand action = (MagentoUploadCommand) context.MenuCommand.Tag;
            string comments = string.Empty;

            if (action == MagentoUploadCommand.Comments)
            {
                var actionResults = userInteraction.GetActionComments(context.Owner, (MagentoVersion) magentoStore.MagentoVersion);
                if (actionResults.Failure)
                {
                    context.Complete();
                    return;
                }

                action = actionResults.Value.Action;
                comments = actionResults.Value.Comments;
            }

            var results = await context.SelectedKeys.SelectWithProgress(messageHelper,
                    "Online Order Action",
                    "ShipWorks is executing an action on the order.",
                    "Updating order {0} of {1}...",
                    x => Task.Run(() => ExecuteOrderCommandCallback(x, action, comments, magentoStore)))
                .ConfigureAwait(true);
            var resultsList = results.ToList();

            IEnumerable<Exception> exceptions = resultsList.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// The worker thread function for executing commands on Magento orders online
        /// </summary>
        private async Task<IResult> ExecuteOrderCommandCallback(long orderID, MagentoUploadCommand action,
            string comments, MagentoStoreEntity magentoStore)
        {
            // create the updater and execute the command
            IMagentoOnlineUpdater updater = updaterFactory.Create(magentoStore);

            try
            {
                // lookup the store to get the email sending preference
                if (magentoStore != null)
                {
                    await updater.UploadShipmentDetails(orderID, action, comments, magentoStore.MagentoTrackingEmails).ConfigureAwait(false);
                }
                else
                {
                    log.WarnFormat("Cannot execute online command for Magento order id {0}, the store was deleted.", orderID);
                }

                return Result.FromSuccess();
            }
            catch (Exception ex) when (ex is MagentoException || ex is GenericStoreException)
            {
                return Result.FromError(ex);
            }
        }
    }
}
