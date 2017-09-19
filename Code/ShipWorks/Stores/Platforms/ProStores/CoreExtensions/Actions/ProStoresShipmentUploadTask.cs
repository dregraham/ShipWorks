using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Platforms.ProStores.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to a ProStores Store
    /// </summary>
    [ActionTask("Upload shipment details", "ProStoresShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class ProStoresShipmentUploadTask : StoreTypeTaskBase
    {
        readonly IShipmentDetailsUpdater shipmentUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresShipmentUploadTask(IShipmentDetailsUpdater shipmentUpdater)
        {
            this.shipmentUpdater = shipmentUpdater;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task supports the given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType) => storeType is ProStoresStoreType;

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
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Execute the details upload
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            try
            {
                await shipmentUpdater.UploadShipmentDetails(inputKeys).ConfigureAwait(false);
            }
            catch (ProStoresException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
