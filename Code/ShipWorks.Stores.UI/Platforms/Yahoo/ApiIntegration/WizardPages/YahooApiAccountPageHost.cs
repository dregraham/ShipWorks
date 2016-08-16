using System;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// Winforms element for hosting the WPF Yahoo Account Page
    /// </summary>
    public partial class YahooApiAccountPageHost : AddStoreWizardPage
    {
        private YahooApiAccountPageViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountPageHost"/> class.
        /// </summary>
        public YahooApiAccountPageHost()
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
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooApiAccountPageViewModel>>().Value;

            YahooApiAccountPage page = new YahooApiAccountPage
            {
                DataContext = viewModel
            };

            ControlHost.Child = page;

            YahooStoreEntity store = GetStore<YahooStoreEntity>();

            viewModel.Load(store);
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            YahooStoreEntity store = GetStore<YahooStoreEntity>();

            string message = viewModel.Save(store);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            MessageHelper.ShowError(this, message);
            e.NextPage = this;
        }
    }
}
