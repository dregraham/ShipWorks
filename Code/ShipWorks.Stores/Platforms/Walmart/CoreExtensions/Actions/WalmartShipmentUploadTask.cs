using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions
{
    /// <summary>
    /// Shipment upload task for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Actions.Tasks.Common.StoreInstanceTaskBase" />
    [ActionTask("Upload shipment details", "WalmartShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class WalmartShipmentUploadTask : StoreInstanceTaskBase
    {
        private readonly IShipmentDetailsUpdater onlineUpdater;
        private readonly IShippingManager shippingManager;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartShipmentUploadTask(IShipmentDetailsUpdater onlineUpdater, IShippingManager shippingManager,
            IStoreManager storeManager)
        {
            this.shippingManager = shippingManager;
            this.onlineUpdater = onlineUpdater;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

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
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            WalmartStoreEntity store = storeManager.GetStore(StoreID) as WalmartStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Upload the details, first starting with all the postponed input, plus the current input
            await UpdloadShipmentDetails(store, inputKeys).ConfigureAwait(false);
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private async Task UpdloadShipmentDetails(WalmartStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                foreach (long shipmentKey in shipmentKeys)
                {
                    ShipmentEntity shipment = shippingManager.GetShipment(shipmentKey).Shipment;

                    await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
                    await onlineUpdater.UpdateShipmentDetails(store, shipment).ConfigureAwait(false);
                }
            }
            catch (WalmartException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}