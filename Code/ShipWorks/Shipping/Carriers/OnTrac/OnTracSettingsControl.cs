using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public partial class OnTracSettingsControl : SettingsControlBase
    {
        public OnTracSettingsControl()
        {
            InitializeComponent();

            insuranceProviderChooser.ProviderChanged += OnInsuranceProviderChanged;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            optionsControl.LoadSettings();
            accountManager.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider)settings.OnTracInsuranceProvider;
            pennyOne.Checked = settings.OnTracInsurancePennyOne;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings();

            settings.OnTracInsuranceProvider = (int)insuranceProviderChooser.InsuranceProvider;
            settings.OnTracInsurancePennyOne = pennyOne.Checked;
        }

        /// <summary>
        /// The selected insurance provider has changed
        /// </summary>
        private void OnInsuranceProviderChanged(object sender, EventArgs e)
        {
            pennyOne.Enabled = (insuranceProviderChooser.InsuranceProvider == InsuranceProvider.ShipWorks);
        }

        /// <summary>
        /// Opening the PennyOne info link
        /// </summary>
        private void OnLinkPennyOne(object sender, EventArgs e)
        {
            using (var dlg = new InsurancePennyOneDlg("OnTrac", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}