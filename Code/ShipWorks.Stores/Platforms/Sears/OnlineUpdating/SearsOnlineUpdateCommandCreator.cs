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

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Sears
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Sears)]
    public class SearsOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private readonly ISearsOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsOnlineUpdateCommandCreator(
            ISearsOnlineUpdater onlineUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
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
        /// Create any MenuCommand's that are applied to this specific store instance
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            var typedStore = store as ISearsStoreEntity;

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, typedStore))
            };
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(IMenuCommandExecutionContext context, ISearsStoreEntity store)
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
        private async Task<IResult> UploadShipmentDetailsCallback(long orderID, ISearsStoreEntity store)
        {
            try
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
                if (shipment == null)
                {
                    log.WarnFormat("Not uploading shipment details for order {0} as it went away.", orderID);
                    return Result.FromSuccess();
                }

                await onlineUpdater.UploadShipmentDetails(store, shipment).ConfigureAwait(false);

                return Result.FromSuccess();
            }
            catch (SearsException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders.  Error message: {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
