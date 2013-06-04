using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Properties;
using System.Windows.Forms;
using ShipWorks.Email.Outlook;
using ShipWorks.Email;
using ShipWorks.Actions;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Dashboard item representing a notifcation to the user that there is an action error to deal with
    /// </summary>
    class DashboardActionErrorItem : DashboardItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardActionErrorItem()
        {

        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.Image = Resources.gear_error16;
            DashboardBar.PrimaryText = "Actions";

            DashboardBar.ApplyActions(new List<DashboardAction> { new DashboardActionMethod("[link]View Errors[/link]", OnViewErrors) });

            DashboardBar.CanUserDismiss = false;
        }

        /// <summary>
        /// Update the display based on action task status
        /// </summary>
        public void UpdateContent(int errors)
        {
            string text = string.Format("There are {0} tasks with errors.", errors);

            DashboardBar.SecondaryText = text;
        }

        /// <summary>
        /// Open the outbox
        /// </summary>
        private void OnViewErrors(Control owner, object userState)
        {
            using (ActionErrorDlg dlg = new ActionErrorDlg())
            {
                dlg.ShowDialog(owner);
            }
        }
    }
}
