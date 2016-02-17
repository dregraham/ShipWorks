using System;
using System.Linq;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    public class ChannelLimitDlgFactory : IChannelLimitDlgFactory
    {
        private readonly ILicenseService licenseService;
        private readonly Func<IWin32Window, IChannelLimitDlg> dlgFactory;
        private readonly IChannelLimitViewModel viewModel;
        private readonly IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory;

        public ChannelLimitDlgFactory(ILicenseService licenseService,
            Func<IWin32Window, IChannelLimitDlg> dlgFactory,
            IChannelLimitViewModel viewModel,
            IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory)
        {
            this.licenseService = licenseService;
            this.dlgFactory = dlgFactory;
            this.viewModel = viewModel;
            this.behaviorFactory = behaviorFactory;
        }

        public IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner, EditionFeature feature)
        {
            // load the customer license into the view model
            viewModel.Load(licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense, behaviorFactory[feature]);

            // Get the dialog
            IChannelLimitDlg dialog = dlgFactory(owner);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}