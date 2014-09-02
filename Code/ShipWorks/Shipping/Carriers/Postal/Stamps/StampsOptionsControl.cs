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
                domesticThermal.Checked = settings.Express1StampsDomesticThermal;
                internationalThermal.Checked = settings.Express1StampsInternationalThermal;

                thermalType.SelectedValue = (ThermalLanguage)settings.Express1StampsThermalType;
            }
            else
            {
                domesticThermal.Checked = settings.StampsDomesticThermal;
                internationalThermal.Checked = settings.StampsInternationalThermal;

                thermalType.SelectedValue = (ThermalLanguage)settings.StampsThermalType;
            }
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        private void OnUpdateThermalUI(object sender, EventArgs e)
        {
            labelThermalType.Enabled = domesticThermal.Checked || internationalThermal.Checked;
            thermalType.Enabled = domesticThermal.Checked || internationalThermal.Checked;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            if(IsExpress1)
            {
                settings.Express1StampsDomesticThermal = domesticThermal.Checked;
                settings.Express1StampsInternationalThermal = internationalThermal.Checked;

                settings.Express1StampsThermalType = (int)thermalType.SelectedValue; 
            }
            else
            {
                settings.StampsDomesticThermal = domesticThermal.Checked;
                settings.StampsInternationalThermal = internationalThermal.Checked;

                settings.StampsThermalType = (int)thermalType.SelectedValue;    
            }
        }
    }
}
