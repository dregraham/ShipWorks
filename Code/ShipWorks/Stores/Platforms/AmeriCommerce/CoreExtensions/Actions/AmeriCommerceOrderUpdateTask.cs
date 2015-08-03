using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for AmeriCommerce
    /// </summary>
    [ActionTask("Update store status", "AmeriCommerceOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class AmeriCommerceOrderUpdateTask : StoreInstanceTaskBase
    {
        int statusCode = -1;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is AmeriCommerceStoreEntity;
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
            return new AmeriCommerceOrderUpdateTaskEditor(this);
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

            AmeriCommerceStoreEntity store = StoreManager.GetStore(StoreID) as AmeriCommerceStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                AmeriCommerceOnlineUpdater updater = new AmeriCommerceOnlineUpdater(store);
                foreach (long entityID in inputKeys)
                {
                    updater.UpdateOrderStatus(entityID, statusCode, context.CommitWork);
                }
            }
            catch (AmeriCommerceException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
