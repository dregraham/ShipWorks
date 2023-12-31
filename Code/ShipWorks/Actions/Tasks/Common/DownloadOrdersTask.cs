﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// A task to automate order downloads to run on a scheduled basis.
    /// </summary>
    [ActionTask("Download orders", "DownloadOrders", ActionTaskCategory.UpdateLocally)]
    public class DownloadOrdersTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DownloadOrdersTask));
        private List<long> storeIDs;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOrdersTask"/> class.
        /// </summary>
        public DownloadOrdersTask()
        {
            storeIDs = new List<long>();
        }

        /// <summary>
        /// Gets or sets the list of store IDs that should have orders downloaded.
        /// </summary>
        public IEnumerable<long> StoreIDs
        {
            get { return storeIDs; }
            set { storeIDs = new List<long>(value); }
        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new DownloadOrdersTaskEditor(this);
        }

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement
        {
            get { return ActionTaskInputRequirement.None; }
        }

        /// <summary>
        /// Run the task.
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            // Null stores were causing the AddToDownloadQueue method to cause the scheduler to crash
            List<StoreEntity> storeEntities =  StoreIDs.Select(StoreManager.GetStore).Where(x => x != null).ToList();

            // Log any missing store ids.  These stores were most likely deleted since the task was created
            foreach (long storeId in StoreIDs.Except(storeEntities.Select(x => x.StoreID)))
            {
                log.WarnFormat("Could not find store with id {0}", storeId);
            }

            DownloadManager.StartDownload(storeEntities, DownloadInitiatedBy.ShipWorks);
        }

        /// <summary>
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="triggerType">Type of trigger that should be tested</param>
        /// <returns></returns>
        public override bool IsAllowedForTrigger(ActionTriggerType triggerType)
        {
            return triggerType == ActionTriggerType.Scheduled;
        }
    }
}
