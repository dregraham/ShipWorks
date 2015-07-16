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
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Control for editing the global UPS settings
    /// </summary>
    public partial class UpsOltSettingsControl : SettingsControlBase
    {
        private CarrierServicePickerControl<UpsServiceType> servicePicker;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltSettingsControl()
        {
            InitializeComponent();
            InitializeServicePicker();
        }

        /// <summary>
        /// Initialize the ShipmentTypeCode from derived class
        /// </summary>
        public override void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            base.Initialize(shipmentTypeCode);
            
        }


        /// <summary>
        /// Initializes the service picker with Ups service types for the USPS carrier.
        /// </summary>
        private void InitializeServicePicker()
        {
            // Add carrier service picker control to the exclusions panel
            servicePicker = new CarrierServicePickerControl<UpsServiceType>();
            servicePicker.Dock = DockStyle.Fill;
            servicePicker.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            panelExclusionConfiguration.Controls.Add(servicePicker);
            panelExclusionConfiguration.Height = servicePicker.Height + 10;
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

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<UpsServiceType> excludedServices = new List<UpsServiceType>();

            List<UpsServiceType> upsServices = Enum.GetValues(typeof(UpsServiceType)).Cast<UpsServiceType>().ToList();
            servicePicker.Initialize(upsServices, excludedServices);
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

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedServiceTypes.Cast<int>();
        }
    }
}
