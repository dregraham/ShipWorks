using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Manages and provides access to the actions in the system
    /// </summary>
    public static class ActionManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionManager));

        static TableSynchronizer<ActionEntity> tableSynchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize table synchronizer
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            tableSynchronizer = new TableSynchronizer<ActionEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (tableSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (tableSynchronizer)
            {
                // Do this outside of a transaction.  I added this b\c DeleteStoreActions gets called in DeleteStore, which is a long running transaction.  The Actions of
                // course would be deleted in a transaction, but there's no need for this query to be in a transaciton.
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                //{
                if (tableSynchronizer.Synchronize())
                {
                    tableSynchronizer.EntityCollection.Sort((int) ActionFieldIndex.Name, ListSortDirection.Ascending);
                }
                //}

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the current list of all actions
        /// </summary>
        public static IList<ActionEntity> Actions
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(tableSynchronizer.EntityCollection);
                }
            }
        }

        /// <summary>
        /// Get the action with the given ID, or null of no such action exists.
        /// </summary>
        public static ActionEntity GetAction(long actionID)
        {
            return Actions.SingleOrDefault(a => a.ActionID == actionID);
        }

        /// <summary>
        /// Instantiate an existing task with the given settings, but no attached ActionTaskEntity
        /// </summary>
        public static ActionTask InstantiateTask(string identifier, string taskSettings)
        {
            // We need the descriptor for this identifier
            ActionTaskDescriptor descriptor = ActionTaskManager.GetDescriptor(identifier);

            // Use the descriptor to create the instance
            ActionTask task = descriptor.CreateInstance();

            // Initialize
            task.Initialize(taskSettings);

            return task;
        }

        /// <summary>
        /// Instantiate an existing task instance
        /// </summary>
        public static ActionTask InstantiateTask(ActionTaskEntity taskEntity)
        {
            // We need the descriptor for this identifier
            ActionTaskDescriptor descriptor = ActionTaskManager.GetDescriptor(taskEntity.TaskIdentifier);

            // Use the descriptor to create the instance
            ActionTask task = descriptor.CreateInstance();

            // Initialize
            task.Initialize(taskEntity);

            return task;
        }

        /// <summary>
        /// Load the trigger object for the action
        /// </summary>
        public static ActionTrigger LoadTrigger(ActionEntity action)
        {
            return ActionTriggerFactory.CreateTrigger((ActionTriggerType) action.TriggerType, action.TriggerSettings);
        }

        /// <summary>
        /// Load all the tasks for the action from the database
        /// </summary>
        public static List<ActionTask> LoadTasks(ActionEntity action)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (action.IsNew)
            {
                return tasks;
            }

            foreach (ActionTaskEntity taskEntity in ActionTaskCollection.Fetch(SqlAdapter.Default, ActionTaskFields.ActionID == action.ActionID))
            {
                ActionTask task = InstantiateTask(taskEntity);
                tasks.Add(task);
            }

            return tasks.OrderBy(t => t.Entity.StepIndex).ToList();
        }

        /// <summary>
        /// Delete the given action
        /// </summary>
        public static void DeleteAction(ActionEntity action)
        {
            // We have to load all the tasks, they'll need deleted
            List<ActionTask> tasks = LoadTasks(action);

            // We have to give the trigger a chance to cleanup its state too
            ActionTrigger trigger = LoadTrigger(action);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Delete all the tasks
                foreach (ActionTask task in tasks)
                {
                    task.Delete(adapter);
                }

                // Cleanup the trigger state
                trigger.DeleteExtraState(action, adapter);

                // Finally delete the action entity itself
                adapter.DeleteEntity(action);

                // Commit transaction
                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete all actions that are specfic to the given store.  Only actions that are configured to run ONLY for the given store will be deleted.
        /// </summary>
        public static void DeleteStoreActions(long storeID)
        {
            // Ensure we have them all in memory
            CheckForChangesNeeded();

            foreach (ActionEntity action in Actions.ToList())
            {
                if (action.StoreLimitedSingleStoreID == storeID)
                {
                    log.InfoFormat("Deleting action '{0}' since its specific to store {0}.", action.Name, storeID);

                    DeleteAction(action);
                }
            }

            // Do another check in case we deleted any
            CheckForChangesNeeded();
        }

        /// <summary>
        /// Saves the given ActionEntity.  Does not save the trigger or tasks.  The purpose of using this method is to
        /// get the proper exception translations.
        /// </summary>
        public static void SaveAction(ActionEntity action, SqlAdapter adapter)
        {
            try
            {
                adapter.SaveAndRefetch(action);

                CheckForChangesNeeded();
            }
            catch (ORMConcurrencyException ex)
            {
                throw new ActionConcurrencyException("Another user has recently made changes.\n\n" +
                                                     "Your changes cannot be saved since they would overwrite the other changes.", ex);
            }
        }

        /// <summary>
        /// Get the summar to display for the given list of tasks
        /// </summary>
        public static string GetTaskSummary(List<ActionTask> tasks)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ActionTask task in tasks)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                var binding = ActionTaskManager.GetBinding(task);

                sb.Append(binding.Identifier);

                if (binding.StoreTypeCode != null)
                {
                    sb.AppendFormat(":{0}", (int) binding.StoreTypeCode);
                }

                if (binding.StoreID != null)
                {
                    sb.AppendFormat(":{0}", (long) binding.StoreID);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the ActionQueueType for this process based on if it's background or not
        /// </summary>
        public static ActionQueueType ExecutionModeActionQueueType
        {
            get
            {
                ActionQueueType actionQueueType = ActionQueueType.Scheduled;

                if (Program.ExecutionMode.IsUIDisplayed)
                {
                    actionQueueType = ActionQueueType.UserInterface;
                }

                return actionQueueType;
            }
        }
    }
}
