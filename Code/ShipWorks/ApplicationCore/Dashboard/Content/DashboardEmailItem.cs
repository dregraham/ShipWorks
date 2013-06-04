using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Properties;
using System.Windows.Forms;
using ShipWorks.Email.Outlook;
using ShipWorks.Email;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Dashboard item representing a notifcation to the user that there is email to deal with
    /// </summary>
    class DashboardEmailItem : DashboardItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardEmailItem()
        {

        }
        
        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.Image = Resources.mailbox_full;
            DashboardBar.PrimaryText = "Email";

            DashboardBar.ApplyActions(new List<DashboardAction> {
                new DashboardActionMethod("[link]Open Outbox[/link]", OnOpenOutbox),
                new DashboardActionMethod("[link]Send Now[/link]", OnSendNow) });

            DashboardBar.CanUserDismiss = false;
        }

        /// <summary>
        /// Update the display based on the status of the outbox
        /// </summary>
        public void UpdateContent(int unsent, int errors)
        {
            string text = string.Format("There are {0} unsent messages in the outbox.", unsent);

            if (errors > 0)
            {
                text += string.Format(" {0} have errors.", errors);
            }

            DashboardBar.SecondaryText = text;
        }

        /// <summary>
        /// Open the outbox
        /// </summary>
        private void OnOpenOutbox(Control owner, object userState)
        {
            using (EmailOutlookDlg dlg = new EmailOutlookDlg(EmailOutlookDlg.Folder.Outbox))
            {
                dlg.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Initiate sending of email
        /// </summary>
        private void OnSendNow(Control owner, object userState)
        {
            if (EmailCommunicator.StartEmailingAccounts())
            {
                EmailCommunicator.ShowProgressDlg(owner);
            }
        }
    }
}
