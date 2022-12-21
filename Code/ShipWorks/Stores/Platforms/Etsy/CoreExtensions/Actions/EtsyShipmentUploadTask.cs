using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Etsy.OnlineUpdating;
using ShipWorks.Stores.Platforms.Platform;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Etsy
    /// </summary>
    [ActionTask("Upload shipment details", "EtsyShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class EtsyShipmentUploadTask : StoreInstanceTaskBase
    {
        const long maxBatchSize = 300;
        protected readonly IPlatformOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyShipmentUploadTask(IIndex<StoreTypeCode, IPlatformOnlineUpdater> platformOnlineUpdaterIndex)
        {
            this.onlineUpdater = platformOnlineUpdaterIndex[StoreTypeCode.Etsy];
        }

        /// <summary>
        /// This task is for shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) => store is EtsyStoreEntity;

        /// <summary>
        /// Run the task
        /// TODO: copied from Amazon, should be extracted to common base
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            MethodConditions.EnsureArgumentIsNotNull(context, nameof(context));
            MethodConditions.EnsureArgumentIsNotNull(inputKeys, nameof(inputKeys));

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            EtsyStoreEntity store = StoreManager.GetStore(StoreID) as EtsyStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            await UploadShipmentDetails(store, inputKeys).ConfigureAwait(false);
        }

        private async Task UploadShipmentDetails(StoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                await onlineUpdater.UploadShipmentDetails(store, shipmentKeys).ConfigureAwait(false);
            }
            catch (PlatformStoreException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
