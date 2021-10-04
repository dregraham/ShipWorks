using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Factory to create an action task
    /// </summary>
    public interface IActionTaskFactory
    {
        /// <summary>
        /// Create an Action Task
        /// </summary>
        ActionTask Create(Type taskType, StoreEntity store, int stepIndex);
    }

    public class ActionTaskFactory : IActionTaskFactory
    {
        public ActionTask Create(Type taskType, StoreEntity store, int stepIndex)
        {
            using (var scope = IoC.UnsafeGlobalLifetimeScope)
            {
                var actionTask =  new ActionTaskDescriptorBinding(taskType, store, scope)
                    .CreateInstance(scope);
                actionTask.Entity.StepIndex = stepIndex;

                return actionTask;
            }
        }
    }
}