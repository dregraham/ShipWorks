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
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore;
using Autofac;
using Autofac.Features.OwnedInstances;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Create online update commands for Etsy
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Magento)]
    public class MagentoOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOnlineUpdateCommandCreator(
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

        /// <summary>
        /// Create the menu commands for updating status
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            MagentoStoreEntity magentoStore = store as MagentoStoreEntity;
            List<IMenuCommand> commands = new List<IMenuCommand>();

            // take actions to Cancel the order
            AsyncMenuCommand command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Cancel), 
                context => OnOrderCommand(context, magentoStore)) { Tag = MagentoUploadCommand.Cancel };
            commands.Add(command);

            // try to complete the shipment - which creates an invoice (online), uploads shipping details if they exist, and
            // sets the order "state" online to complete
            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Complete), 
                context => OnOrderCommand(context, magentoStore)) { Tag = MagentoUploadCommand.Complete };
            commands.Add(command);

            // place the order into Hold status
            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Hold), 
                context => OnOrderCommand(context, magentoStore)) { Tag = MagentoUploadCommand.Hold };
            commands.Add(command);

            if (magentoStore.MagentoVersion == (int) MagentoVersion.MagentoTwoREST)
            {
                // take the order out of Hold status
                command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Unhold), 
                    context => OnOrderCommand(context, magentoStore)) { Tag = MagentoUploadCommand.Unhold };
                commands.Add(command);
            }

            command = new AsyncMenuCommand(EnumHelper.GetDescription(MagentoUploadCommand.Comments), 
                context => OnOrderCommand(context, magentoStore)) { Tag = MagentoUploadCommand.Comments, BreakBefore = true };
            commands.Add(command);

            return commands;
        }

        /// <summary>
        /// MenuCommand handler for executing order commands
        /// </summary>
        private async Task OnOrderCommand(MenuCommandExecutionContext context, MagentoStoreEntity magentoStore)
        {
            IMenuCommand command = context.MenuCommand;
            MagentoUploadCommand action;
            string comments = "";
            if ((MagentoUploadCommand) command.Tag == MagentoUploadCommand.Comments)
            {
                // open a window for the user to select an action and comments
                using (MagentoActionCommentsDlg dlg = new MagentoActionCommentsDlg((MagentoVersion) magentoStore.MagentoVersion))
                {
                    if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                    {
                        action = dlg.Action;
                        comments = dlg.Comments;
                    }
                    else
                    {
                        // cancel now
                        context.Complete();
                        return;
                    }
                }
            }
            else
            {
                action = (MagentoUploadCommand) command.Tag;
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
            IMagentoOnlineUpdater updater = (IMagentoOnlineUpdater) CreateOnlineUpdater(magentoStore);

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

        /// <summary>
        /// Create a magento online updater
        /// </summary>
        private GenericStoreOnlineUpdater CreateOnlineUpdater(MagentoStoreEntity magentoStore)
        {
            if (magentoStore.MagentoVersion == (int) MagentoVersion.MagentoTwoREST)
            {
                return (GenericStoreOnlineUpdater)
                    IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<Owned<IMagentoOnlineUpdater>>(
                        MagentoVersion.MagentoTwoREST,
                        new TypedParameter(typeof(GenericModuleStoreEntity), magentoStore)).Value;
            }

            return new MagentoOnlineUpdater((GenericModuleStoreEntity) magentoStore);
        }
    }
}
