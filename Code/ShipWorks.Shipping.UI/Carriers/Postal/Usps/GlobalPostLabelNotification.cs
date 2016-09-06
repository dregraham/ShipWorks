using System;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;

namespace ShipWorks.Shipping.UI.Carriers.Postal.Usps
{
    public class GlobalPostLabelNotification : IGlobalPostLabelNotification
    {
        private readonly IUserSession userSession;
        private readonly IDismissableWebBrowserDlgViewModel browserViewModel;

        public GlobalPostLabelNotification(IUserSession userSession, IDismissableWebBrowserDlgViewModel browserViewModel)
        {
            this.userSession = userSession;
            this.browserViewModel = browserViewModel;
        }

        /// <summary>
        /// Check to see if we should show the notification based on the current user session
        /// </summary>
        /// <returns></returns>
        public bool AppliesToSession() =>
            userSession.User.NextGlobalPostNotificationDate.ToUniversalTime() < DateTime.UtcNow;

        /// <summary>
        /// Show the notification and save result
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Show()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/3782");
            string moreInfoLink = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
            browserViewModel.Load(displayUri, "Your GlobalPost Label", moreInfoLink);

            DismissableWebBrowserDlg webBrowserDlg = new DismissableWebBrowserDlg();
            webBrowserDlg.DataContext = browserViewModel;

            webBrowserDlg.ShowDialog();

            if (((IDismissableWebBrowserDlgViewModel) webBrowserDlg.DataContext).Dissmissed)
            {
                userSession.User.NextGlobalPostNotificationDate = DateTime.UtcNow.AddYears(200);
            }
        }
    }
}