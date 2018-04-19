using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityInterfaces;
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
        private readonly Func<IDismissableWebBrowserDlg> browserFactory;
        private readonly IDismissableWebBrowserDlgViewModel browserViewModel;
        private readonly IUserSession userSession;
        private readonly IWin32Window owner;

        readonly ICurrentUserSettings userSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalPostLabelNotification"/> class.
        /// </summary>
        public GlobalPostLabelNotification(
            Func<IDismissableWebBrowserDlg> browserFactory,
            IDismissableWebBrowserDlgViewModel browserViewModel,
            ICurrentUserSettings userSettings,
            IUserSession userSession,
            IWin32Window owner)
        {
            this.userSettings = userSettings;
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
        public void Show(IShipmentEntity shipment)
        {
            if (userSession.User.Settings.NextGlobalPostNotificationDate >= DateTime.UtcNow)
            {
                return;
            }

            var assets = new GlobalPostDialogDetails(shipment);

            if (!userSettings.ShouldShowNotification(assets.NotificationType))
            {
                return;
            }

            browserViewModel.Load(assets.DisplayUrl, assets.Title, assets.MoreInfoLink);

            IDismissableWebBrowserDlg webBrowserDlg = browserFactory();
            webBrowserDlg.LoadOwner(owner);
            webBrowserDlg.Height = 680;
            webBrowserDlg.Width = 1300;
            webBrowserDlg.DataContext = browserViewModel;

            webBrowserDlg.ShowDialog();

            // As per SDC mockups, if the user does not dismiss the dialog, show them again after a day
            if (browserViewModel.Dismissed)
            {
                userSettings.StopShowingNotification(assets.NotificationType);
            }

            userSession.User.Settings.NextGlobalPostNotificationDate = DateTime.UtcNow.AddDays(1);
        }
    }
}