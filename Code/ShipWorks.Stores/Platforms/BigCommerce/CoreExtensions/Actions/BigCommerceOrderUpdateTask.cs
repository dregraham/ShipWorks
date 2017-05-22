using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for BigCommerce
    /// </summary>
    [ActionTask("Update store status", "BigCommerceOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class BigCommerceOrderUpdateTask : StoreInstanceTaskBase
    {
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
        public override void Run(List<long> inputKeys, ActionStepContext context)
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
                    IBigCommerceOnlineUpdater updater = lifetimeScope.Resolve<IBigCommerceOnlineUpdater>(TypedParameter.From(store));
                    foreach (long orderID in inputKeys)
                    {
                        updater.UpdateOrderStatus(orderID, StatusCode, context.CommitWork);
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
