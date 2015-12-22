using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

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
            FedExShipmentType shipmentType = (FedExShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode);

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.FedExInsuranceProvider;
            pennyOne.Checked = settings.FedExInsurancePennyOne;

            InitializeServicePicker(shipmentType);
            InitializePackagePicker(shipmentType);

            enableFims.Checked = settings.FedExFimsEnabled;
            fimsUsername.Text = settings.FedExFimsUsername;
            fimsPassword.Text = settings.FedExFimsPassword;

            SetFimsFieldsState();
        }

        /// <summary>
        /// Initialize the package picker
        /// </summary>
        private void InitializePackagePicker(FedExShipmentType shipmentType)
        {
            IEnumerable<FedExPackagingType> excludedPackages = shipmentType.GetExcludedPackageTypes().Cast<FedExPackagingType>();

            IEnumerable<FedExPackagingType> upsPackages = Enum.GetValues(typeof(FedExPackagingType)).Cast<FedExPackagingType>().ToList();

            packagePicker.Initialize(upsPackages, excludedPackages);
        }

        /// <summary>
        /// Initialize the service picker
        /// </summary>
        private void InitializeServicePicker(FedExShipmentType shipmentType)
        {
            IEnumerable<FedExServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Cast<FedExServiceType>();

            IEnumerable<FedExServiceType> upsServices = Enum.GetValues(typeof(FedExServiceType)).Cast<FedExServiceType>().ToList();

            servicePicker.Initialize(upsServices, excludedServices);
        }

        /// <summary>
        /// Save the settings 
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings(settings);

            settings.FedExInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.FedExInsurancePennyOne = pennyOne.Checked;

            settings.FedExFimsEnabled = enableFims.Checked;
            settings.FedExFimsUsername = fimsUsername.Text.Trim();
            settings.FedExFimsPassword = fimsPassword.Text.Trim();
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

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return servicePicker.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Returns a list of excluded package types
        /// </summary>
        public override IEnumerable<int> GetExcludedPackageTypes()
        {
            return packagePicker.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Enables/disables the FIMS username and password textboxes based on the checked state of the
        /// FIMS enabled checkbox
        /// </summary>
        private void OnEnableFimsCheckedChanged(object sender, EventArgs e)
        {
            SetFimsFieldsState();
        }

        /// <summary>
        /// Enables/disables the FIMS username and password textboxes based on the checked state of the
        /// FIMS enabled checkbox
        /// </summary>
        private void SetFimsFieldsState()
        {
            fimsUsername.Enabled = enableFims.Checked;
            fimsPassword.Enabled = enableFims.Checked;
        }
    }
}

