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

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Generates and handles OnlineUpdateInstanceCommands
    /// </summary>
    [KeyedComponent(typeof(IOnlineUpdateCommandCreator), StoreTypeCode.Jet)]
    public class JetUpdateOnlineCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly JetOnlineUpdater onlineUpdater;
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetOnlineUpdateInstanceCommands"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="onlineUpdaterFactory">The online updater.</param>
        /// <param name="logFactory">The log factory.</param>
        public JetUpdateOnlineCommandCreator(JetOnlineUpdater onlineUpdater,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory)
        {
            this.messageHelper = messageHelper;
            this.onlineUpdater = onlineUpdater;
            log = logFactory(GetType());
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
            IJetStoreEntity jetStore = store as IJetStoreEntity;
            if (jetStore == null)
            {
                throw new JetException("Attempted to create Jet instance commands for a non Jet store");
            }

            return new[]
            {
                new AsyncMenuCommand("Upload Shipment Details", context => OnUploadShipmentDetails(jetStore, context))
                {
                    BreakAfter = true
                }
            };
        }

        /// <summary>
        /// Called when the user clicks the upload shipment details menu command
        ///  </summary>
        public async Task OnUploadShipmentDetails(IJetStoreEntity store, IMenuCommandExecutionContext context)
        {
            var results = await context.SelectedKeys
                .SelectWithProgress(messageHelper,
                    "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...",
                    x => Task.Run(() => UploadShipmentDetails(x, store)))
                .ConfigureAwait(true);

            var exceptions = results.Select(x => x.Exception).Where(x => x != null);
            context.Complete(exceptions, MenuCommandResult.Error);
        }

        /// <summary>
        /// Uploads shipment details for the given order
        /// </summary>
        private async Task<IResult> UploadShipmentDetails(long orderID, IJetStoreEntity store)
        {
            try
            {
                await onlineUpdater.UpdateShipmentDetails(orderID, store).ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (JetException ex)
            {
                log.ErrorFormat("Error uploading shipment information for orders {0}", ex.Message);
                return Result.FromError(ex);
            }
        }
    }
}