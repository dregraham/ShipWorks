﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// i-parcel settings control for changing i-parcel shipping settings.
    /// </summary>
    public partial class iParcelSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelSettingsControl" /> class.
        /// </summary>
        public iParcelSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Loads the shippings settings for i-parcel.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            optionsControl.LoadSettings();
            accountManager.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.IParcelInsuranceProvider;
            pennyOne.Checked = settings.IParcelInsurancePennyOne;

            // Load up the service picker based on the excluded service types
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            List<iParcelServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (iParcelServiceType) exclusion).ToList();

            List<iParcelServiceType> allServices = EnumHelper.GetEnumList<iParcelServiceType>().Select(e => e.Value).ToList();
            servicePicker.Initialize(allServices, excludedServices);
        }

        /// <summary>
        /// Save the shipping settings for i-parcel.
        /// </summary>
        /// <param name="settings"></param>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            base.SaveSettings(settings);
            optionsControl.SaveSettings();

            settings.IParcelInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.IParcelInsurancePennyOne = pennyOne.Checked;
        }

        /// <summary>
        /// Gets the excluded services based on the items in the service picker.
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Called when [insurance provider changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnInsuranceProviderChanged(object sender, EventArgs e)
        {
            pennyOne.Enabled = (insuranceProviderChooser.InsuranceProvider == InsuranceProvider.ShipWorks);
        }

        /// <summary>
        /// Called when the penny one link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnLinkPennyOne(object sender, EventArgs e)
        {
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("i-parcel", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
