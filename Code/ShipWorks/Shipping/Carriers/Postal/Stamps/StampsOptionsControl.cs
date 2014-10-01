using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for editing options specific to the Stamps.com integration
    /// </summary>
    public partial class StampsOptionsControl : PostalOptionsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The stamps reseller type.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            EnumHelper.BindComboBox<ThermalLanguage>(thermalType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if (ShipmentTypeCode == ShipmentTypeCode.Express1Stamps)
            {
                thermalPrinter.Checked = settings.Express1StampsThermal;
                thermalType.SelectedValue = (ThermalLanguage)settings.Express1StampsThermalType;
            }
            else if (ShipmentTypeCode == ShipmentTypeCode.Stamps)
            {
                thermalPrinter.Checked = settings.StampsThermal;
                thermalType.SelectedValue = (ThermalLanguage)settings.StampsThermalType;
            }
            else if (ShipmentTypeCode == ShipmentTypeCode.Usps)
            {
                thermalPrinter.Checked = settings.UspsThermal;
                thermalType.SelectedValue = (ThermalLanguage)settings.UspsThermalType;
            }
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        private void OnUpdateThermalUI(object sender, EventArgs e)
        {
            labelThermalType.Enabled = thermalPrinter.Checked;
            thermalType.Enabled = thermalPrinter.Checked;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            if (ShipmentTypeCode == ShipmentTypeCode.Express1Stamps)
            {
                settings.Express1StampsThermal = thermalPrinter.Checked;
                settings.Express1StampsThermalType = (int)thermalType.SelectedValue;   
            }
            else if (ShipmentTypeCode == ShipmentTypeCode.Stamps)
            {
                settings.StampsThermal = thermalPrinter.Checked;
                settings.StampsThermalType = (int)thermalType.SelectedValue;
            }
            else if (ShipmentTypeCode == ShipmentTypeCode.Usps)
            {
                settings.UspsThermal = thermalPrinter.Checked;
                settings.UspsThermalType = (int)thermalType.SelectedValue;
            }
        }
    }
}
