using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Etsy;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Etsy
    /// </summary>
    [ActionTask("Upload shipment details", "EtsyShipmentUploadTask")]
    public class EtsyShipmentUploadTask : StoreInstanceTaskBase
    {

        const long maxBatchSize = 300;

        /// <summary>
        /// This task is for shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload the tracking number for:";
            }
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            EtsyStoreEntity etsyStore = store as EtsyStoreEntity;

            if (etsyStore == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (context==null)
            {
                throw new ArgumentNullException("context");
            }

            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            EtsyStoreEntity store = StoreManager.GetStore(StoreID) as EtsyStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            EtsyOnlineUpdater updater = new EtsyOnlineUpdater(store);

            foreach (var shipmentID in inputKeys)
            {
                // Upload the details, first starting with all the postponed input, plus the current input
                try
                {
                    updater.UploadShipmentDetails(shipmentID, context.CommitWork);
                }
                catch (EtsyException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }


    }
}
