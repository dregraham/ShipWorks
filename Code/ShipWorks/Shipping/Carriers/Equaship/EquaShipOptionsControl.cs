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
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Control for configuring EquaShip shipping options
    /// </summary>
    public partial class EquaShipOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load settings from the database
        /// </summary>
        public void LoadSettings()
        {
            EnumHelper.BindComboBox<ThermalLanguage>(thermalType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            //thermalPrinter.Checked = settings.EquaShipThermal;
            //thermalType.SelectedValue = (ThermalLanguage)settings.EquaShipThermalType;
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
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            //settings.EquaShipThermal = thermalPrinter.Checked;
            //settings.EquaShipThermalType = (int)thermalType.SelectedValue;
        }
    }
}
