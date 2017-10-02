using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

namespace ShipWorks.Stores.Platforms.ThreeDCart.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for ThreeDCart
    /// </summary>
    [ActionTask("Update store status", "ThreeDCartOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class ThreeDCartOrderUpdateTask : StoreInstanceTaskBase
    {
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
            return new ThreeDCartOrderUpdateTaskEditor(this);
        }

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
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
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    if (store.RestUser)
                    {
                        ThreeDCartRestOnlineUpdater updater = scope.Resolve<ThreeDCartRestOnlineUpdater>(TypedParameter.From(store));
                        foreach (long orderID in inputKeys)
                        {
                            await updater.UpdateOrderStatus(orderID, StatusCode, context.CommitWork).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        ThreeDCartSoapOnlineUpdater updater = scope.Resolve<ThreeDCartSoapOnlineUpdater>(TypedParameter.From(store));
                        foreach (long orderID in inputKeys)
                        {
                            await updater.UpdateOrderStatus(orderID, StatusCode, context.CommitWork).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (ThreeDCartException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
