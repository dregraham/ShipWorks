using System;
using System.Reflection;
using System.Windows;
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
        private readonly IWebBrowserFactory webBrowserFactory;
        private ICustomerLicense license;
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePlanDlgViewModel"/> class.
        /// </summary>
        public UpgradePlanDlgViewModel(IWebBrowserFactory webBrowserFactory)
        {
            this.webBrowserFactory = webBrowserFactory;
        }

        /// <summary>
        /// Message to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message { get; set; }

        /// <summary>
        /// Command for clicking Upgrade Plan
        /// </summary>
        public RelayCommand<Window> UpgradePlanClickCommand
        {
            get
            {
                return new RelayCommand<Window>(UpgradeAccount);
            }
        }

        /// <summary>
        /// Loads the message to be displayed
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="customerLicense"></param>
        public void Load(string message, ICustomerLicense customerLicense)
        {
            Message = message;
            license = customerLicense;
        }

        /// <summary>
        /// Clicking Upgrade Plan opens the browswer dlg with the upgrade url
        /// </summary>
        private void UpgradeAccount(Window owner)
        {
            Uri uri = new Uri("https://www.interapptive.com/account/changeplan.php");
            IDialog browserDlg = webBrowserFactory.Create(uri, "Upgrade your account", owner);
            browserDlg.ShowDialog();

            if (!license.IsOverChannelLimit)
            {
                owner?.Close();
            }
        }
    }
}