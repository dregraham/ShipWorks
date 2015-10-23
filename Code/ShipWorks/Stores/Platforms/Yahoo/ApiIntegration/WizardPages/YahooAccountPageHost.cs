using System;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public partial class YahooAccountPageHost : AddStoreWizardPage
    {
        private YahooAccountSettingsViewModel viewModel;

        public YahooAccountPageHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
            SteppingInto += OnSteppingInto;
            StepNext += OnStepNext;
        }

        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            //viewModel.Load();
        }
        
        private void OnPageLoad(object sender, EventArgs e)
        {
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooAccountSettingsViewModel>>().Value;
            YahooApiAccountSettings page = new YahooApiAccountSettings
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
