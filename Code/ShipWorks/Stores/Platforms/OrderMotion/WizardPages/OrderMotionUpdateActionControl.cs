using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
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
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (createTask.Checked)
            {
                OrderMotionShipmentUploadTask task = (OrderMotionShipmentUploadTask)new ActionTaskDescriptorBinding(typeof(OrderMotionShipmentUploadTask), store).CreateInstance();

                return new List<ActionTask> { task };
            }

            return null;
        }
    }
}
