using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Winforms element for hosting the WPF Yahoo Account Settings Page
    /// </summary>
    public partial class CustomerLicenseActivationControlHost : WizardPage
    {
        private CustomerLicenseActivationViewModel viewModel;
        private readonly SqlSession sqlSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="TangoUserControlHost"/> class.
        /// </summary>
        public CustomerLicenseActivationControlHost(CustomerLicenseActivationViewModel viewModel, SqlSession sqlSession)
        {
            this.viewModel = viewModel;
            this.sqlSession = sqlSession;

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
            string message = viewModel.Save(sqlSession);
            
            sqlSession.SaveAsCurrent();

            // Initialize the session
            UserSession.InitializeForCurrentDatabase();

            // Logon the user - this has failed in the wild (FB 275179), so instead of crashing, we'll ask them to log in again
            if (UserSession.Logon(viewModel.Username, viewModel.Password, true))
            {
                // Initialize the session
                UserManager.InitializeForCurrentUser();
                UserSession.InitializeForCurrentSession();
            }
            else
            {
                message = "There was a problem while logging in. Please try again.";
            }

            AddStoreWizard.RunWizard(this);
            
            if (!string.IsNullOrEmpty(message))
            {
                MessageHelper.ShowError(this, message);
                e.NextPage = this;
            }
        }
    }
}
