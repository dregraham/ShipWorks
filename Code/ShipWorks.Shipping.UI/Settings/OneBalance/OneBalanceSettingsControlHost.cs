using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using System.Linq;
using Autofac;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// WinForms host for the One Balance Settings Control
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IOneBalanceSettingsControlHost))]
    public partial class OneBalanceSettingsControlHost : UserControl, IOneBalanceSettingsControlHost
    {
        private OneBalanceSettingsControlViewModel settingsViewModel;
        private readonly IUspsAccountManager accountManager;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IUspsAccountManager accountManager, ILifetimeScope lifetimeScope)
        {
            InitializeComponent();
            this.accountManager = accountManager;
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Initializes the hosted control
        /// </summary>
        public void Initialize()
        {
            var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

            var webClient = account == null ? null : new UspsPostageWebClient(account);

            settingsViewModel = new OneBalanceSettingsControlViewModel(webClient, lifetimeScope);

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
