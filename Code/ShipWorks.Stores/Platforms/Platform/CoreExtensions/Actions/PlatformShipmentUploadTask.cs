using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Platform.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Platform
    /// </summary>
    /// <remarks>
    /// The ActionTask Identifier is API because this was originally written for API and I worry that customers might
    /// have an action task configured to run based on the identifier and I don't want to mess them up.
    /// </remarks>
    [ActionTask("Upload shipment details", "ApiShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class PlatformShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long MaxBatchSize = 1000;
        private readonly IPlatformOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformShipmentUploadTask(IPlatformOnlineUpdater onlineUpdater)
        {
            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() =>
            new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) =>
            !store.OrderSourceID.IsNullOrWhiteSpace();

        /// <summary>
        /// Run the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            var store = StoreManager.GetStore(StoreID);
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < MaxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                await UpdloadShipmentDetails(store, postponedKeys.Concat(inputKeys)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private async Task UpdloadShipmentDetails(StoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                await onlineUpdater.UploadShipmentDetails(store, shipmentKeys).ConfigureAwait(false);
            }
            catch (PlatformStoreException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}

