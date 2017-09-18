using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Actions
{
    [ActionTask("Upload shipment details", "LemonStandShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class LemonStandShipmentUploadTask : StoreInstanceTaskBase
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
            LemonStandStoreEntity lemonStandStore = store as LemonStandStoreEntity;
            return lemonStandStore != null;
        }

        /// <summary>
        /// This ActionTask should be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        ///     Run the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            LemonStandStoreEntity store = StoreManager.GetStore(StoreID) as LemonStandStoreEntity;
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
        ///     Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private static async Task UpdloadShipmentDetails(LemonStandStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    LemonStandOnlineUpdater updater = scope.Resolve<LemonStandOnlineUpdater>(TypedParameter.From(store));

                    foreach (long shipmentKey in shipmentKeys)
                    {
                        ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);

                        await updater.UpdateShipmentDetails(shipment).ConfigureAwait(false);
                    }
                }
            }
            catch (LemonStandException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}