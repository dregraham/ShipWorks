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

namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ProStores)]
    public class OnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IShipmentDetailsUpdater shipmentUpdater;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateCommandCreator(IMessageHelper messageHelper, IShipmentDetailsUpdater shipmentUpdater, Func<Type, ILog> createLogger)
        {
            this.messageHelper = messageHelper;
            this.shipmentUpdater = shipmentUpdater;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands()
        {
            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", OnUploadDetails)
            };
        }

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store) =>
            Enumerable.Empty<IMenuCommand>();

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(IMenuCommandExecutionContext context)
        {
            IResult result = null;
            using (var dialog = messageHelper.ShowProgressDialog("Upload Shipment Details", "ShipWorks is uploading shipment information."))
            {
                dialog.ToUpdater($"Updating {context.SelectedKeys.Count()} orders...");
                result = await Task.Run(() => ShipmentUploadCallback(context.SelectedKeys)).ConfigureAwait(true);
            }

            var exceptions = result.Failure ? new[] { result.Exception } : Enumerable.Empty<Exception>();
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> ShipmentUploadCallback(IEnumerable<long> orderKeys)
        {
            try
            {
                await shipmentUpdater.UploadOrderShipmentDetails(orderKeys).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (ProStoresException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
