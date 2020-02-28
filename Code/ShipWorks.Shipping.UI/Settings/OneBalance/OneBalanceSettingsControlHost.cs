using Interapptive.Shared.ComponentRegistration;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal;
using System;

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
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IUspsAccountManager accountManager, Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory)
        {
            InitializeComponent();
            this.accountManager = accountManager;
            this.addMoneyDialogFactory = addMoneyDialogFactory;
        }

        /// <summary>
        /// Initializes the hosted control
        /// </summary>
        public void Initialize()
        {
            var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

            var webClient = account == null ? null : new UspsPostageWebClient(account);

            settingsViewModel = new OneBalanceSettingsControlViewModel(webClient, addMoneyDialogFactory);

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
