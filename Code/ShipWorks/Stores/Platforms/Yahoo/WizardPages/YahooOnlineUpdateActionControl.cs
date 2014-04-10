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
using ShipWorks.Stores.Platforms.Yahoo.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Yahoo.WizardPages
{
    /// <summary>
    /// Control for creating online update action tasks for the yahoo add store wizard
    /// </summary>
    public partial class YahooOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the UI for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            YahooStoreEntity yahoo = (YahooStoreEntity) store;

            statusUpdate.Enabled = yahoo.TrackingUpdatePassword.Length > 0;
            labelDisabled.Visible = yahoo.TrackingUpdatePassword.Length == 0;

            if (yahoo.TrackingUpdatePassword.Length == 0)
            {
                statusUpdate.Checked = false;
            }

            Height = labelDisabled.Visible ? labelDisabled.Bottom + 4 : statusUpdate.Bottom;
        }

        /// <summary>
        /// Create the tasks selected by the user
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (statusUpdate.Checked)
            {
                return new List<ActionTask> { new ActionTaskDescriptorBinding(typeof(YahooShipmentUploadTask), store).CreateInstance() };
            }

            return null;
        }
    }
}
