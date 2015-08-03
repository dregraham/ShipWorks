using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions
{
    /// <summary>
    /// Task for updating online order status for NetworkSolutions
    /// </summary>
    [ActionTask("Update store status", "NetworkSolutionsOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class NetworkSolutionsOrderUpdateTask : StoreInstanceTaskBase
    {
        long statusCode = -1;
        string comments = "";

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is NetworkSolutionsStoreEntity;
        }

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public long StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        /// <summary>
        /// The comment string to be applied to the status update
        /// </summary>
        public string Comment
        {
            get { return comments; }
            set { comments = value; }
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
            return new NetworkSolutionsOrderUpdateTaskEditor(this);
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

            NetworkSolutionsStoreEntity store = StoreManager.GetStore(StoreID) as NetworkSolutionsStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                NetworkSolutionsOnlineUpdater updater = new NetworkSolutionsOnlineUpdater(store);
                foreach (long entityID in inputKeys)
                {
                    updater.UpdateOrderStatus(entityID, statusCode, comments, context.CommitWork);
                }
            }
            catch (NetworkSolutionsException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
