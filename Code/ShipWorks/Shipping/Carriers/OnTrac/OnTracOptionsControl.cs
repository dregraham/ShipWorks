using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// UserControl for editing options specific to the OnTrac integration
    /// </summary>
    public partial class OnTracOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracOptionsControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            EnumHelper.BindComboBox<ThermalLabelType>(thermalType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            thermalPrinter.Checked = settings.OnTracThermal;
            thermalType.SelectedValue = (ThermalLabelType) settings.OnTracThermalType;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.OnTracThermal = thermalPrinter.Checked;
            settings.OnTracThermalType = (int) thermalType.SelectedValue;
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        void OnUpdateThermalUI(object sender, EventArgs e)
        {
            labelThermalType.Enabled = thermalPrinter.Checked;
            thermalType.Enabled = thermalPrinter.Checked;
        }
    }
}