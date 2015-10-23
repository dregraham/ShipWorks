using System.Collections.Generic;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
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
                return new List<ActionTask> { new ActionTaskDescriptorBinding(typeof(YahooEmailShipmentUploadTask), store).CreateInstance() };
            }

            return null;
        }
    }
}
