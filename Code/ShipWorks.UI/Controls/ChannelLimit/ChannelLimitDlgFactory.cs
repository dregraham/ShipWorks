using System;
using System.Linq;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Factory that creates a ChannelLimitDlg
    /// </summary>
    public class ChannelLimitDlgFactory : IChannelLimitDlgFactory
    {
        private readonly ILicenseService licenseService;
        private readonly Func<IWin32Window, IChannelLimitDlg> dlgFactory;
        private readonly IChannelLimitViewModel viewModel;
        private readonly IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitDlgFactory"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets the channel limit dialog.
        /// </summary>
        public IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner, EditionFeature feature)
        {
            ICustomerLicense customerLicense = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;

            if (customerLicense == null)
            {
                throw new InvalidCastException("Expected a ICustomerLicense from the LicenseService");
            }

            // load the customer license into the view model
            viewModel.Load(customerLicense, behaviorFactory[feature]);

            // Get the dialog
            IChannelLimitDlg dialog = dlgFactory(owner);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}