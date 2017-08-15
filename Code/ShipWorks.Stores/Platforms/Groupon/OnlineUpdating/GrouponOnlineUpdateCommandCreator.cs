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

namespace ShipWorks.Stores.Platforms.Groupon.OnlineUpdating
{
    /// <summary>
    /// Create online update commands for Groupon stores
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Groupon)]
    public class GrouponOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private readonly IGrouponOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOnlineUpdateCommandCreator(IGrouponOnlineUpdater onlineUpdater, IMessageHelper messageHelper, Func<Type, ILog> createLogger)
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
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store)
        {
            IGrouponStoreEntity typedStore = store as IGrouponStoreEntity;
            if (typedStore == null)
            {
                log.Warn("Cannot create instance command for non-Groupon store");
                return Enumerable.Empty<IMenuCommand>();
            }

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadDetails(typedStore, context))
            };
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private async Task OnUploadDetails(IGrouponStoreEntity store, MenuCommandExecutionContext context)
        {
            var results = await UploadDetails(store, context.SelectedKeys).ConfigureAwait(true);

            var exceptions = results.Success ? Enumerable.Empty<Exception>() : new[] { results.Exception };
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Upload the shipping details
        /// </summary>
        private async Task<GenericResult<IEnumerable<long>>> UploadDetails(IGrouponStoreEntity store, IEnumerable<long> orderKeys)
        {
            using (var progress = messageHelper.ShowProgressDialog("Upload Shipment Details", "ShipWorks is uploading shipment information."))
            {
                progress.ToUpdater(orderKeys, "Updating {0} orders...");

                try
                {
                    await onlineUpdater.UpdateShipmentDetails(store, orderKeys).ConfigureAwait(false);
                    return GenericResult.FromSuccess(orderKeys);
                }
                catch (GrouponException ex)
                {
                    log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                    return GenericResult.FromError(ex, orderKeys);
                }
            }
        }
    }
}
