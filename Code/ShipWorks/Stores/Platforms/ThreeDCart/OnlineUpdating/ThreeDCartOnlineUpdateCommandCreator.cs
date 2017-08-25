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
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartOnlineUpdateCommandCreator(IOrderManager orderManager,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.orderManager = orderManager;
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
            List<IMenuCommand> commands = new List<IMenuCommand>();
            ThreeDCartStoreEntity typedStore = (ThreeDCartStoreEntity) store;

            if (typedStore.RestUser)
            {
                commands = CreateOnlineUpdateStatusCommandsRest(typedStore).ToList();
            }
            else
            {
                commands = CreateOnlineUpdateStatusCommandsSoap(typedStore).ToList();
            }

            IMenuCommand uploadCommand = new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(context, typedStore));
            uploadCommand.BreakBefore = commands.Any();
            commands.Add(uploadCommand);

            return commands;
        }

        private IEnumerable<IMenuCommand> CreateOnlineUpdateStatusCommandsRest(ThreeDCartStoreEntity store)
        {
            IEnumerable<EnumEntry<Enums.ThreeDCartOrderStatus>> statuses = EnumHelper.GetEnumList<Enums.ThreeDCartOrderStatus>();

            List<IMenuCommand> commands = statuses.Select(s => s.Description)
                .Select(x =>
                    new AsyncMenuCommand(x, context => OnSetOnlineStatus(context, store))
                    {
                        Tag = EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(x)
                    })
                .OfType<IMenuCommand>()
                .ToList();

            return commands;
        }

        private IEnumerable<IMenuCommand> CreateOnlineUpdateStatusCommandsSoap(ThreeDCartStoreEntity store)
        {
            ThreeDCartStatusCodeProvider statusCodeProvider = new ThreeDCartStatusCodeProvider(store);

            List<IMenuCommand> commands = statusCodeProvider.CodeNames
                .Select(x =>
                    new AsyncMenuCommand(x, context => OnSetOnlineStatus(context, store))
                    {
                        Tag = statusCodeProvider.GetCodeValue(x)
                    })
                .OfType<IMenuCommand>()
                .ToList();

            return commands;
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private async Task OnSetOnlineStatus(MenuCommandExecutionContext context, ThreeDCartStoreEntity store)
        {
            int statusCode = (int) context.MenuCommand.Tag;

            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper, "Set Status", "ShipWorks is setting the online status.", "Updating order {0} of {1}...",
                    orderID => Task.Run(() => SetOnlineStatusCallback(orderID, statusCode, store)))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }
        
        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private async Task<IResult> SetOnlineStatusCallback(long orderID, int statusCode, ThreeDCartStoreEntity store)
        {
            try
            {
                if (store.RestUser)
                {
                    ThreeDCartRestOnlineUpdater updater = new ThreeDCartRestOnlineUpdater(store);
                    await updater.UpdateOrderStatus(orderID, statusCode).ConfigureAwait(false);
                }
                else
                {
                    ThreeDCartSoapOnlineUpdater updater = new ThreeDCartSoapOnlineUpdater(store);
                    await updater.UpdateOrderStatus(orderID, statusCode).ConfigureAwait(false);
                }
                return Result.FromSuccess();
            }
            catch (ThreeDCartException ex)
            {
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private async Task OnUploadShipmentDetails(MenuCommandExecutionContext context, ThreeDCartStoreEntity store)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    orderID => Task.Run(() => UploadShipmentDetailsCallback(orderID, store)))
                .ConfigureAwait(true);

            context.Complete(results.Select(x => x.Exception).Where(x => x != null), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, ThreeDCartStoreEntity store)
        {
            ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(orderID).ConfigureAwait(false);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Result.FromSuccess();
            }

            try
            {
                if (store.RestUser)
                {
                    ThreeDCartRestOnlineUpdater updater = new ThreeDCartRestOnlineUpdater(store);
                    await updater.UpdateShipmentDetails(shipment.ShipmentID).ConfigureAwait(false);
                }
                else
                {
                    ThreeDCartSoapOnlineUpdater updater = new ThreeDCartSoapOnlineUpdater(store);
                    await updater.UpdateShipmentDetails(shipment.ShipmentID).ConfigureAwait(false);
                }

                return Result.FromSuccess();
            }
            catch (ThreeDCartException ex)
            {
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
