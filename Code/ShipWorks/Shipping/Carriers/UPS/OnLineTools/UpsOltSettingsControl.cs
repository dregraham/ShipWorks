using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Control for editing the global UPS settings
    /// </summary>
    public partial class UpsOltSettingsControl : SettingsControlBase
    {
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltSettingsControl(ILifetimeScope scope)
        {
            this.scope = scope;

            InitializeComponent();
            oneBalanceUpsBannerControl.SetupComplete += OnOneBalanceSetupComplete;
            UpdateOneBalanceBannerVisibility();
        }

        /// <summary>
        /// Reload the accounts after setup is complete
        /// </summary>
        private void OnOneBalanceSetupComplete(object sender, EventArgs e)
        {
            UpdateOneBalanceBannerVisibility();
            accountControl.LoadShippers();
        }

        /// <summary>
        /// update the one balance banner visibility
        /// </summary>
        private void UpdateOneBalanceBannerVisibility()
        {
            var licenses = scope.Resolve<ILicenseService>().GetLicenses();

            var shouldShow = (UpsAccountManager.AccountsReadOnly.None() || UpsAccountManager.AccountsReadOnly.All(a => string.IsNullOrWhiteSpace(a.ShipEngineCarrierId))) &&
            licenses.None(x => x.IsCtp);

            if (shouldShow)
            {
                oneBalanceUpsBannerControl.Visible = true;
                panel.Location = new Point(4, 89);
            }
            else
            {
                oneBalanceUpsBannerControl.Visible = false;
                panel.Location = new Point(0, 0);
            }
        }

        /// <summary>
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Carrier supports packages
        /// </summary>
        protected override bool SupportsPackages => true;

        /// <summary>
        /// Load the settings into the control
        /// </summary>
        public override void LoadSettings()
        {
            optionsControl.LoadSettings();
            accountControl.Initialize(ShipmentTypeCode.UpsOnLineTools);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            UpsShipmentType shipmentType = (UpsShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode);

            upsMailInnovationsOptions.LoadSettings(shipmentType);
            insuranceProviderChooser.InsuranceProvider = (InsuranceProvider) settings.UpsInsuranceProvider;
            pennyOne.Checked = settings.UpsInsurancePennyOne;
            noDims.Checked = settings.UpsAllowNoDims;

            shippingCutoff.Value = settings.GetShipmentDateCutoff(ShipmentTypeCode);

            InitializeServicePicker(shipmentType);
            InitializePackagingTypePicker(shipmentType);

            UpdateMailInnovationsVisibility();
        }

        /// <summary>
        /// Hide MI if no accounts support it
        /// </summary>
        private void UpdateMailInnovationsVisibility()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                bool onlyOneBalanceAccounts = scope.Resolve<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>().AccountsReadOnly.All(a => !string.IsNullOrEmpty(a.ShipEngineCarrierId));

                if (onlyOneBalanceAccounts)
                {
                    panelMailInnovations.Visible = false;
                    Point existingLocation = servicesPanel.Location;
                    servicesPanel.Location = new Point(existingLocation.X, existingLocation.Y - 57);
                }
            }
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

                var upsAccounts = UpsAccountManager.AccountsReadOnly.ToList();
                bool onlyShipEngineAccounts = upsAccounts.Any() && upsAccounts.None(a => string.IsNullOrEmpty(a.ShipEngineCarrierId));

                var upsServices = Enum.GetValues(typeof(UpsServiceType)).Cast<UpsServiceType>()
                    .Where(t => ShowService(t, onlyShipEngineAccounts, isMIAvailable, isSurePostAvailable));

                servicePicker.Initialize(upsServices, excludedServices);
            }
        }

        /// <summary>
        /// Initialize the packaging type picker
        /// </summary>
        private void InitializePackagingTypePicker(UpsShipmentType shipmentType)
        {
            IEnumerable<UpsPackagingType> excludedPackagingTypes = shipmentType.GetExcludedPackageTypes().Cast<UpsPackagingType>();

            IEnumerable<UpsPackagingType> allPackagingTypes = EnumHelper.GetEnumList<UpsPackagingType>().Select(x => x.Value).OrderBy(x => EnumHelper.GetDescription(x));

            upsPackagingTypeServicePickerControl.Initialize(allPackagingTypes, excludedPackagingTypes);
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
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            upsMailInnovationsOptions.SaveSettings(settings);

            optionsControl.SaveSettings();

            settings.SetShipmentDateCutoff(ShipmentTypeCode, shippingCutoff.Value);

            settings.UpsInsuranceProvider = (int) insuranceProviderChooser.InsuranceProvider;
            settings.UpsInsurancePennyOne = pennyOne.Checked;
            settings.UpsAllowNoDims = noDims.Checked;
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
            List<int> servicesToExclude = servicePicker.ExcludedEnumValues.Select(type => (int) type).ToList();

            return servicesToExclude;
        }

        /// <summary>
        /// Returns true if we should show the service. Else false.
        /// </summary>
        private static bool ShowService(UpsServiceType upsServiceType, bool onlyShipEngineAccounts, bool isMiAvailable, bool isSurePostAvailable)
        {
            // Filter out non-supported shipengine services when they have ups accounts and
            // all of the accounts are shipengine accounts. This takes priority over surepost and MI.
            if (onlyShipEngineAccounts)
            {
                return UpsShipEngineServiceTypeUtility.IsServiceSupported(upsServiceType);
            }

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


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            oneBalanceUpsBannerControl.SetupComplete -= OnOneBalanceSetupComplete;
            base.Dispose(disposing);
        }
    }
}
