using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;
using System;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace ShipWorks.Shipping.UI.Carriers.Postal.Usps
{
    /// <summary>
    /// Displays information to the user as to why a domestic label printed for their
    /// Global Post shipment.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Postal.Usps.IGlobalPostLabelNotification" />
    public class GlobalPostLabelNotification : IGlobalPostLabelNotification
    {
        private readonly Func<string, IDialog> browserFactory;
        private readonly IDismissableWebBrowserDlgViewModel browserViewModel;
        private readonly IUserSession userSession;
        private readonly IWin32Window owner;

        private const string DisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3782";
        private const string MoreInfoUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
        private const string BrowserDlgTitle = "Your GlobalPost Label";

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalPostLabelNotification"/> class.
        /// </summary>
        public GlobalPostLabelNotification(Func<string, IDialog> browserFactory,
            IDismissableWebBrowserDlgViewModel browserViewModel,
            IUserSession userSession,
            IWin32Window owner)
        {
            this.browserFactory = browserFactory;
            this.browserViewModel = browserViewModel;
            this.userSession = userSession;
            this.owner = owner;
        }

        /// <summary>
        /// Check to see if we should show the notification based on the current user. If the user
        /// dismisses the notification, don't show again. If not, show once a day.
        /// </summary>
        public bool AppliesToCurrentUser() =>
            userSession.User.Settings.NextGlobalPostNotificationDate < DateTime.UtcNow;

        /// <summary>
        /// Show the notification and save result
        /// </summary>
        public void Show()
        {
            Uri displayUri = new Uri(DisplayUrl);
            browserViewModel.Load(displayUri, BrowserDlgTitle, MoreInfoUrl);

            IDialog webBrowserDlg = browserFactory("DismissableWebBrowserDlg");
            webBrowserDlg.LoadOwner(owner);
            webBrowserDlg.Height = 680;
            webBrowserDlg.Width = 1300;
            webBrowserDlg.DataContext = browserViewModel;

            webBrowserDlg.ShowDialog();

            // As per SDC mockups, if the user does not dismiss the dialog, show them again after a day
            userSession.User.Settings.NextGlobalPostNotificationDate = ((IDismissableWebBrowserDlgViewModel) webBrowserDlg.DataContext).Dismissed ?
                SqlDateTime.MaxValue.Value :
                DateTime.UtcNow.AddDays(1);
        }
    }
}