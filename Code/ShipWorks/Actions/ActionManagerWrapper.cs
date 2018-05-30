using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Manages and provides access to the actions in the system
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 1)]
    [Component(RegistrationType.SpecificService, Service = typeof(IActionManager))]
    public class ActionManagerWrapper : IActionManager, IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        /// <summary>
        /// Initialize table synchronizer
        /// </summary>
        public void InitializeForCurrentSession() => ActionManager.InitializeForCurrentSession();

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public void CheckForChangesNeeded() => ActionManager.CheckForChangesNeeded();

        /// <summary>
        /// Get the current list of all actions
        /// </summary>
        public IList<ActionEntity> Actions => ActionManager.Actions;

        /// <summary>
        /// Load all the tasks for the action from the database
        /// </summary>
        public List<ActionTask> LoadTasks(ILifetimeScope lifetimeScope, ActionEntity action) => 
            ActionManager.LoadTasks(lifetimeScope, action);

        /// <summary>
        /// Load the trigger object for the action
        /// </summary>
        public ActionTrigger LoadTrigger(ActionEntity action) => ActionManager.LoadTrigger(action);

        /// <summary>
        /// Saves the given ActionEntity.  Does not save the trigger or tasks.  The purpose of using this method is to
        /// get the proper exception translations.
        /// </summary>
        public void SaveAction(ActionEntity action, ISqlAdapter adapter) => ActionManager.SaveAction(action, adapter);

        /// <summary>
        /// Instantiate an existing task instance
        /// </summary>
        public ActionTask InstantiateTask(ILifetimeScope lifetimeScope, ActionTaskEntity taskEntity) =>
            ActionManager.InstantiateTask(lifetimeScope, taskEntity);
    }
}
