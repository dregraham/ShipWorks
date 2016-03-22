using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

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
        public override string InputLabel
        {
            get
            {
                return "Upload tracking number of:";
            }
        }

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Insantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Execute the details upload
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            foreach (long entityID in inputKeys)
            {
                List<long> storeKeys = DataProvider.GetRelatedKeys(entityID, EntityType.StoreEntity);
                if (storeKeys.Count == 0)
                {
                    // Store or shipment disapeared
                    continue;
                }

                ThreeDCartStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as ThreeDCartStoreEntity;
                if (storeEntity == null)
                {
                    // This isnt a generic store or the store went away
                    continue;
                }

                try
                {
                    ThreeDCartRestOnlineUpdater updater = new ThreeDCartRestOnlineUpdater(storeEntity);
                    updater.UpdateShipmentDetails(entityID);
                }
                catch (ThreeDCartException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
