using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Groupon
    /// </summary>
    [ActionTask("Upload shipment details", "GrouponShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class GrouponShipmentUploadTask : StoreInstanceTaskBase
    {
        private IGrouponOnlineUpdater grouponOnlineUpdater;
        private readonly IShippingManager shippingManager;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponShipmentUploadTask(IGrouponOnlineUpdater grouponOnlineUpdater, IShippingManager shippingManager,
            IStoreManager storeManager)
        {
            this.grouponOnlineUpdater = grouponOnlineUpdater;
            this.shippingManager = shippingManager;
            this.storeManager = storeManager;
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
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            GrouponStoreEntity store = storeManager.GetStore(StoreID) as GrouponStoreEntity;
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
        private async Task UpdloadShipmentDetails(GrouponStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                foreach (long shipmentKey in shipmentKeys)
                {
                    ShipmentEntity shipment = shippingManager.GetShipment(shipmentKey).Shipment;

                    await grouponOnlineUpdater.UpdateShipmentDetails(store, shipment.Order, shipment).ConfigureAwait(false);
                }
            }
            catch (GrouponException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
