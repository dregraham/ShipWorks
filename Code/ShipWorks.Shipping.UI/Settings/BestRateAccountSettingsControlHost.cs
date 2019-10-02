using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings
{
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.BestRate)]
    [Component(RegistrationType.Self)]
    public partial class BestRateAccountSettingsControlHost : SettingsControlBase
    {

        public BestRateAccountSettingsControlHost(BestRateAccountSettingsViewModel settingsViewModel)
        {
            InitializeComponent();

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
