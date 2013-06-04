using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    /// <summary>
    /// UserControl for creation of store wizard online update tasks
    /// </summary>
    public partial class EbayOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the tasks configured in the control
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (markShipped.Checked || markPaid.Checked)
            {
                EbayOnlineUpdateTask task = (EbayOnlineUpdateTask) new ActionTaskDescriptorBinding(typeof(EbayOnlineUpdateTask), store).CreateInstance();
                task.MarkShipped = markShipped.Checked;
                task.MarkPaid = markPaid.Checked;

                return new List<ActionTask> { task };
            }

            return null;
        }
    }
}
