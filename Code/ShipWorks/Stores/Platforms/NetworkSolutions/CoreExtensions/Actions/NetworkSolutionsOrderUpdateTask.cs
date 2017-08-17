using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for NetworkSolutions
    /// </summary>
    [ActionTask("Update store status", "NetworkSolutionsOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class NetworkSolutionsOrderUpdateTask : StoreInstanceTaskBase
    {
        private readonly IOrderStatusUpdater orderStatusUpdater;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOrderUpdateTask(IOrderStatusUpdater orderStatusUpdater, IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            this.orderStatusUpdater = orderStatusUpdater;
        }

        /// <summary>
        /// Should the task run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) =>
            store is NetworkSolutionsStoreEntity;

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public long StatusCode { get; set; } = -1;

        /// <summary>
        /// The comment string to be applied to the status update
        /// </summary>
        public string Comment { get; set; } = string.Empty;

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
        public override ActionTaskEditor CreateEditor() => new NetworkSolutionsOrderUpdateTaskEditor(this);

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            NetworkSolutionsStoreEntity store = storeManager.GetStore(StoreID) as NetworkSolutionsStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long entityID in inputKeys)
                {
                    await orderStatusUpdater.UpdateOrderStatus(store, entityID, StatusCode, Comment, context.CommitWork).ConfigureAwait(false);
                }
            }
            catch (NetworkSolutionsException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
