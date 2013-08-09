using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// A task to automate order downloads to run on a scheduled basis.
    /// </summary>
    [ActionTask("Download orders", "DownloadOrders", ActionTriggerClassifications.Scheduled)]
    public class DownloadOrdersTask : ActionTask
    {
        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new DownloadOrdersTaskEditor();
        }

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public override bool RequiresInput
        {
            get { return false; }
        }

        /// <summary>
        /// Run the task.
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            ActionEntity action = ActionManager.GetAction(context.Queue.ActionID);
            if (action != null)
            {
                List<StoreEntity> storeEntities = new List<StoreEntity>();

                foreach (long storeId in action.StoreLimitedList)
                {
                    StoreEntity storeEntity = StoreManager.GetStore(storeId);
                    storeEntities.Add(storeEntity);
                }

                DownloadManager.StartDownload(storeEntities, DownloadInitiatedBy.ShipWorks);
            }
    }
    }
}
