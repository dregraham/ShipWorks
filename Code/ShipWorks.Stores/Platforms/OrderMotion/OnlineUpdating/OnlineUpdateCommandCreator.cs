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

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.OrderMotion)]
    public class OnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IShipmentDetailsUpdater shipmentUpdater;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateCommandCreator(IShipmentDetailsUpdater shipmentUpdater,
            IOrderManager orderManager,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.orderManager = orderManager;
            this.shipmentUpdater = shipmentUpdater;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create online update commands
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands()
        {
            return new IMenuCommand[]
            {
                new AsyncMenuCommand("Upload Shipment Details", OnUploadShipmentDetails)
            };
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store) =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        public async Task OnUploadShipmentDetails(IMenuCommandExecutionContext context)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment",
                    "ShipWorks is uploading shipment information",
                    "Updating order {0} of {1}...",
                    x => UploadShipmentDetails(x))
                .ConfigureAwait(true);

            context.Complete(results.Select(x => x.Exception).Where(x => x != null), MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetails(long orderID)
        {
            ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(orderID).ConfigureAwait(false);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Result.FromSuccess();
            }

            try
            {
                await shipmentUpdater.UploadShipmentDetails(shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (OrderMotionException ex)
            {
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
