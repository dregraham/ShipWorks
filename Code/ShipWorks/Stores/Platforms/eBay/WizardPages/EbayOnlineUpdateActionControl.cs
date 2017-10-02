using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
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
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            if (markShipped.Checked || markPaid.Checked)
            {
                EbayOnlineUpdateTask task = (EbayOnlineUpdateTask) new ActionTaskDescriptorBinding(typeof(EbayOnlineUpdateTask), store).CreateInstance(lifetimeScope);
                task.MarkShipped = markShipped.Checked;
                task.MarkPaid = markPaid.Checked;

                return new List<ActionTask> { task };
            }

            return null;
        }
    }
}
