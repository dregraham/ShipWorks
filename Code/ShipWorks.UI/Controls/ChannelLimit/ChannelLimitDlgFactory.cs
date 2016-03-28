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

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitDlgFactory"/> class.
        /// </summary>
        public ChannelLimitDlgFactory(ILicenseService licenseService,
            IChannelLimitViewModel viewModel,
            IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory)
        {
            this.licenseService = licenseService;
            this.viewModel = viewModel;
            this.behaviorFactory = behaviorFactory;
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
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                // Get the dialog
                IDialog dialog = scope.ResolveNamed<IDialog>("ChannelLimitDlg", new TypedParameter(typeof(IWin32Window), owner));
                dialog.DataContext = viewModel;

                return dialog;
            }
        }
    }
}