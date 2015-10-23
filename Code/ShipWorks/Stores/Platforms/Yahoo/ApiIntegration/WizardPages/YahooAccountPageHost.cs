using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public partial class YahooAccountPageHost : AddStoreWizardPage
    {
        public YahooAccountPageHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
        }

        private void OnPageLoad(object sender, EventArgs e)
        {
            YahooApiAccountSettings page = new YahooApiAccountSettings
            {
                DataContext = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooAccountSettingsViewModel>>().Value
            };
            ControlHost.Child = page;
        }
    }
}
