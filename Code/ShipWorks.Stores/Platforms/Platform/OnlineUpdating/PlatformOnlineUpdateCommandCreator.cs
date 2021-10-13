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

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Platform stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Api)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.BrightpearlHub)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.WalmartHub)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.ChannelAdvisorHub)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.VolusionHub)]
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.GrouponHub)]
    public class PlatformUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IPlatformOnlineUpdater platformOnlineUpdater;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformUpdateCommandCreator(IPlatformOnlineUpdater platformOnlineUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> createLogger)
        {
            this.platformOnlineUpdater = platformOnlineUpdater;
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

                    await platformOnlineUpdater.UploadOrderShipmentDetails(orderKeys).ConfigureAwait(false);

                    return Result.FromSuccess();
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);

                return Result.FromError(ex);
            }
        }
    }
}
