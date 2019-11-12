using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Rakuten.CoreExtensions.Actions
{
    /// <summary>
    /// Shipment upload task for Rakuten
    /// </summary>
    /// <seealso cref="ShipWorks.Actions.Tasks.Common.StoreInstanceTaskBase" />
    [ActionTask("Upload shipment details", "RakutenShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class RakutenShipmentUploadTask : StoreInstanceTaskBase
    {
        readonly RakutenOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenShipmentUploadTask(RakutenOnlineUpdater onlineUpdater)
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
            RakutenStoreEntity rakutenStore = store as RakutenStoreEntity;
            return rakutenStore != null;
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

            RakutenStoreEntity store = StoreManager.GetStore(StoreID) as RakutenStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long entityID in inputKeys)
                {
                    await onlineUpdater.UploadTrackingNumber(store, entityID).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}