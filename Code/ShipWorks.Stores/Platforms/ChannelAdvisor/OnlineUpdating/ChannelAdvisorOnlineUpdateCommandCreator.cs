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

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IChannelAdvisorOnlineUpdater onlineUpdater;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorOnlineUpdateCommandCreator(IChannelAdvisorOnlineUpdater onlineUpdater,
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
        /// Create menu commands for uploading shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, store))
            };
        }

        /// <summary>
        /// MenuCommand handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(IMenuCommandExecutionContext context, IStoreEntity store)
        {
            var typedStore = (IChannelAdvisorStoreEntity) store;

            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information.",
                    "Updating order {0} of {1}...",
                    orderID => ShipmentUploadCallback(orderID, typedStore))
                .ConfigureAwait(true);

            var exceptions = results.Where(x => x.Failure).Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> ShipmentUploadCallback(long orderID, IChannelAdvisorStoreEntity store)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Result.FromSuccess();
            }

            try
            {
                await onlineUpdater.UploadTrackingNumber(store, shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (ChannelAdvisorException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
