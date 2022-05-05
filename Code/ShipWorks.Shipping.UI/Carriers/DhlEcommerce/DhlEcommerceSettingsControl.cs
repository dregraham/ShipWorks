using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// Settings control for DHL eCommerce
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(SettingsControlBase), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceSettingsControl()
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.DhlEcommerce);

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// DHL eCommerce does support services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Initialize the service picker control
        /// </summary>
        private void InitializeServicePicker(ShipmentType shipmentType)
        {
            var excludedServices = shipmentType.GetExcludedServiceTypes().Cast<DhlEcommerceServiceType>();

            var allServices = EnumHelper.GetEnumList<DhlEcommerceServiceType>()
                .OrderBy(s => s.Description)
                .Select(s => s.Value);

            excludedServiceControl.Initialize(allServices, excludedServices);
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            return excludedServiceControl.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Load the account manager with DHL eCommerce accounts
        /// </summary>
        public override void LoadSettings()
        {
            carrierAccountManagerControl.Initialize(ShipmentTypeCode);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode);

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.DhlEcommerceInsuranceProvider;
            pennyOne.Checked = settings.DhlEcommerceInsurancePennyOne;

            requestedLabelFormatOptionControl.LoadDefaultProfile(ShipmentTypeManager.GetType(ShipmentTypeCode));
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.DhlEcommerceInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.DhlEcommerceInsurancePennyOne = pennyOne.Checked;
            settings.SetShipmentDateCutoff(ShipmentTypeCode, shippingCutoff.Value);

            requestedLabelFormatOptionControl.SaveDefaultProfile();
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
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("DHL", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
