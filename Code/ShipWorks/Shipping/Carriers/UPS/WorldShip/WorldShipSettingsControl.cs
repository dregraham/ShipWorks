﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Settings control for WorldShip shipments
    /// </summary>
    public partial class WorldShipSettingsControl : SettingsControlBase
    {
        private readonly UpsShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipSettingsControl()
        {
            InitializeComponent();

            shipmentType = (UpsShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsWorldShip);
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
            accountControl.Initialize(ShipmentTypeCode.UpsWorldShip);

            upsMailInnovationsOptions.LoadSettings(shipmentType);

            InitializeServicePicker();
            InitializePackagingTypePicker();
        }

        /// <summary>
        /// Initializes the service picker for the shipment type
        /// </summary>
        private void InitializeServicePicker()
        {
            // Check if Mi is enabled
            bool isMiAvailable = shipmentType.IsMailInnovationsEnabled();

            // Check if SurePost is enabled
            bool isSurePostAvailable;
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.UpsSurePost, null);

                isSurePostAvailable = restrictionLevel == EditionRestrictionLevel.None;
            }

            List<UpsServiceType> excludedServices = shipmentType.GetExcludedServiceTypes().Select(exclusion => (UpsServiceType) exclusion).ToList();

            List<UpsServiceType> upsServices = Enum.GetValues(typeof(UpsServiceType)).Cast<UpsServiceType>()
                .Where(t => ShowService(t, isMiAvailable, isSurePostAvailable)).ToList();

            servicePicker.Initialize(upsServices, excludedServices);
        }


        /// <summary>
        /// Initialize the packaging type picker
        /// </summary>
        private void InitializePackagingTypePicker()
        {
            IEnumerable<UpsPackagingType> excluededPackagingTypes = shipmentType.GetExcludedPackageTypes().Cast<UpsPackagingType>();

            IEnumerable<UpsPackagingType> allPackagingTypes = EnumHelper.GetEnumList<UpsPackagingType>().Select(x => x.Value).OrderBy(x => EnumHelper.GetDescription(x));

            packagingTypePicker.Initialize(allPackagingTypes, excluededPackagingTypes);
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        protected override void SaveSettings(ShippingSettingsEntity settings)
        {
            upsMailInnovationsOptions.SaveSettings(settings);

            optionsControl.SaveSettings(settings);
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
        /// Returns a list of ExcludedPackageTypeEntity based on the packagingTypePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedPackageTypes()
        {
            List<int> packageTypesToExclude = packagingTypePicker.ExcludedEnumValues.Select(type => (int) type).ToList();

            return packageTypesToExclude;
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
