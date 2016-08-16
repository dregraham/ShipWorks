using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.ApplicationCore;
using Autofac;

namespace ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions
{
    [ActionTask("Upload shipment details", "SparkPayShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class SparkPayShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long maxBatchSize = 1000;

        /// <summary>
        ///     This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        ///     Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        ///     Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        ///     Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            SparkPayStoreEntity sparkPayStore = store as SparkPayStoreEntity;
            return sparkPayStore != null;
        }

        /// <summary>
        ///     Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            SparkPayStoreEntity store = StoreManager.GetStore(StoreID) as SparkPayStoreEntity;
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
                UpdloadShipmentDetails(store, postponedKeys.Concat(inputKeys));
            }
        }

        /// <summary>
        ///     Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private void UpdloadShipmentDetails(SparkPayStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    foreach (long shipmentKey in shipmentKeys)
                    {
                        ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);
                        SparkPayOnlineUpdater updater = scope.Resolve<SparkPayOnlineUpdater>(new TypedParameter(typeof(SparkPayStoreEntity), store));

                        updater.UpdateShipmentDetails(shipment);
                    }
                }
            }
            catch (SparkPayException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}