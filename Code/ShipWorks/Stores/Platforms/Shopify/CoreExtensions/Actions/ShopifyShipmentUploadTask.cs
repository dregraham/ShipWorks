using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Shopify.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment detials to Shopify
    /// </summary>
    [ActionTask("Upload shipment details", "ShopifyShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class ShopifyShipmentUploadTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is ShopifyStoreEntity;
        }

        /// <summary>
        /// This task is for Shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
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
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Executes the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, ActionStepContext context)
        {
            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys", "inputKeys is required");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context", "context is required");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            ShopifyStoreEntity store = StoreManager.GetStore(StoreID) as ShopifyStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long shipmentID in inputKeys)
                {
                    ShopifyOnlineUpdater updater = new ShopifyOnlineUpdater(store);

                    await updater.UpdateOnlineStatus(shipmentID, context.CommitWork).ConfigureAwait(false);
                }
            }
            catch (ShopifyException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
