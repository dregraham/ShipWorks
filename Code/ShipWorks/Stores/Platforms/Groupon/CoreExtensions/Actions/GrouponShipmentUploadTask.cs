using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment detials to Groupon
    /// </summary>
    [ActionTask("Upload shipment details", "GrouponShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class GrouponShipmentUploadTask : StoreInstanceTaskBase
    {
        const long maxBatchSize = 1000;

        /// <summary>
        /// This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload the tracking number for:";
            }
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            GrouponStoreEntity grouponStore = store as GrouponStoreEntity;
            if (grouponStore == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            GrouponStoreEntity store = StoreManager.GetStore(StoreID) as GrouponStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>)d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < maxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                UpdloadShipmentDetails(store, postponedKeys.Concat(inputKeys));
            }
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private static void UpdloadShipmentDetails(GrouponStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                foreach(long shipmentKey in shipmentKeys)
                {
                    GrouponOnlineUpdater updater = new GrouponOnlineUpdater(store);

                    ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);

                    updater.UpdateShipmentDetails(shipment);
                }
            }
            catch (GrouponException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
