using System;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls.WebBrowser;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    /// <summary>
    /// ViewModel for UpgradePlanDlg
    /// </summary>
    public class UpgradePlanDlgViewModel : IUpgradePlanDlgViewModel
    {
        private readonly WebBrowserFactory webBrowserFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePlanDlgViewModel"/> class.
        /// </summary>
        public UpgradePlanDlgViewModel(WebBrowserFactory webBrowserFactory)
        {
            this.webBrowserFactory = webBrowserFactory;
        }

        /// <summary>
        /// Message to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message => "To access this feature, you must upgrade your plan";

        /// <summary>
        /// Command for clicking Upgrade Plan
        /// </summary>
        public RelayCommand UpgradePlanClickCommand
        {
            get
            {
                return new RelayCommand(UpgradeAccount);
            }
        }

        /// <summary>
        /// Clicking Upgrade Plan opens the browswer dlg with the upgrade url
        /// </summary>
        private void UpgradeAccount()
        {
            Uri uri = new Uri("https://www.interapptive.com/account/changeplan.php");
            IDialog browserDlg = webBrowserFactory.Create(uri, "Upgrade your account");
            browserDlg.ShowDialog();
        }
    }
}