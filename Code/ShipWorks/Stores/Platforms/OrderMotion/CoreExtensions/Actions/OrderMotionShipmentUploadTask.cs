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

namespace ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to OrderMotion
    /// </summary>
    [ActionTask("Upload shipment details", "OrderMotionShipmentUpload")]
    public class OrderMotionShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is OrderMotionStoreType;
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

                OrderMotionStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as OrderMotionStoreEntity;
                if (storeEntity == null)
                {
                    // This isnt a generic store or the store went away
                    continue;
                }

                try
                {
                    OrderMotionOnlineUpdater updater = new OrderMotionOnlineUpdater(storeEntity);
                    updater.UploadShipmentDetails(entityID);
                }
                catch (OrderMotionException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}