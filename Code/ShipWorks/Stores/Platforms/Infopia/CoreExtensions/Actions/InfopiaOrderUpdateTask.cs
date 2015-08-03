using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.Infopia;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for updating a Generic Store's online order with a status code
    /// </summary>
    [ActionTask("Update store status", "InfopiaOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class InfopiaOrderUpdateTask : StoreInstanceTaskBase
    {
        string status = null;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is InfopiaStoreEntity;
        }

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
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
            return new InfopiaOrderUpdateTaskEditor(this);
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

            InfopiaStoreEntity store = StoreManager.GetStore(StoreID) as InfopiaStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                InfopiaOnlineUpdater updater = new InfopiaOnlineUpdater(store);
                foreach (long entityID in inputKeys)
                {
                    updater.UpdateOrderStatus(entityID, status, context.CommitWork);
                }
            }
            catch (InfopiaException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
