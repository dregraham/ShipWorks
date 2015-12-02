using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Winforms element for hosting the WPF Yahoo Account Settings Page
    /// </summary>
    public partial class YahooApiAccountSettingsHost : AccountSettingsControlBase
    {
        private YahooApiAccountSettingsViewModel viewModel;

        /// <summary>
        /// The store this account settings page is for
        /// </summary>
        public YahooStoreEntity Store { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountSettingsHost"/> class.
        /// </summary>
        public YahooApiAccountSettingsHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
        }

        /// <summary>
        /// Called when [page load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Loads the store this page is for
        /// </summary>
        /// <param name="store">The store.</param>
        public override void LoadStore(StoreEntity store)
        {
            Store = store as YahooStoreEntity;
        }

        /// <summary>
        /// Save the new account information if no errors occured,
        /// displays error if one occurs.
        /// </summary>
        /// <param name="store">The store entity</param>
        /// <returns></returns>
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
