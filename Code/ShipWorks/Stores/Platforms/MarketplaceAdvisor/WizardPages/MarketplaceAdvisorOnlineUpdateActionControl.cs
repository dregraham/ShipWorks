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
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    /// <summary>
    /// Control for creating online update action tasks for the MA add store wizard
    /// </summary>
    public partial class MarketplaceAdvisorOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the UI based on the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            MarketplaceAdvisorStoreEntity maStore = (MarketplaceAdvisorStoreEntity) store;

            // Order workflow\promotion only applies to OMS
            promote.Visible = (maStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS);
        }

        /// <summary>
        /// Create tasks for the given store based on user selection
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (shipmentUpdate.Checked)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(MarketplaceAdvisorShipmentUploadTask), store).CreateInstance());
            }

            if (promote.Checked && promote.Visible)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(MarketplaceAdvisorPromoteOrderTask), store).CreateInstance());
            }

            return tasks;
        }
    }
}
