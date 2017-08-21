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

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Buy.com stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.NeweggMarketplace)]
    public class NeweggOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly IShipmentDetailsUpdater updater;
        private readonly ILog log;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeweggOnlineUpdateCommandCreator(IShipmentDetailsUpdater updater,
            IOrderManager orderManager,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.orderManager = orderManager;
            this.updater = updater;
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
        /// <returns></returns>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            var typedStore = store as INeweggStoreEntity;

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(typedStore, context))
            };
        }

        /// <summary>
        /// Called when [upload shipment details].
        /// </summary>
        /// <param name="context">The context.</param>
        private async Task OnUploadShipmentDetails(INeweggStoreEntity store, MenuCommandExecutionContext context)
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
        private async Task<IResult> UploadShipmentDetails(INeweggStoreEntity store, long orderID)
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
                await updater.UploadShippingDetails(store, shipment).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (NeweggException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orderID {0}: {1}", orderID, ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
