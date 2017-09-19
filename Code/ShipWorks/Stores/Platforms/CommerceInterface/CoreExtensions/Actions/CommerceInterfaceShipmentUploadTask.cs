using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to CommerceInterface
    /// </summary>
    [ActionTask("Upload shipment details", "CommerceInterfaceShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class CommerceInterfaceShipmentUploadTask : StoreInstanceTaskBase
    {
        // status code to update on the order
        int statusCode;

        /// <summary>
        /// The status to set the order to
        /// </summary>
        public int StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        /// <summary>
        /// Determine if this task applies to the store provided
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return (store.TypeCode == (int)StoreTypeCode.CommerceInterface);
        }
        /// <summary>
        /// Create the UI for configuring this task
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new CommerceInterfaceShipmentUploadTaskEditor(this);
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

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

                GenericModuleStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as GenericModuleStoreEntity;
                if (storeEntity == null)
                {
                    // This isn't a generic store or the store went away
                    continue;
                }

                try
                {
                    CommerceInterfaceOnlineUpdater updater = new CommerceInterfaceOnlineUpdater(storeEntity);
                    await updater.UploadTrackingNumber(entityID, statusCode).ConfigureAwait(false);
                }
                catch (GenericStoreException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
