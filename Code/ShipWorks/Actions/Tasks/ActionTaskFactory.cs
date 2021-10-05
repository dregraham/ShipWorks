using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Factory to create an action task
    /// </summary>
    [Component]
    public class ActionTaskFactory : IActionTaskFactory
    {
        /// <summary>
        /// Create an Action Task
        /// </summary>
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