using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data;
using Interapptive.Shared.Net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to a Generic Store
    /// </summary>
    [ActionTask("Upload shipment details", "GenericStoreShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class GenericStoreShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            if (!(storeType is GenericModuleStoreType))
            {
                return false;
            }

            // We don't support this for generic store derivates that provide their own implementation
            if (storeType.TypeCode == Stores.StoreTypeCode.Magento)
            {
                return false;
            }

            // Supported if there are any stores of this exact type code that support shipment details
            return StoreManager.GetAllStores().Any(s => s.TypeCode == (int) storeType.TypeCode && ((GenericModuleStoreEntity) s).ModuleOnlineShipmentDetails);
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

                GenericModuleStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as GenericModuleStoreEntity;
                if (storeEntity == null)
                {
                    // This isnt a generic store or the store went away
                    continue;
                }

                if (!storeEntity.ModuleOnlineShipmentDetails)
                {
                    throw new ActionTaskRunException("The store no longer supports online shipment updates.");
                }

                try
                {
                    GenericStoreOnlineUpdater updater = new GenericStoreOnlineUpdater(storeEntity);
                    updater.UploadTrackingNumber(entityID);
                }
                catch (GenericStoreException ex)
                {
                    // Could be that uploading shipment details isn't supported, even though the store claimed it was
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
