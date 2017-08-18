using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to an Infopia Store
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [ActionTask("Upload shipment details", "InfopiaShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class InfopiaShipmentUploadTask : StoreTypeTaskBase
    {
        private readonly IInfopiaOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaShipmentUploadTask(IInfopiaOnlineUpdater onlineUpdater)
        {
            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task supports the given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType) => storeType is InfopiaStoreType;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor() =>
            new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Execute the details upload
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            foreach (long entityID in inputKeys)
            {
                List<long> storeKeys = DataProvider.GetRelatedKeys(entityID, EntityType.StoreEntity);
                if (storeKeys.Count == 0)
                {
                    // Store or shipment disappeared
                    continue;
                }

                InfopiaStoreEntity store = StoreManager.GetStore(storeKeys[0]) as InfopiaStoreEntity;
                if (store == null)
                {
                    // This isn't a generic store or the store went away
                    continue;
                }

                try
                {
                    await onlineUpdater.UploadShipmentDetails(store, entityID).ConfigureAwait(false);
                }
                catch (InfopiaException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
