using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

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

        public override ActionTaskEditor CreateEditor()
        {
            return IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<ActionTaskEditor>(StoreTypeCode.LemonStand, new TypedParameter(typeof(LemonStandOrderUpdateTask), this));
        }

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;

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
            catch (LemonStandException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
