using System;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    public class ChannelLimitDlgFactory : IChannelLimitDlgFactory
    {
        private readonly ILicenseService licenseService;
        private readonly Func<IWin32Window, IChannelLimitDlg> dlgFactory;
        private readonly IChannelLimitViewModel viewModel;

        public ChannelLimitDlgFactory(ILicenseService licenseService, Func<IWin32Window, IChannelLimitDlg> dlgFactory, IChannelLimitViewModel viewModel)
        {
            this.licenseService = licenseService;
            this.dlgFactory = dlgFactory;
            this.viewModel = viewModel;
        }

        public IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner)
        {
            // load the customer license into the view model
            viewModel.Load(licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense);

            // Get the dialog
            IChannelLimitDlg dialog = dlgFactory(owner);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}