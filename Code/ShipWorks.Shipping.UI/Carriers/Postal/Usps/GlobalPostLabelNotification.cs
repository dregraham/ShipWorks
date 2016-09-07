using System;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;

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

        private const string DisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3782";
        private const string MoreInfoUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
        private const string BrowserDlgTitle = "Your GlobalPost Label";

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalPostLabelNotification"/> class.
        /// </summary>
        /// <param name="browserFactory"></param>
        /// <param name="browserViewModel">The browser view model.</param>
        /// <param name="userSession">The user session.</param>
        public GlobalPostLabelNotification(Func<string, IDialog> browserFactory, IDismissableWebBrowserDlgViewModel browserViewModel, IUserSession userSession)
        {
            this.browserFactory = browserFactory;
            this.browserViewModel = browserViewModel;
            this.userSession = userSession;
        }

        /// <summary>
        /// Check to see if we should show the notification based on the current user session
        /// </summary>
        public bool AppliesToSession() =>
            userSession.User.NextGlobalPostNotificationDate.ToUniversalTime() < DateTime.UtcNow;

        /// <summary>
        /// Show the notification and save result
        /// </summary>
        public void Show()
        {
            Uri displayUri = new Uri(DisplayUrl);
            browserViewModel.Load(displayUri, BrowserDlgTitle, MoreInfoUrl);

            IDialog webBrowserDlg = browserFactory("DismissableWebBrowserDlg");
            webBrowserDlg.DataContext = browserViewModel;

            webBrowserDlg.ShowDialog();

            // As per SDC mockups, if the user does not dismiss the dialog, show them again after a day
            userSession.User.NextGlobalPostNotificationDate = ((IDismissableWebBrowserDlgViewModel) webBrowserDlg.DataContext).Dismissed ?
                DateTime.UtcNow.AddYears(200) :
                DateTime.UtcNow.AddDays(1);
        }
    }
}