using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to OrderMotion
    /// </summary>
    [ActionTask("Upload shipment details", "OrderMotionShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class OrderMotionShipmentUploadTask : StoreTypeTaskBase
    {
        readonly IShipmentDetailsUpdater shipmentUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionShipmentUploadTask(IShipmentDetailsUpdater shipmentUpdater)
        {
            this.shipmentUpdater = shipmentUpdater;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public override bool SupportsType(StoreType storeType) =>
            storeType is OrderMotionStoreType;

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
                try
                {
                    await shipmentUpdater.UploadShipmentDetails(entityID).ConfigureAwait(false);
                }
                catch (OrderMotionException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}