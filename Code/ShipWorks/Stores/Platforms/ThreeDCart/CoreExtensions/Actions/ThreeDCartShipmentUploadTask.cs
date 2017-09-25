using System.Collections.Generic;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.ThreeDCart.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to ThreeDCart
    /// </summary>
    [ActionTask("Upload shipment details", "ThreeDCartShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class ThreeDCartShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is ThreeDCartStoreType;
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
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Execute the status updates
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

                ThreeDCartStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as ThreeDCartStoreEntity;
                if (storeEntity == null)
                {
                    // This is not the store you're looking for
                    continue;
                }

                try
                {
                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        if (storeEntity.RestUser)
                        {
                            ThreeDCartRestOnlineUpdater updater = scope.Resolve<ThreeDCartRestOnlineUpdater>(TypedParameter.From(storeEntity));
                            await updater.UpdateShipmentDetails(entityID).ConfigureAwait(false);
                        }
                        else
                        {
                            ThreeDCartSoapOnlineUpdater updater = scope.Resolve<ThreeDCartSoapOnlineUpdater>(TypedParameter.From(storeEntity));
                            await updater.UpdateShipmentDetails(entityID).ConfigureAwait(false);
                        }
                    }
                }
                catch (ThreeDCartException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
