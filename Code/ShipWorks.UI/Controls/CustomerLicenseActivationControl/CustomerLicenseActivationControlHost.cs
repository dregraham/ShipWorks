using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Wizard;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Winforms element for hosting the WPF Yahoo Account Settings Page
    /// </summary>
    public partial class CustomerLicenseActivationControlHost : WizardPage
    {
        private CustomerLicenseActivationViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TangoUserControlHost"/> class.
        /// </summary>
        public CustomerLicenseActivationControlHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
            StepNext += OnStepNext;
        }

        /// <summary>
        /// Called when [page load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnPageLoad(object sender, EventArgs e)
        {
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<CustomerLicenseActivationViewModel>();

            CustomerLicenseActivationControl page = new CustomerLicenseActivationControl
            {
                DataContext = viewModel
            };

            elementHost.Child = page;
        }

        /// <summary>
        /// Called when clicking next in the setup wizard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            string message = viewModel.Save();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            MessageHelper.ShowError(this, message);
            e.NextPage = this;
        }
    }
}
