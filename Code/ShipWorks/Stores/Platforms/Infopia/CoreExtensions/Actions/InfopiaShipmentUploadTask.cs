using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.Infopia;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to an Infopia Store
    /// </summary>
    [ActionTask("Upload shipment details", "InfopiaShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class InfopiaShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is InfopiaStoreType;
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
        /// This task only operates on orders
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

                InfopiaStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as InfopiaStoreEntity;
                if (storeEntity == null)
                {
                    // This isnt a generic store or the store went away
                    continue;
                }

                try
                {
                    InfopiaOnlineUpdater updater = new InfopiaOnlineUpdater(storeEntity);
                    updater.UploadShipmentDetails(entityID);
                }
                catch (InfopiaException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
