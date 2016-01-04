using System;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Winforms element for hosting the WPF CustomerLicenseActivationControl
    /// </summary>
    public partial class CustomerLicenseActivationControlHost : WizardPage , ICustomerLicenseActivation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationControlHost(ICustomerLicenseActivationViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            Load += OnPageLoad;
        }

        /// <summary>
        /// Called when [page load].
        /// </summary>
        private void OnPageLoad(object sender, EventArgs e)
        {
            CustomerLicenseActivationControl page = new CustomerLicenseActivationControl
            {
                DataContext = ViewModel
            };

            elementHost.Child = page;
        }

        /// <summary>
        /// Called when clicking next in the setup wizard
        /// </summary>
        public GenericResult<UserEntity> Save()
        {
            return ViewModel.Save();
        }

        /// <summary>
        /// The ViewModel
        /// </summary>
        /// <remarks>
        /// Exposed so that the wizard can access the credentials
        /// which is needed log the user in after creating the user
        /// </remarks>
        public ICustomerLicenseActivationViewModel ViewModel { get; }
    }
}
