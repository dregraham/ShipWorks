﻿using System;
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

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Settings control to go in the Shipping Settings window
    /// </summary>
    public partial class FedExSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            shippersControl.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.FedExInsuranceProvider;
            pennyOne.Checked = settings.FedExInsurancePennyOne;
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            settings.FedExInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.FedExInsurancePennyOne = pennyOne.Checked;
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
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("FedEx", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
