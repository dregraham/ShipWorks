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
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Settings control to go in the Shipping Settings window
    /// </summary>
    public partial class FedExSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            shippersControl.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            FedExShipmentType shipmentType = (FedExShipmentType)ShipmentTypeManager.GetType(ShipmentTypeCode);

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.FedExInsuranceProvider;
            pennyOne.Checked = settings.FedExInsurancePennyOne;

            List<FedExServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (FedExServiceType)exclusion).ToList();

            List<FedExServiceType> upsServices = Enum.GetValues(typeof(FedExServiceType)).Cast<FedExServiceType>().ToList();

            servicePicker.Initialize(upsServices, excludedServices);
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            settings.FedExInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.FedExInsurancePennyOne = pennyOne.Checked;
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
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("FedEx", false))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            List<int> servicesToExclude = servicePicker.ExcludedEnumValues.Select(type => (int)type).ToList();

            return servicesToExclude;
        }
    }
}
