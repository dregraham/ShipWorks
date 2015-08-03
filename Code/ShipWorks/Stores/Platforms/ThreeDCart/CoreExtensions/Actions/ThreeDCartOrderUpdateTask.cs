using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for ThreeDCart
    /// </summary>
    [ActionTask("Update store status", "ThreeDCartOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class ThreeDCartOrderUpdateTask : StoreInstanceTaskBase
    {
        int statusCode = -1;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is ThreeDCartStoreEntity;
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
        public override string InputLabel
        {
            get
            {
                return "Set status of:";
            }
        }

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        /// <summary>
        /// Insantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new ThreeDCartOrderUpdateTaskEditor(this);
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

            ThreeDCartStoreEntity store = StoreManager.GetStore(StoreID) as ThreeDCartStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                ThreeDCartOnlineUpdater updater = new ThreeDCartOnlineUpdater(store);
                foreach (long orderID in inputKeys)
                {
                    updater.UpdateOrderStatus(orderID, statusCode, context.CommitWork);
                }
            }
            catch (ThreeDCartException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
