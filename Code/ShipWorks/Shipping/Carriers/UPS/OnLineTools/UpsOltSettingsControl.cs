using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Control for editing the global UPS settings
    /// </summary>
    public partial class UpsOltSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            accountControl.Initialize(ShipmentTypeCode.UpsOnLineTools);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            UpsShipmentType shipmentType = (UpsShipmentType)ShipmentTypeManager.GetType(ShipmentTypeCode);

            upsMailInnovationsOptions.LoadSettings(shipmentType);
            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.UpsInsuranceProvider;
            pennyOne.Checked = settings.UpsInsurancePennyOne;

            InitializeServicePicker(shipmentType);
            InitializePackagingTypePicker(shipmentType);
        }

        /// <summary>
        /// Initialize the service picker
        /// </summary>
        private void InitializeServicePicker(UpsShipmentType shipmentType)
        {
            // Check if Mi is enabled
            bool isMIAvailable = shipmentType.IsMailInnovationsEnabled();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.UpsSurePost, null);

                // Check if SurePost is enabled
                bool isSurePostAvailable = restrictionLevel == EditionRestrictionLevel.None;

                List<UpsServiceType> excludedServices =
                    shipmentType.GetExcludedServiceTypes().Select(exclusion => (UpsServiceType) exclusion).ToList();

                List<UpsServiceType> upsServices = Enum.GetValues(typeof (UpsServiceType)).Cast<UpsServiceType>()
                    .Where(t => ShowService(t, isMIAvailable, isSurePostAvailable)).ToList();

                servicePicker.Initialize(upsServices, excludedServices);
            }
        }

        /// <summary>
        /// Initialize the packaging type picker
        /// </summary>
        private void InitializePackagingTypePicker(UpsShipmentType shipmentType)
        {
            IEnumerable<UpsPackagingType> excluededPackagingTypes = shipmentType.GetExcludedPackageTypes().Cast<UpsPackagingType>();

            IEnumerable<UpsPackagingType> allPackagingTypes = EnumHelper.GetEnumList<UpsPackagingType>().Select(x => x.Value).OrderBy(x => EnumHelper.GetDescription(x));

            upsPackagingTypeServicePickerControl.Initialize(allPackagingTypes, excluededPackagingTypes);
        }

        /// <summary>
        /// Return the excluded package types
        /// </summary>
        public override IEnumerable<int> GetExcludedPackageTypes()
        {
            return upsPackagingTypeServicePickerControl.ExcludedEnumValues.Cast<int>();
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void SaveSettings(ShippingSettingsEntity settings)
        {
            upsMailInnovationsOptions.SaveSettings(settings);

            optionsControl.SaveSettings();

            settings.UpsInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.UpsInsurancePennyOne = pennyOne.Checked;
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
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg("UPS", false))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Returns a list of ExcludedServiceTypeEntity based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            List<int> servicesToExclude = servicePicker.ExcludedEnumValues.Select(type => (int)type).ToList();

            return servicesToExclude;
        }

        /// <summary>
        /// Returns true if we should show the service. Else false.
        /// </summary>
        private static bool ShowService(UpsServiceType upsServiceType, bool isMiAvailable, bool isSurePostAvailable)
        {
            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                return isSurePostAvailable;
            }

            if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                return isMiAvailable;
            }

            return true;
        }
    }
}
