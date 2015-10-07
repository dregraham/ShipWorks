using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Actions
{
    [ActionTask("Update store status", "LemonStandOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class LemonStandOrderUpdateTask : StoreInstanceTaskBase
    {
        // Default the status code to an invalid code (so the drop down works correctly)
        int statusCode = -1;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is LemonStandStoreEntity;
        }

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public int StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;


        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            LemonStandStoreEntity store = StoreManager.GetStore(StoreID) as LemonStandStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                LemonStandOnlineUpdater updater = new LemonStandOnlineUpdater(store);
                foreach (long orderID in inputKeys)
                {
                    updater.UpdateOrderStatus(orderID, statusCode, context.CommitWork);
                }
            }
            catch (BigCommerceException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
