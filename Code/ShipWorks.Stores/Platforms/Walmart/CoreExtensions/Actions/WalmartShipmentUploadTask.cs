using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions
{
    /// <summary>
    /// Shipment upload task for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Actions.Tasks.Common.StoreInstanceTaskBase" />
    [ActionTask("Upload shipment details", "WalmartShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class WalmartShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long MaxBatchSize = 1000;

        /// <summary>
        /// This task is for shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the shipment details for:";

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            WalmartStoreEntity walmartStore = store as WalmartStoreEntity;
            return walmartStore != null;
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

            WalmartStoreEntity store = StoreManager.GetStore(StoreID) as WalmartStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>)d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < MaxBatchSize)
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
        private void UpdloadShipmentDetails(WalmartStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    foreach (long shipmentKey in shipmentKeys)
                    {
                        ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);
                        WalmartOnlineUpdater updater = scope.Resolve<WalmartOnlineUpdater>(new TypedParameter(typeof(WalmartStoreEntity), store));

                        updater.UpdateShipmentDetails(shipment);
                    }
                }
            }
            catch (WalmartException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}