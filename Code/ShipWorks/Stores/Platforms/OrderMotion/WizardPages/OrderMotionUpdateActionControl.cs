using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.OrderMotion.WizardPages
{
    /// <summary>
    /// Control for configuring actions to be created during the Add Store wizard.
    /// </summary>
    public partial class OrderMotionOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the tasks that were configured on the control
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            if (createTask.Checked)
            {
                OrderMotionShipmentUploadTask task = (OrderMotionShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(OrderMotionShipmentUploadTask), store).CreateInstance(lifetimeScope);

                return new List<ActionTask> { task };
            }

            return null;
        }
    }
}
