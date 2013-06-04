using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;

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
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (createTask.Checked)
            {
                return new List<ActionTask> { new ActionTaskDescriptorBinding(taskType, store).CreateInstance() };
            }
            else
            {
                return null;
            }
        }
    }
}
