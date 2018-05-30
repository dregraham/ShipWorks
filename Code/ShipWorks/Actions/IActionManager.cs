using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Manages and provides access to the actions in the system
    /// </summary>
    public interface IActionManager
    {
        /// <summary>
        /// Get the current list of all actions
        /// </summary>
        IList<ActionEntity> Actions { get; }

        /// <summary>
        /// Load all the tasks for the action from the database
        /// </summary>
        List<ActionTask> LoadTasks(ILifetimeScope lifetimeScope, ActionEntity action);

        /// <summary>
        /// Load the trigger object for the action
        /// </summary>
        ActionTrigger LoadTrigger(ActionEntity action);

        /// <summary>
        /// Saves the given ActionEntity.  Does not save the trigger or tasks.  The purpose of using this method is to
        /// get the proper exception translations.
        /// </summary>
        void SaveAction(ActionEntity action, ISqlAdapter adapter);

        /// <summary>
        /// Instantiate an existing task instance
        /// </summary>
        ActionTask InstantiateTask(ILifetimeScope lifetimeScope, ActionTaskEntity taskEntity);
    }
}
