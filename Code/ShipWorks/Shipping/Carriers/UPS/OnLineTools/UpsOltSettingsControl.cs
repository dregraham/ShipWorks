using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Control for editing the global UPS settings
    /// </summary>
    public partial class UpsOltSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            accountControl.Initialize(ShipmentTypeCode.UpsOnLineTools);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            upsMailInnovationsOptions.LoadSettings((UpsShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools));
            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.UpsInsuranceProvider;
            pennyOne.Checked = settings.UpsInsurancePennyOne;
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            upsMailInnovationsOptions.SaveSettings(settings);

            optionsControl.SaveSettings();

            settings.UpsInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.UpsInsurancePennyOne = pennyOne.Checked;
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
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("UPS", false))
            {
                dlg.ShowDialog(this);
            }
        }

        public override List<ExcludedServiceTypeEntity> GetExcludededServices()
        {
            //List<int> servicesToExclude = servicePicker.ExcludedServiceTypes.Select(type => (int)type).ToList();

            List<ExcludedServiceTypeEntity> excludedServiceTypes = new List<ExcludedServiceTypeEntity>();

            ExcludedServiceTypeEntity test = new ExcludedServiceTypeEntity()
            {
                ShipmentType = 1,
                ServiceType = 9
            };


            excludedServiceTypes.Add(test);
            return excludedServiceTypes;
        }

    }
}
