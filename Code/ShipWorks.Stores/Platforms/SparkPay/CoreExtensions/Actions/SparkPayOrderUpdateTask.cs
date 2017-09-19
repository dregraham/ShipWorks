using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions
{
    [ActionTask("Update store status", "SparkPayOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class SparkPayOrderUpdateTask : StoreInstanceTaskBase
    {
        private readonly ISparkPayOnlineUpdater onlineUpdater;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayOrderUpdateTask(ISparkPayOnlineUpdater onlineUpdater, IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) => store is SparkPayStoreEntity;

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public int StatusCode { get; set; } = -1;

        /// <summary>
        /// Create the editor
        /// </summary>
        public override ActionTaskEditor CreateEditor() =>
            IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<ActionTaskEditor>(StoreTypeCode.SparkPay, TypedParameter.From(this));

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;

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

            var store = storeManager.GetStore(StoreID) as SparkPayStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long orderID in inputKeys)
                {
                    await onlineUpdater.UpdateOrderStatus(store, orderID, StatusCode, context.CommitWork).ConfigureAwait(false);
                }
            }
            catch (SparkPayException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
