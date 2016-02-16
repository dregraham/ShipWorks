using System;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    /// <summary>
    /// Creates an UpgradePlanDlg
    /// </summary>
    public class UpgradePlanDlgFactory : IUpgradePlanDlgFactory
    {
        private readonly Func<string, IWin32Window, IDialog> dialogFactory;
        private readonly IUpgradePlanDlgViewModel viewModel;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradePlanDlgFactory"/> class.
        /// </summary>
        public UpgradePlanDlgFactory(Func<string, IWin32Window, IDialog> dialogFactory, IUpgradePlanDlgViewModel viewModel, ILicenseService licenseService)
        {
            this.dialogFactory = dialogFactory;
            this.viewModel = viewModel;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Creates an UpgradePlanDlg with the following message.
        /// </summary>
        public IDialog Create(string message, IWin32Window owner)
        {
            viewModel.Load(message, licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense);
            IDialog upgradePlanDlg = dialogFactory("UpgradePlanDlg", owner);
            upgradePlanDlg.DataContext = viewModel;

            return upgradePlanDlg;
        }
    }
}