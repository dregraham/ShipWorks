using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment deails to CommerceInterface
    /// </summary>
    [ActionTask("Upload shipment details", "CommerceInterfaceShipmentUpload")]
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

                try
                {
                    CommerceInterfaceOnlineUpdater updater = new CommerceInterfaceOnlineUpdater(storeEntity);
                    updater.UploadTrackingNumber(entityID, statusCode);
                }
                catch (GenericStoreException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }

      
    }
}
