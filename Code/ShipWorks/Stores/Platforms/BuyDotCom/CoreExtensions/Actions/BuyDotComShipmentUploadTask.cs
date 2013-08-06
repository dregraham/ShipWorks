using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.BuyDotCom;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.BuyDotCom.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Buy.com
    /// </summary>
    [ActionTask("Upload shipment details", "BuyDotComShipmentUploadTask")]
    public class BuyDotComShipmentUploadTask : StoreInstanceTaskBase
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
            BuyDotComStoreEntity buyDotComStore = store as BuyDotComStoreEntity;
            if (buyDotComStore == null)
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

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            BuyDotComStoreEntity store = StoreManager.GetStore(StoreID) as BuyDotComStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count + inputKeys.Count() < maxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                UploadShipmentDetails(store, postponedKeys.Concat(inputKeys));
            }
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private void UploadShipmentDetails(BuyDotComStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                BuyDotComOnlineUpdater updater = new BuyDotComOnlineUpdater(store);
                updater.UploadShipmentDetails(shipmentKeys);
            }
            catch (BuyDotComException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
