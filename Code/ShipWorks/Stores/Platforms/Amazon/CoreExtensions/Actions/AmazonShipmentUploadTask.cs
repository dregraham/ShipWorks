using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Templates.Processing;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using log4net;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment detials to Amazon
    /// </summary>
    [ActionTask("Upload shipment details", "AmazonShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class AmazonShipmentUploadTask : StoreInstanceTaskBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonShipmentUploadTask));

        const long maxBatchSize = 1000;

        /// <summary>
        /// This task is for Orders
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
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
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
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            AmazonStoreEntity store = StoreManager.GetStore(StoreID) as AmazonStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>) d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < maxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                UpdloadShipmentDetails(store, postponedKeys.Concat(inputKeys));
            }
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        private void UpdloadShipmentDetails(AmazonStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                AmazonOnlineUpdater updater = new AmazonOnlineUpdater(store);
                updater.UploadShipmentDetails(shipmentKeys);
            }
            catch (AmazonException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
