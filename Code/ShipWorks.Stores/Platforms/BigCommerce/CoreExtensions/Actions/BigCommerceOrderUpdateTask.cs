using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for BigCommerce
    /// </summary>
    [ActionTask("Update store status", "BigCommerceOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class BigCommerceOrderUpdateTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is BigCommerceStoreEntity;
        }

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        /// <remarks>
        /// Default the status code to an invalid code (so the drop down works correctly)
        /// </remarks>
        public int StatusCode { get; set; } = -1;

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IBigCommerceOrderUpdateEditorFactory factory = lifetimeScope.Resolve<IBigCommerceOrderUpdateEditorFactory>();
                return factory.Create(this);
            }
        }

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            BigCommerceStoreEntity store = StoreManager.GetStore(StoreID) as BigCommerceStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IBigCommerceOrderStatusUpdater updater = lifetimeScope.Resolve<IBigCommerceOrderStatusUpdater>(TypedParameter.From(store));
                    foreach (long orderID in inputKeys)
                    {
                        await updater.UpdateOrderStatus(store, orderID, StatusCode, context.CommitWork).ConfigureAwait(false);
                    }
                }
            }
            catch (BigCommerceException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
