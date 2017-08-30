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
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Buy.com stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.BuyDotCom)]
    public class BuyDotComOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly IShipmentDetailsUpdater updater;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComOnlineUpdateCommandCreator(IShipmentDetailsUpdater updater, IMessageHelper messageHelper, Func<Type, ILog> createLogger)
        {
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
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(context, store as IBuyDotComStoreEntity))
            };
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        public async Task OnUploadDetails(IMenuCommandExecutionContext context, IBuyDotComStoreEntity store)
        {
            IResult result = null;

            using (var dialog = messageHelper.ShowProgressDialog("Upload Shipment Details", "ShipWorks is uploading shipment information."))
            {
                dialog.ToUpdater($"Updating {context.SelectedKeys.Count()} orders...");
                result = await UploadShipmentDetails(store, context.SelectedKeys).ConfigureAwait(true);
            }

            var errors = result.Success ? Enumerable.Empty<Exception>() : new[] { result.Exception };

            context.Complete(errors, MenuCommandResult.Error);
        }

        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private async Task<IResult> UploadShipmentDetails(IBuyDotComStoreEntity store, IEnumerable<long> orderKeys)
        {
            try
            {
                await updater.UploadShipmentDetails(store, orderKeys).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (BuyDotComException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}
