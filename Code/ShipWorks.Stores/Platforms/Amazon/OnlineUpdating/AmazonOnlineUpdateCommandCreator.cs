using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Amazon stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Amazon)]
    public class AmazonOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly IAmazonOnlineUpdater shipmentUpdater;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOnlineUpdateCommandCreator(IAmazonOnlineUpdater shipmentUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.shipmentUpdater = shipmentUpdater;
            this.messageHelper = messageHelper;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(store, context))
            };
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(StoreEntity store, IMenuCommandExecutionContext context)
        {
            var results = await ShipmentUploadCallback(store, context.SelectedKeys).ConfigureAwait(true);
            var exceptions = results.Failure ? new[] { results.Exception } : Enumerable.Empty<Exception>();

            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> ShipmentUploadCallback(StoreEntity store, IEnumerable<long> orderKeys)
        {
            // upload the tracking number for the most recent processed, not voided shipment
            try
            {
                using (var progressDialog = messageHelper.ShowProgressDialog(
                    "Upload Shipment Details",
                    "ShipWorks is uploading shipment information."))
                {
                    progressDialog.ToUpdater($"Updating {orderKeys} orders...");

                    await shipmentUpdater.UploadOrderShipmentDetails((AmazonStoreEntity) store, orderKeys).ConfigureAwait(false);

                    return Result.FromSuccess();
                }
            }
            catch (AmazonException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                return Result.FromError(ex);
            }
        }
    }
}
