using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
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
            labelFormat.SelectedValueChanged += OnUpdateThermalUI;

            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat);
            EnumHelper.BindComboBox<ThermalDocTabType>(thermalDocTabType);
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            maskAccountNumber.Checked = settings.FedExMaskAccount;

            labelFormat.SelectedValue = ShippingProfileManager.GetLabelFormatFromDefaultProfile<FedExShipmentType>();

            thermalDocTab.Checked = settings.FedExThermalDocTab;
            thermalDocTabType.SelectedValue = (ThermalDocTabType) settings.FedExThermalDocTabType;
        }

        /// <summary>
        /// Update the enabled state of the thermal UI based on what's selected
        /// </summary>
        private void OnUpdateThermalUI(object sender, EventArgs e)
        {
            bool isThermal = ((ThermalLanguage?) labelFormat.SelectedValue).GetValueOrDefault(ThermalLanguage.None) != ThermalLanguage.None;

            thermalDocTab.Enabled = isThermal;

            labelThermalDocTabType.Enabled = isThermal && thermalDocTab.Checked;
            thermalDocTabType.Enabled = isThermal && thermalDocTab.Checked;
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.FedExMaskAccount = maskAccountNumber.Checked;

            ShippingProfileManager.SaveLabelFormatToDefaultProfile<FedExShipmentType>((ThermalLanguage)labelFormat.SelectedValue);

            settings.FedExThermalDocTab = thermalDocTab.Checked;
            settings.FedExThermalDocTabType = (int) thermalDocTabType.SelectedValue;
        }
    }
}
