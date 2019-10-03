using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings
{
    /// <summary>
    /// WinForms host for BestRateAccountSettingsControl
    /// </summary>
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.BestRate)]
    [Component(RegistrationType.Self)]
    public partial class BestRateAccountSettingsControlHost : SettingsControlBase
    {
        private readonly BestRateAccountSettingsViewModel settingsViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateAccountSettingsControlHost(BestRateAccountSettingsViewModel settingsViewModel)
        {
            InitializeComponent();

            this.settingsViewModel = settingsViewModel;
            settingsControl.DataContext = settingsViewModel;
        }

        /// <summary>
        /// Load the settings view model
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            settingsViewModel.Load();
        }

        /// <summary>
        /// Save the settings view model
        /// </summary>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            settingsViewModel.Save();
        }
    }
}
