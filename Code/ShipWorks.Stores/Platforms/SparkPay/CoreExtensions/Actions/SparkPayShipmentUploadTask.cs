using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions
{
    [ActionTask("Upload shipment details", "SparkPayShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class SparkPayShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long maxBatchSize = 1000;
        private readonly ISparkPayOnlineUpdater onlineUpdater;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayShipmentUploadTask(ISparkPayOnlineUpdater onlineUpdater, IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            this.onlineUpdater = onlineUpdater;
        }

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
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            SparkPayStoreEntity sparkPayStore = store as SparkPayStoreEntity;
            return sparkPayStore != null;
        }

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Run the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            var store = storeManager.GetStore(StoreID) as SparkPayStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < maxBatchSize)
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
        private async Task UpdloadShipmentDetails(SparkPayStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                foreach (long shipmentKey in shipmentKeys)
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);

                    await onlineUpdater.UpdateShipmentDetails(store, shipment).ConfigureAwait(false);
                }
            }
            catch (SparkPayException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}