using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal;
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

        // Global Post stuff 
        private const string GlobalPostDisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3782";
        private const string GlobalPostMoreInfoUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
        private const string GlobalPostBrowserDlgTitle = "Your GlobalPost Label";

        // Global Post Advantage Program stuff
        private const string GlobalPostAdvantageProgramDisplayUrl = "https://secure.la.stamps.com/img/rnt_kb_files/RNTimages/globalpost/shipworksGAP.png";
        private const string GlobalPostAdvantageProgramMoreInfoUrl = "http://support.shipworks.com/support/solutions/articles/4000114989";
        private const string GlobalPostAdvantageProgramBrowserDlgTitle = "Your First-Class International Envelope Label";

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
        public void Show(IShipmentEntity shipment)
        {
            (string urlToUse, string titleToUse, string moreInfo) = GetDialogAssets(shipment);

            Uri displayUri = new Uri(urlToUse);
            browserViewModel.Load(displayUri, titleToUse, moreInfo);

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

        /// <summary>
        /// Get the assets needed for displaying the dialog
        /// </summary>
        private static (string urlToUse, string titleToUse, string moreInfo) GetDialogAssets(IShipmentEntity shipment)
        {
            bool pureGlobalPost = PostalUtility.IsGlobalPost((PostalServiceType)shipment.Postal.Service);

            string urlToUse = pureGlobalPost ? GlobalPostDisplayUrl : GlobalPostAdvantageProgramDisplayUrl;
            string titleToUse = pureGlobalPost ? GlobalPostBrowserDlgTitle : GlobalPostAdvantageProgramBrowserDlgTitle;
            string moreInfo = pureGlobalPost ? GlobalPostMoreInfoUrl : GlobalPostAdvantageProgramMoreInfoUrl;

            return (urlToUse, titleToUse, moreInfo);
        }
    }
}