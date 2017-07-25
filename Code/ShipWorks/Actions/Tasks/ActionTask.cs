using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Base for all action tasks in ShipWorks
    /// </summary>
    public abstract class ActionTask : SerializableObject
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionTask));

        ActionTaskEntity taskEntity;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ActionTask()
        {

        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public abstract ActionTaskEditor CreateEditor();

        /// <summary>
        /// Run the task.
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        public virtual void Run(List<long> inputKeys, ActionStepContext context)
        {
            Run(inputKeys);
        }

        /// <summary>
        /// Run the task for tasks that require input but don't need the context
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        protected virtual void Run(List<long> inputKeys)
        {
            Run();
        }

        /// <summary>
        /// Run the task for tasks that don't require any input or context
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        protected virtual void Run()
        {

        }

        /// <summary>
        /// Run the task.
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        public virtual Task RunAsync(List<long> inputKeys, ActionStepContext context) => RunAsync(inputKeys);

        /// <summary>
        /// Run the task for tasks that require input but don't need the context
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        protected virtual Task RunAsync(List<long> inputKeys) => RunAsync();

        /// <summary>
        /// Run the task for tasks that don't require any input or context
        ///
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        ///
        /// </summary>
        protected virtual Task RunAsync() => Task.CompletedTask;

        /// <summary>
        /// Commit the task.
        ///
        /// This should not perform any long-running operations, but should simply save to the database.
        ///
        /// </summary>
        public virtual async Task Commit(List<long> inputKeys, ActionStepContext context)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                await context.CommitWork.CommitAsync(adapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// The underlying database entity this task represents.  This can be null when the task has failed and is being re-executed.  In that
        /// case there is no underlying entity, just the raw task settings.
        /// </summary>
        public ActionTaskEntity Entity => taskEntity;

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public virtual ActionTaskInputRequirement InputRequirement =>
            ActionTaskInputRequirement.Required;

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public virtual bool IsAsync => false;

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public virtual string InputLabel
        {
            get
            {
                if (InputRequirement != ActionTaskInputRequirement.None)
                {
                    throw new NotImplementedException("Must be overridden by derived class when input is required or optional.");
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// If the task operates on only one type of input, this specified what type that is.
        /// </summary>
        public virtual EntityType? InputEntityType => null;

        /// <summary>
        /// Indicates if the task directly or indirectly reads and makes decisions based on the contents of filters during its execution.
        /// It is important to return true if a task does so that the action engine can ensure filters are up to date.
        /// </summary>
        public virtual bool ReadsFilterContents => false;

        /// <summary>
        /// The entity has been loaded and this task instance should load its settings.
        /// </summary>
        public void Initialize(ActionTaskEntity taskEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(taskEntity, nameof(taskEntity));

            if (this.taskEntity != null)
            {
                throw new InvalidOperationException("Already been initialized");
            }

            this.taskEntity = taskEntity;

            if (ActionTaskManager.GetDescriptor(GetType()).Identifier != taskEntity.TaskIdentifier)
            {
                throw new InvalidOperationException("Trying to initialize the wrong type task for " + taskEntity.TaskIdentifier);
            }

            DeserializeXml(taskEntity.TaskSettings);
        }

        /// <summary>
        /// Load the settings from the task from the given xml settings.  The task object will not be able to be saved or deleted.  The use
        /// of this initialization is for the specific case where the task needs to be recreated to retry a failed task at a later date.
        /// </summary>
        public void Initialize(string taskSettings)
        {
            DeserializeXml(taskSettings);
        }

        /// <summary>
        /// Save the task settings to the entity, and persist any other task data to the database.
        /// </summary>
        public void Save(ActionEntity action, SqlAdapter adapter)
        {
            if (taskEntity == null)
            {
                throw new InvalidOperationException("Attempt to save ActionTask that has no backing entity.");
            }

            // If the task is new, we have to save it right away to get its PK.  The "SaveExtraState" can sometimes
            // potentially need to use its PK.
            bool isNew = taskEntity.IsNew;
            if (taskEntity.IsNew)
            {
                taskEntity.ActionID = action.ActionID;
                adapter.SaveAndRefetch(taskEntity);
            }

            SaveFilterReferences(action, isNew);

            SaveExtraState(action, adapter);

            // After we save the extra state (which could effect the settings), now we can serialize the settings and save the entity.
            taskEntity.TaskSettings = SerializeXml("Settings");
            adapter.SaveAndRefetch(taskEntity);
        }

        /// <summary>
        /// Save the references we have to filters from the run condition and data source
        /// </summary>
        private void SaveFilterReferences(ActionEntity action, bool isNew)
        {
            long originalConditionID = 0;
            long originalInputID = 0;

            if ((bool) taskEntity.Fields[ActionTaskFields.FilterCondition.FieldIndex].DbValue)
            {
                originalConditionID = (long) taskEntity.Fields[ActionTaskFields.FilterConditionNodeID.FieldIndex].DbValue;
            }

            if ((int) taskEntity.Fields[ActionTaskFields.InputSource.FieldIndex].DbValue == (int) ActionTaskInputSource.FilterContents)
            {
                originalInputID = (long) taskEntity.Fields[ActionTaskFields.InputFilterNodeID.FieldIndex].DbValue;
            }

            long newConditionID = taskEntity.FilterCondition ? taskEntity.FilterConditionNodeID : 0;
            long newInputID = (taskEntity.InputSource == (int) ActionTaskInputSource.FilterContents) ? taskEntity.InputFilterNodeID : 0;

            UpdateObjectReferenceIfNecessary(action, isNew, newConditionID, originalConditionID, "ActionTaskCondition");
            UpdateObjectReferenceIfNecessary(action, isNew, newInputID, originalInputID, "ActionTaskInput");
        }

        /// <summary>
        /// Update the object reference, if necessary
        /// </summary>
        private void UpdateObjectReferenceIfNecessary(ActionEntity action, bool isNew, long newId, long originalId, string description)
        {
            if (!isNew && newId == originalId)
            {
                return;
            }

            if (newId > 0)
            {
                ObjectReferenceManager.SetReference(
                    taskEntity.ActionTaskID,
                    description,
                    newId,
                    GetObjectReferenceReason(action));
            }
            else
            {
                ObjectReferenceManager.ClearReference(taskEntity.ActionTaskID, description);
            }
        }

        /// <summary>
        /// Get the reason to use when registring an object with the ObjectReferenceManager
        /// </summary>
        protected string GetObjectReferenceReason(ActionEntity action)
        {
            return string.Format("'{0}' task for action '{1}'", ActionTaskManager.GetDescriptor(taskEntity.TaskIdentifier).BaseName, action.Name);
        }

        /// <summary>
        /// Some tasks may have extra database state they need to save in addition to what get's persisted in the XML settings.
        /// Such as the sound file resource for the PlaySound task.  This gives the task a chance to save it.
        /// </summary>
        protected virtual void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {

        }

        /// <summary>
        /// The entity is being deleted so cleanup any database resources used.
        /// </summary>
        public void Delete(SqlAdapter adapter)
        {
            if (taskEntity == null)
            {
                throw new InvalidOperationException("Attempt to delete ActionTask that has no backing entity.");
            }

            DeleteFilterReferences();

            DeleteExtraState();

            adapter.DeleteEntity(taskEntity);
        }

        /// <summary>
        /// Delete the filter references we have due to the run condition and data source
        /// </summary>
        private void DeleteFilterReferences()
        {
            ObjectReferenceManager.ClearReference(taskEntity.ActionTaskID, "ActionTaskCondition");
            ObjectReferenceManager.ClearReference(taskEntity.ActionTaskID, "ActionTaskInput");
        }

        /// <summary>
        /// The task is being deleted.  Any database extra state saved by the task in SaveExtraState should be cleaned up.
        /// </summary>
        protected virtual void DeleteExtraState()
        {

        }

        /// <summary>
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="triggerType">Type of trigger that should be tested</param>
        /// <returns></returns>
        public virtual bool IsAllowedForTrigger(ActionTriggerType triggerType)
        {
            return true;
        }
    }
}
