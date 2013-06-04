using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for editing options specific to the endicia integration
    /// </summary>
    public partial class EndiciaOptionsControl : UserControl
    {
        // Endicia or a reseller like Express1
        EndiciaReseller endiciaReseller = EndiciaReseller.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings(EndiciaReseller reseller)
        {
            // remember the reseller for use when saving
            endiciaReseller = reseller;

            EnumHelper.BindComboBox<ThermalLabelType>(thermalType);
            EnumHelper.BindComboBox<ThermalDocTabType>(thermalDocTabType);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            switch (endiciaReseller)
            {
                case EndiciaReseller.Express1:
                    {
                        thermalPrinter.Checked = settings.Express1Thermal;
                        thermalType.SelectedValue = (ThermalLabelType)settings.Express1ThermalType;

                        customsCertify.Checked = settings.Express1CustomsCertify;
                        customsSigner.Text = settings.Express1CustomsSigner;

                        thermalDocTab.Checked = settings.Express1ThermalDocTab;
                        thermalDocTabType.SelectedValue = (ThermalDocTabType) settings.Express1ThermalDocTabType;

                        break;
                    }

                case EndiciaReseller.None:
                default:
                    {
                        thermalPrinter.Checked = settings.EndiciaThermal;
                        thermalType.SelectedValue = (ThermalLabelType)settings.EndiciaThermalType;

                        customsCertify.Checked = settings.EndiciaCustomsCertify;
                        customsSigner.Text = settings.EndiciaCustomsSigner;

                        thermalDocTab.Checked = settings.EndiciaThermalDocTab;
                        thermalDocTabType.SelectedValue = (ThermalDocTabType) settings.EndiciaThermalDocTabType;

                        break;
                    }
            }
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
            switch (endiciaReseller)
            {
                case EndiciaReseller.Express1:
                    {
                        settings.Express1Thermal = thermalPrinter.Checked;
                        settings.Express1ThermalType = (int)thermalType.SelectedValue;

                        settings.Express1CustomsCertify = customsCertify.Checked;
                        settings.Express1CustomsSigner = customsSigner.Text;

                        settings.Express1ThermalDocTab = thermalDocTab.Checked;
                        settings.Express1ThermalDocTabType = (int) thermalDocTabType.SelectedValue;

                        break;
                    }
                case EndiciaReseller.None:
                default:
                    {
                        settings.EndiciaThermal = thermalPrinter.Checked;
                        settings.EndiciaThermalType = (int)thermalType.SelectedValue;

                        settings.EndiciaCustomsCertify = customsCertify.Checked;
                        settings.EndiciaCustomsSigner = customsSigner.Text;

                        settings.EndiciaThermalDocTab = thermalDocTab.Checked;
                        settings.EndiciaThermalDocTabType = (int) thermalDocTabType.SelectedValue;

                        break;
                    }
            }
        }

        /// <summary>
        /// Change the option for customs certify
        /// </summary>
        private void OnChangeCustomsCertify(object sender, EventArgs e)
        {
            customsSigner.Enabled = customsCertify.Checked;
        }
    }
}
