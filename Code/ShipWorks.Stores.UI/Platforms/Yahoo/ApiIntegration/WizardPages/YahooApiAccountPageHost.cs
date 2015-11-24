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
    public partial class YahooApiAccountPageHost : AddStoreWizardPage
    {
        private YahooApiAccountPageViewModel viewModel;

        public YahooApiAccountPageHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
            StepNext += OnStepNext;
        }

        private void OnPageLoad(object sender, EventArgs e)
        {
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooApiAccountPageViewModel>>().Value;

            YahooApiAccountPage page = new YahooApiAccountPage
            {
                DataContext = viewModel
            };
            ControlHost.Child = page;
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            YahooStoreEntity store = GetStore<YahooStoreEntity>();

            string message = viewModel.Save(store);

            if (!string.IsNullOrEmpty(message))
            {
                MessageHelper.ShowError(this, message);
                e.NextPage = this;
                return;
            }
        }
    }
}
