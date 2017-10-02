using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Control that can be used by any store type that provides "Basic" automatic online update options, that just consist
    /// of a single action.
    /// </summary>
    public partial class OnlineUpdateShipmentUpdateActionControl : OnlineUpdateActionControlBase
    {
        Type taskType;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateShipmentUpdateActionControl(Type taskType)
        {
            InitializeComponent();

            if (taskType == null)
            {
                throw new ArgumentNullException("taskType");
            }

            this.taskType = taskType;
        }

        /// <summary>
        /// Create the configured tasks
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            if (createTask.Checked)
            {
                return new List<ActionTask> { new ActionTaskDescriptorBinding(taskType, store).CreateInstance(lifetimeScope) };
            }
            else
            {
                return null;
            }
        }
    }
}
