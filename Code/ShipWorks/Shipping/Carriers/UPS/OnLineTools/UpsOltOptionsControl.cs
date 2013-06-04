using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// User control for editing global ups options
    /// </summary>
    public partial class UpsOltOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltOptionsControl()
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

            thermalPrinter.Checked = settings.UpsThermal;
            thermalType.SelectedValue = (ThermalLabelType) settings.UpsThermalType;
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
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.UpsThermal = thermalPrinter.Checked;
            settings.UpsThermalType = (int) thermalType.SelectedValue;
        }
    }
}
