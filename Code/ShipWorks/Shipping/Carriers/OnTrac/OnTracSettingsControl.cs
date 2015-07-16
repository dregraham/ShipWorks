using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public partial class OnTracSettingsControl : SettingsControlBase
    {
        public OnTracSettingsControl()
        {
            InitializeComponent();

            insuranceProviderChooser.ProviderChanged += OnInsuranceProviderChanged;
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            optionsControl.LoadSettings();
            accountManager.Initialize();

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider)settings.OnTracInsuranceProvider;
            pennyOne.Checked = settings.OnTracInsurancePennyOne;

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode);
            IEnumerable<OnTracServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Cast<OnTracServiceType>();

            IEnumerable<OnTracServiceType> allServices = OnTracShipmentType.ServiceTypes
                .Where(x => x != OnTracServiceType.None)
                .OrderBy(x => EnumHelper.GetDescription(x));

            excludedServiceControl.Initialize(allServices, excludedServices);
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            optionsControl.SaveSettings();

            settings.OnTracInsuranceProvider = (int)insuranceProviderChooser.InsuranceProvider;
            settings.OnTracInsurancePennyOne = pennyOne.Checked;
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override List<ExcludedServiceTypeEntity> GetExcludedServices()
        {
            List<int> servicesToExclude = excludedServiceControl.ExcludedServiceTypes.Select(type => (int)type).ToList();

            List<ExcludedServiceTypeEntity> excludedServiceTypes = servicesToExclude
                .Select(serviceToExclude => new ExcludedServiceTypeEntity { ShipmentType = (int)this.ShipmentTypeCode, ServiceType = serviceToExclude })
                .ToList();

            return excludedServiceTypes;
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
            using (var dlg = new InsurancePennyOneDlg("OnTrac", false))
            {
                dlg.ShowDialog(this);
            }
        }
    }

    [CLSCompliant(false)]
    public class OnTracServicePickerControl : CarrierServicePickerControl<OnTracServiceType>
    {
        
    }
}