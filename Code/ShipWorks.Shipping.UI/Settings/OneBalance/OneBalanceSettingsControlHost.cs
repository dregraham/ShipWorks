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
    /// WinForms host for One Balance Settings Control
    /// </summary>

    [KeyedComponent(typeof(IOneBalanceSettingsControlHost), "OneBalanceSettings")]
    [Component(RegistrationType.Self)]
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

        public void Initialize()
        {
            var account = accountManager.GetAccounts(UspsResellerType.None).Where(a => a.ShipEngineCarrierId != null).FirstOrDefault();

            var webClient = new UspsPostageWebClient(account);
            settingsViewModel = new OneBalanceSettingsControlViewModel(accountManager, webClient);

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
