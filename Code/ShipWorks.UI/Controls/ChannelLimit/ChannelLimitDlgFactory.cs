using System;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Factory that creates a ChannelLimitDlg
    /// </summary>
    public class ChannelLimitDlgFactory : IChannelLimitDlgFactory
    {
        private readonly ILicenseService licenseService;
        private readonly IChannelLimitViewModel viewModel;
        private readonly IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory;
        private readonly Func<string, IDialog> dialogFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitDlgFactory"/> class.
        /// </summary>
        public ChannelLimitDlgFactory(ILicenseService licenseService,
            IChannelLimitViewModel viewModel,
            IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory,
            Func<string, IDialog> dialogFactory)
        {
            this.licenseService = licenseService;
            this.viewModel = viewModel;
            this.behaviorFactory = behaviorFactory;
            this.dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Gets the channel limit dialog.
        /// </summary>
        public IDialog GetChannelLimitDlg(IWin32Window owner, EditionFeature feature, EnforcementContext context)
        {
            ICustomerLicense customerLicense = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;

            if (customerLicense == null)
            {
                throw new InvalidCastException("Expected a ICustomerLicense from the LicenseService");
            }

            viewModel.EnforcementContext = context;
            // load the customer license into the view model
            viewModel.Load(customerLicense, behaviorFactory[feature]);

            // Get the dialog
            IDialog dialog = dialogFactory("ChannelLimitDlg");
            dialog.LoadOwner(owner);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}