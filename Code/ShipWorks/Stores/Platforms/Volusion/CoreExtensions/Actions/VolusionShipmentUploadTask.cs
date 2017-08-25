using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Volusion.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Volusion.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Volusion
    /// </summary>
    [ActionTask("Upload shipment details", "VolusionShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class VolusionShipmentUploadTask : StoreTypeTaskBase
    {
        private readonly IShipmentDetailsUpdater shipmentUpdater;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionShipmentUploadTask(IShipmentDetailsUpdater shipmentUpdater, Func<Type, ILog> createLogger)
        {
            this.shipmentUpdater = shipmentUpdater;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Option for allowing Volusion to send the shipment details email
        /// </summary>
        public bool SendEmail { get; set; } = true;

        /// <summary>
        /// Limit this task to just Volusion stores.
        /// </summary>
        public override bool SupportsType(StoreType storeType) => storeType is VolusionStoreType;

        /// <summary>
        /// Create the UI for configuring this task
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor() =>
            new VolusionShipmentUploadTaskEditor(this);

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Execute the details upload
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, ActionStepContext context)
        {
            foreach (long entityID in inputKeys)
            {
                List<long> storeKeys = DataProvider.GetRelatedKeys(entityID, EntityType.StoreEntity);
                if (storeKeys.Count == 0)
                {
                    // Store or shipment disappeared
                    continue;
                }

                VolusionStoreEntity store = StoreManager.GetStore(storeKeys[0]) as VolusionStoreEntity;
                if (store == null)
                {
                    // This isn't a Volusion store or the store went away
                    continue;
                }

                try
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(entityID);
                    if (shipment == null)
                    {
                        log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", entityID);
                        continue;
                    }

                    await shipmentUpdater.UploadShipmentDetails(store, shipment, SendEmail, context.CommitWork).ConfigureAwait(false);
                }
                catch (VolusionException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
