using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using System.Linq;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// WinForms host for the One Balance Settings Control
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IOneBalanceSettingsControlHost))]
    public partial class OneBalanceSettingsControlHost : UserControl, IOneBalanceSettingsControlHost
    {
        private OneBalanceSettingsControlViewModel settingsViewModel;
        private IUspsAccountManager accountManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IUspsAccountManager accountManager)
        {
            InitializeComponent();
            this.accountManager = accountManager;
        }

        /// <summary>
        /// Initializes the hosted control
        /// </summary>
        public void Initialize()
        {
            var account = accountManager.GetAccounts(UspsResellerType.None).Where(a => a.ShipEngineCarrierId != null).FirstOrDefault();

            var webClient = new UspsPostageWebClient(account);
            settingsViewModel = new OneBalanceSettingsControlViewModel(webClient);

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
