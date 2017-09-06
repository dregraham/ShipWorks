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
using ShipWorks.Stores.Platforms.GenericModule;
using Autofac;
using ShipWorks.ApplicationCore;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.GenericModule.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.GenericModule)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Amosoft)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Brightpearl)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Cart66Lite)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Cart66Pro)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ChannelSale)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Choxi)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ClickCartPro)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.CloudConversion)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.CreLoaded)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.CsCart)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Fortune3)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.GeekSeller)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.InfiPlex)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.InstaStore)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Jigoshop)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.LimeLightCRM)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.LiveSite)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.LoadedCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Miva)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.nopCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OpenCart)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OpenSky)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OrderBot)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OrderDesk)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OrderDynamics)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.osCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.PowersportsSupport)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.PrestaShop)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.RevolutionParts)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SearchFit)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SellerActive)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SellerCloud)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SellerExpress)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SellerVantage)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Shopp)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Shopperpress)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SolidCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.StageBloc)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.SureDone)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.VirtueMart)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.WebShopManager)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.WooCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.WPeCommerce)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.XCart)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ZenCart)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Zenventory)]

    public class GenericModuleOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private readonly IStoreTypeManager storeTypeManager;
        readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleOnlineUpdateCommandCreator(IOrderManager orderManager,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger,
            IStoreTypeManager storeTypeManager)
        {
            this.orderManager = orderManager;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
            this.storeTypeManager = storeTypeManager;
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
            GenericModuleStoreEntity typedStore = (GenericModuleStoreEntity) store;
            GenericOnlineStatusSupport statusSupport = (GenericOnlineStatusSupport) typedStore.ModuleOnlineStatusSupport;
            List<IMenuCommand> commands = new List<IMenuCommand>();

            // See if status is supported at all
            if (statusSupport != GenericOnlineStatusSupport.None &&
                statusSupport != GenericOnlineStatusSupport.DownloadOnly)
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) storeTypeManager.GetType(store);
                GenericStoreStatusCodeProvider statusCodeProvider = storeType.CreateStatusCodeProvider();

                // Add each status option
                foreach (object code in statusCodeProvider.CodeValues)
                {
                    IMenuCommand command = new AsyncMenuCommand(statusCodeProvider.GetCodeName(code), context => OnSetOnlineStatus(context, typedStore));
                    command.Tag = code;

                    commands.Add(command);
                }

                // Check if we can add the ability to manually set status with comment
                if (statusSupport == GenericOnlineStatusSupport.StatusWithComment)
                {
                    // Can only do custom if there are any in the first place
                    if (statusCodeProvider.CodeValues.Count > 0)
                    {
                        IMenuCommand withComment = new AsyncMenuCommand("Set with comment...", context => OnSetOnlineStatus(context, typedStore));
                        withComment.Tag = null;
                        withComment.BreakBefore = true;

                        commands.Add(withComment);
                    }
                }
            }

            // Check if we can add the ability to upload tracking number
            if (typedStore.ModuleOnlineShipmentDetails)
            {
                // Add the option to Upload shipment details
                IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(context, typedStore));
                uploadCommand.BreakBefore = commands.Any();
                commands.Add(uploadCommand);
            }

            return commands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        public async Task OnUploadShipmentDetails(IMenuCommandExecutionContext context, GenericModuleStoreEntity store)
        {
            var results = await UploadShipmentDetails(context.SelectedKeys, store).ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Set the online status of all the requested orders
        /// </summary>
        private async Task<IEnumerable<IResult>> UploadShipmentDetails(IEnumerable<long> keys, GenericModuleStoreEntity store)
        {
            return await keys
                .SelectWithProgress(messageHelper, "Upload Shipment Details", "ShipWorks is uploading the tracking number.", "Updating order {0} of {1}...",
                    orderID => UploadShipmentDetailsCallback(orderID, store))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, GenericModuleStoreEntity store)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(orderID).ConfigureAwait(false);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Result.FromSuccess();
            }

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) storeTypeManager.GetType(store);
                GenericStoreOnlineUpdater updater = storeType.CreateOnlineUpdater();
                await updater.UploadTrackingNumber(shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (GenericStoreException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Set the online status for the selected orders
        /// </summary>
        public async Task OnSetOnlineStatus(IMenuCommandExecutionContext context, GenericModuleStoreEntity store)
        {
            IMenuCommand command = context.MenuCommand;
            object code = null;
            string comment = "";

            if (command.Tag == null)
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) storeTypeManager.GetType(store);
                using (OnlineStatusCommentDlg dlg = new OnlineStatusCommentDlg(storeType.CreateStatusCodeProvider()))
                {
                    if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                    {
                        code = dlg.Code;
                        comment = dlg.Comments;
                    }
                    else
                    {
                        // Cancel now
                        context.Complete();
                        return;
                    }
                }
            }
            else
            {
                code = command.Tag;
            }

            var results = await SetOnlineStatus(context, code, comment, store).ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Set the online status of all the requested orders
        /// </summary>
        private async Task<IEnumerable<GenericResult<long>>> SetOnlineStatus(IMenuCommandExecutionContext context, object code, string comment, GenericModuleStoreEntity store)
        {
            return await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Set Status", "ShipWorks is setting the online status.", "Updating order {0} of {1}...",
                    key => SetOnlineStatusCallback(key, code, comment, store))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private async Task<GenericResult<long>> SetOnlineStatusCallback(long orderID, object code, string comment, GenericModuleStoreEntity store)
        {
            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) storeTypeManager.GetType(store);
                GenericStoreOnlineUpdater updater = storeType.CreateOnlineUpdater();
                await updater.UpdateOrderStatus(orderID, code, comment).ConfigureAwait(false);
                return GenericResult.FromSuccess(orderID);
            }
            catch (GenericStoreException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return GenericResult.FromError(ex, orderID);
            }
        }
    }
}
