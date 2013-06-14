﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Actions.Triggers;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters;
using log4net;
using System.Transactions;
using ShipWorks.Data;
using ShipWorks.Actions.Scheduling;

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
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (tableSynchronizer.Synchronize())
                    {
                        tableSynchronizer.EntityCollection.Sort((int) ActionFieldIndex.Name, ListSortDirection.Ascending);
                    }
                }

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
            return Actions.Where(a => a.ActionID == actionID).SingleOrDefault();
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

            if (trigger.TriggerType == ActionTriggerType.Cron)
            {
                // We need to unschedule the scheudled action
                new Scheduler().UnscheduleAction(action, trigger as CronTrigger);
            }

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
                // If we are a scheduled action, we need to do additional checking before trying to save.
                if (action.TriggerType == (int) ActionTriggerType.Cron)
                {
                    // Get the cron trigger for this action.
                    CronTrigger cronTrigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.Cron, action.TriggerSettings) as CronTrigger;

                    // We need to see if any field has changed (other than the Enabled field, since the user could modify it on the Action list dialog)
                    var changedFields = action.Fields.GetAsEntityFieldCoreArray().Where(f => f.FieldIndex != ActionFields.Enabled.FieldIndex && f.IsChanged);

                    bool updateJob = action.IsDirty && changedFields.Any();

                    // If we are to update the job, validate the date
                    if (updateJob)
                    {
                        // Jobs/actions cannot be scheduled to occur in the past
                        if (cronTrigger.StartDateTimeInUtc <= DateTime.UtcNow)
                        {
                            throw new SchedulingException("The start date must be in the future when scheduling a new action.");
                        }
                    }

                    // Now we can save it to the db
                    adapter.SaveAndRefetch(action);

                    // And finally schedule the action
                    if (updateJob)
                    {
                        Scheduler scheduler = new Scheduler();
                        scheduler.ScheduleAction(action, cronTrigger);
                    }
                }
                else
                {
                    // The action isn't a scheduled one, so save.
                    adapter.SaveAndRefetch(action);
                }
            }
            catch (ORMConcurrencyException ex)
            {
                throw new ActionConcurrencyException("Another user has recently made changes.\n\n" +
                                                     "Your changes cannot be saved since they would overwrite the other changes.", ex);
            }
            catch (SchedulingException schedulingException)
            {
                log.Error(string.Format("An error occurred while scheduling an action. {0}", schedulingException.Message), schedulingException);
                throw;
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

                sb.Append(ActionTaskManager.GetDescriptor(task.GetType()).Identifier);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the ActionQueueType based on UserInteractive.
        /// </summary>
        public static ActionQueueType ActionQueueType
        {
            get
            {
                ActionQueueType actionQueueType = ActionQueueType.Scheduled;

                if (Environment.UserInteractive)
                {
                    actionQueueType = ActionQueueType.UserInterface;
                }

                return actionQueueType;
            }
        }
    }
}
