using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// WinForms host for the One Balance Settings Control
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IOneBalanceSettingsControlHost))]
    public partial class OneBalanceSettingsControlHost : UserControl, IOneBalanceSettingsControlHost
    {
        IOneBalanceSettingsControlViewModel settingsViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IOneBalanceSettingsControlViewModel settingsViewModel)
        {
            InitializeComponent();
            this.settingsViewModel = settingsViewModel;
            this.settingsControl.DataContext = this.settingsViewModel;
        }

        /// <summary>
        /// Save the settings view model
        /// </summary>
        public void SaveSettings()
        {
            settingsViewModel.SaveAutoFundSettings();
        }
    }
}
