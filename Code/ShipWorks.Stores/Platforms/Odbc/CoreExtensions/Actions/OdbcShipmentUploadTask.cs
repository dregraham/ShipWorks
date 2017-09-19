using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Upload;

namespace ShipWorks.Stores.Platforms.Odbc.CoreExtensions.Actions
{
    /// <summary>
    /// Action task for uploading shipment details for ODBC
    /// </summary>
    [ActionTask("Upload shipment details", "OdbcShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class OdbcShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long MaxBatchSize = 1000;

        /// <summary>
        /// Gets the type of the input entity.
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Gets the input label.
        /// </summary>
        public override string InputLabel => "Upload the shipment details for:";

        /// <summary>
        /// Creates the action task editor.
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Whether this task supports the given store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            OdbcStoreEntity odbcStore = store as OdbcStoreEntity;

            return odbcStore != null && odbcStore.UploadStrategy != (int) OdbcShipmentUploadStrategy.DoNotUpload;
        }

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Runs this upload task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            OdbcStoreEntity store = StoreManager.GetStore(StoreID) as OdbcStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < MaxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                await UpdloadShipmentDetails(store, postponedKeys.Concat(inputKeys)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        private async Task UpdloadShipmentDetails(OdbcStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    IOdbcUploader uploader = scope.Resolve<IOdbcUploader>();
                    await uploader.UploadShipments(store, shipmentKeys).ConfigureAwait(false);
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}