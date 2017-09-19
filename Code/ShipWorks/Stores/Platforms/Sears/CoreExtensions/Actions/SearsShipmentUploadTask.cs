using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Sears.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Sears
    /// </summary>
    [ActionTask("Upload shipment details", "SearsShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class SearsShipmentUploadTask : StoreTypeTaskBase
    {
        private readonly ISearsOnlineUpdater onlineUpdater;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsShipmentUploadTask(ISearsOnlineUpdater onlineUpdater, IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            this.onlineUpdater = onlineUpdater;
        }

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
        public override bool SupportsType(StoreType storeType) =>
            storeType.TypeCode == StoreTypeCode.Sears;

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Run the task
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            foreach (long shipmentID in inputKeys)
            {
                try
                {
                    var store = storeManager.GetRelatedStore(shipmentID) as ISearsStoreEntity;
                    await onlineUpdater.UploadShipmentDetails(store, shipmentID).ConfigureAwait(false);
                }
                catch (SearsException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
