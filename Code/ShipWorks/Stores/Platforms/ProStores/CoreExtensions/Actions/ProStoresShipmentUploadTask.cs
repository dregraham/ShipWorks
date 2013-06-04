using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to a ProStores Store
    /// </summary>
    [ActionTask("Upload shipment details", "ProStoresShipmentUpload")]
    public class ProStoresShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is ProStoresStoreType;
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
            try
            {
                ProStoresOnlineUpdater updater = new ProStoresOnlineUpdater();
                updater.UploadShipmentDetails(inputKeys);
            }
            catch (ProStoresException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
