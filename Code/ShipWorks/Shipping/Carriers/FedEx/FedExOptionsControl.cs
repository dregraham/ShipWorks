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
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// UserControl of common options that are displayed both in the wizard and in settings
    /// </summary>
    public partial class FedExOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            EnumHelper.BindComboBox<ThermalLanguage>(thermalType);
            EnumHelper.BindComboBox<ThermalDocTabType>(thermalDocTabType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            maskAccountNumber.Checked = settings.FedExMaskAccount;

            thermalPrinter.Checked = settings.FedExThermal;
            thermalType.SelectedValue = (ThermalLanguage) settings.FedExThermalType;

            thermalDocTab.Checked = settings.FedExThermalDocTab;
            thermalDocTabType.SelectedValue = (ThermalDocTabType) settings.FedExThermalDocTabType;
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        private void OnUpdateThermalUI(object sender, EventArgs e)
        {
            labelThermalType.Enabled = thermalPrinter.Checked;
            thermalType.Enabled = thermalPrinter.Checked;
            thermalDocTab.Enabled = thermalPrinter.Checked;

            labelThermalDocTabType.Enabled = thermalPrinter.Checked && thermalDocTab.Checked;
            thermalDocTabType.Enabled = thermalPrinter.Checked && thermalDocTab.Checked;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.FedExMaskAccount = maskAccountNumber.Checked;

            settings.FedExThermal = thermalPrinter.Checked;
            settings.FedExThermalType = (int) thermalType.SelectedValue;

            settings.FedExThermalDocTab = thermalDocTab.Checked;
            settings.FedExThermalDocTabType = (int) thermalDocTabType.SelectedValue;
        }
    }
}
