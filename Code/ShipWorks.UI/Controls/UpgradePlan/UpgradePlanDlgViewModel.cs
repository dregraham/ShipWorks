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
        private readonly IWebBrowserDlgViewModel webBrowserDlgViewModel;
        private readonly Func<string, IDialog> webBrowserDlgFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePlanDlgViewModel"/> class.
        /// </summary>
        /// <param name="webBrowserDlgFactory">The web browser dialog factory.</param>
        /// <param name="webBrowserDlgViewModel">The web browser dialog view model.</param>
        public UpgradePlanDlgViewModel(Func<string, IDialog> webBrowserDlgFactory, IWebBrowserDlgViewModel webBrowserDlgViewModel)
        {
            this.webBrowserDlgFactory = webBrowserDlgFactory;
            this.webBrowserDlgViewModel = webBrowserDlgViewModel;
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
            webBrowserDlgViewModel.Load(uri, "Upgrade your account");
            IDialog browserDlg = webBrowserDlgFactory("WebBrowserDlg");
            browserDlg.DataContext = webBrowserDlgViewModel;
            browserDlg.ShowDialog();
        }
    }
}