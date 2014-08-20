using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System;
using ShipWorks.Common.IO.Hardware.Printers;

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
        /// Whether the control is used for Express1.
        /// </summary>
        public bool IsExpress1 
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

            if(IsExpress1)
            {
                thermalPrinter.Checked = settings.Express1StampsDomesticThermal;
                thermalType.SelectedValue = (ThermalLanguage)settings.Express1StampsThermalType;
            }
            else
            {
                thermalPrinter.Checked = settings.StampsDomesticThermal;
                thermalType.SelectedValue = (ThermalLanguage)settings.StampsThermalType;
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
            if(IsExpress1)
            {
                settings.Express1StampsDomesticThermal = thermalPrinter.Checked;
                settings.Express1StampsThermalType = (int)thermalType.SelectedValue;   
            }
            else
            {
                settings.StampsDomesticThermal = thermalPrinter.Checked;
                settings.StampsThermalType = (int)thermalType.SelectedValue;    
            }
        }
    }
}
