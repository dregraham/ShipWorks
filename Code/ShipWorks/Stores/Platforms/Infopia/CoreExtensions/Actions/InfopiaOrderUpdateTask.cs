using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for updating a Generic Store's online order with a status code
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [ActionTask("Update store status", "InfopiaOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class InfopiaOrderUpdateTask : StoreInstanceTaskBase
    {
        private readonly IInfopiaOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOrderUpdateTask(IInfopiaOnlineUpdater onlineUpdater)
        {
            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// Should the async version be used
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) =>
            store is InfopiaStoreEntity;

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public string Status { get; set; }

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
        public override ActionTaskEditor CreateEditor() =>
            new InfopiaOrderUpdateTaskEditor(this);

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            InfopiaStoreEntity store = StoreManager.GetStore(StoreID) as InfopiaStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long entityID in inputKeys)
                {
                    await onlineUpdater.UpdateOrderStatus(store, entityID, Status, context.CommitWork).ConfigureAwait(false);
                }
            }
            catch (InfopiaException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
