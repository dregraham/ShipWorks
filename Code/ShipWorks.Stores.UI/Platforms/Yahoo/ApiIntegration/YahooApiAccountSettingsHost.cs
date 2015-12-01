using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    public partial class YahooApiAccountSettingsHost : AccountSettingsControlBase
    {
        private YahooApiAccountSettingsViewModel viewModel;

        public YahooStoreEntity Store { get; set; }

        public YahooApiAccountSettingsHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
        }

        private void OnPageLoad(object sender, EventArgs e)
        {
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooApiAccountSettingsViewModel>>().Value;
            YahooApiAccountSettings page = new YahooApiAccountSettings
            {
                DataContext = viewModel
            };
            ControlHost.Child = page;

            viewModel.Load(Store);
        }

        public override void LoadStore(StoreEntity store)
        {
            Store = store as YahooStoreEntity;
        }

        public override bool SaveToEntity(StoreEntity store)
        {
            string message = viewModel.Save(store as YahooStoreEntity);

            if (message.Equals(string.Empty))
            {
                return true;
            }

            MessageHelper.ShowError(this, message);

            return false;
        }
    }
}
