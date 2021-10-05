using System;
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
}