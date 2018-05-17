using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Overstock.CoreExtensions.Actions
{
    /// <summary>
    /// Shipment upload task for Overstock
    /// </summary>
    /// <seealso cref="ShipWorks.Actions.Tasks.Common.StoreInstanceTaskBase" />
    [ActionTask("Upload shipment details", "OverstockShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class OverstockShipmentUploadTask : StoreInstanceTaskBase
    {
        readonly IOverstockShipmentDetailsUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockShipmentUploadTask(IOverstockShipmentDetailsUpdater onlineUpdater)
        {
            this.onlineUpdater = onlineUpdater;
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
            OverstockStoreEntity overstockStore = store as OverstockStoreEntity;
            return overstockStore != null;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            OverstockStoreEntity store = StoreManager.GetStore(StoreID) as OverstockStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long entityID in inputKeys)
                {
                    await onlineUpdater.UploadShipmentDetails(entityID).ConfigureAwait(false);
                }
            }
            catch (OverstockException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}