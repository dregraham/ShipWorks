﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    public static class ShippingSettings
    {
        static ShippingSettingsEntity entity;
        static bool needCheckForChanges = false;

        static object threadLock = new object();
        private static IShippingSettingsEntity readOnlyEntity;

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            entity = new ShippingSettingsEntity(true);

            RefreshSettingsData();
        }

        /// <summary>
        /// Check the database for the latest SystemData
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            needCheckForChanges = true;
        }

        /// <summary>
        /// Fetch the latest shipping settings
        /// </summary>
        public static ShippingSettingsEntity Fetch()
        {
            lock (threadLock)
            {
                if (needCheckForChanges)
                {
                    RefreshSettingsData();
                }

                return EntityUtility.CloneEntity(entity);
            }
        }

        /// <summary>
        /// Fetch the latest shipping settings
        /// </summary>
        public static IShippingSettingsEntity FetchReadOnly()
        {
            lock (threadLock)
            {
                if (needCheckForChanges)
                {
                    RefreshSettingsData();
                }

                return readOnlyEntity;
            }
        }

        /// <summary>
        /// Load the shipping settings if necessary
        /// </summary>
        private static void RefreshSettingsData()
        {
            SqlAdapter.Default.FetchEntity(entity);
            readOnlyEntity = entity.AsReadOnly();
            needCheckForChanges = false;
        }

        /// <summary>
        /// Save the given entity as the current set of shipping settings
        /// </summary>
        public static void Save(ShippingSettingsEntity settings)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(settings);

                Interlocked.Exchange(ref entity, EntityUtility.CloneEntity(settings));
                readOnlyEntity = entity.AsReadOnly();
            }

            needCheckForChanges = false;
        }

        /// <summary>
        /// Marks the given shipment type as activated.  Use by the 2x upgrader to indicate that shipments exist for the type, but the user
        /// opted to not fully migrate the configurat\accounts for the type.
        /// </summary>
        public static void MarkAsActivated(ShipmentTypeCode shipmentTypeCode)
        {
            CheckForChangesNeeded();
            ShippingSettingsEntity settings = Fetch();

            Activate(shipmentTypeCode, settings);

            // Save the changes, if any
            Save(settings);
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public static void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode)
        {
            CheckForChangesNeeded();
            ShippingSettingsEntity settings = Fetch();

            List<ShipmentTypeCode> configured = settings.ConfiguredTypes.ToList();

            bool isConfigured = configured.Contains(shipmentTypeCode);

            if (!isConfigured)
            {
                Messenger.Current.Send(new ConfiguringCarrierMessage(settings, shipmentTypeCode));
            }

            Activate(shipmentTypeCode, settings);
            SetDefaultProviderIfNecessary(settings, shipmentTypeCode);

            // Make sure its marked as configured
            if (!isConfigured)
            {
                configured.Add(shipmentTypeCode);
                settings.ConfiguredTypes = configured;
            }

            // Save the changes, if any
            Save(settings);

            if (!isConfigured)
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    lifetimeScope.Resolve<IShippingManager>().UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);
                    lifetimeScope.Resolve<IMessenger>().Send(new CarrierConfiguredMessage(settings, shipmentTypeCode));
                }
            }
        }

        /// <summary>
        /// Activate the shipmentTypeCode, if necessary
        /// </summary>
        private static void Activate(ShipmentTypeCode shipmentTypeCode, ShippingSettingsEntity settings)
        {
            List<ShipmentTypeCode> activated = settings.ActivatedTypes.ToList();

            // If its configured, its activated
            if (!activated.Contains(shipmentTypeCode))
            {
                activated.Add(shipmentTypeCode);
                settings.ActivatedTypes = activated.ToArray();
            }
        }

        /// <summary>
        /// Set the default provider to the specified value if necessary
        /// </summary>
        private static void SetDefaultProviderIfNecessary(ShippingSettingsEntity settings, ShipmentTypeCode shipmentTypeCode)
        {
            if (!settings.ConfiguredTypes.Any(t => t != ShipmentTypeCode.BestRate) && settings.DefaultType == (int) ShipmentTypeCode.None)
            {
                settings.DefaultType = (int) shipmentTypeCode;
            }
        }

        /// <summary>
        /// Collect Shipping Settings related telemetry
        /// </summary>
        public static IDictionary<string, string> GetTelemetryData()
        {
            IShippingSettingsEntity settings = FetchReadOnly();

            // Grab the ShipmentTypeCodes description excluding express 1 because we need a more explicit description
            List<string> shipmentTypeDescriptions = settings.ActivatedTypes
                .Intersect(Enum.GetValues(typeof(ShipmentTypeCode)).Cast<ShipmentTypeCode>())
                .Except(settings.ExcludedTypes)
                .Except(new[] { ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Usps })
                .Select(t => EnumHelper.GetDescription(t))
                .ToList();

            if (settings.ActivatedTypes.Contains(ShipmentTypeCode.Express1Usps) && !settings.ExcludedTypes.Contains(ShipmentTypeCode.Express1Usps))
            {
                shipmentTypeDescriptions.Add("Express1 (Stamps.com)");
            }

            if (settings.ActivatedTypes.Contains(ShipmentTypeCode.Express1Endicia) && !settings.ExcludedTypes.Contains(ShipmentTypeCode.Express1Endicia))
            {
                shipmentTypeDescriptions.Add("Express1 (Endicia)");
            }

            return new Dictionary<string, string>
            {
                {"Shipping.ActiveProviders", string.Join(",", shipmentTypeDescriptions.Any() ? string.Join(",", shipmentTypeDescriptions) : "N/A") }
            };
        }

        /// <summary>
        /// Create a single instance of the database row for a new shipworks database instance
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void CreateInstance(SqlAdapter adapter)
        {
            ShippingSettingsEntity settings = new ShippingSettingsEntity(true);

            settings.ActivatedTypes = Enumerable.Empty<ShipmentTypeCode>();
            settings.ConfiguredTypes = Enumerable.Empty<ShipmentTypeCode>();

            // Only want to show the single USPS provider by default
            settings.ExcludedTypes = new[] { ShipmentTypeCode.Endicia, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Usps, ShipmentTypeCode.PostalWebTools, ShipmentTypeCode.iParcel, ShipmentTypeCode.OnTrac, ShipmentTypeCode.DhlEcommerce };
            settings.DefaultType = (int) ShipmentTypeCode.None;

            settings.BlankPhoneOption = (int) ShipmentBlankPhoneOption.ShipperPhone;
            settings.BlankPhoneNumber = "999-999-9999";

            settings.InsurancePolicy = "";
            settings.InsuranceLastAgreed = null;

            settings.FedExUsername = null;
            settings.FedExPassword = null;
            settings.FedExMaskAccount = true;
            settings.FedExThermalDocTab = false;
            settings.FedExThermalDocTabType = (int) ThermalDocTabType.Leading;
            settings.FedExInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.FedExInsurancePennyOne = false;

            settings.UpsAccessKey = null;
            settings.UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.UpsInsurancePennyOne = false;

            settings.EndiciaCustomsCertify = false;
            settings.EndiciaCustomsSigner = "";
            settings.EndiciaThermalDocTab = false;
            settings.EndiciaThermalDocTabType = (int) ThermalDocTabType.Leading;
            settings.EndiciaAutomaticExpress1 = false;
            settings.EndiciaAutomaticExpress1Account = 0;
            settings.EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks;

            settings.Express1EndiciaCustomsCertify = false;
            settings.Express1EndiciaCustomsSigner = "";
            settings.Express1EndiciaThermalDocTab = false;
            settings.Express1EndiciaThermalDocTabType = (int) ThermalDocTabType.Leading;
            settings.Express1EndiciaSingleSource = false;

            settings.Express1UspsSingleSource = false;

            settings.WorldShipLaunch = false;
            settings.UpsMailInnovationsEnabled = false;

            settings.UspsAutomaticExpress1 = false;
            settings.UspsAutomaticExpress1Account = 0;
            settings.UspsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.ShipmentDateCutoffJson = string.Empty;

            settings.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.OnTracInsurancePennyOne = false;

            settings.IParcelInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.IParcelInsurancePennyOne = false;

            settings.UpsMailInnovationsEnabled = false;
            settings.WorldShipMailInnovationsEnabled = false;

            settings.BestRateExcludedTypes = Enumerable.Empty<ShipmentTypeCode>();
            settings.ShipSenseEnabled = true;
            settings.ShipSenseUniquenessXml = "<ShipSenseUniqueness><ItemProperty><Name>SKU</Name><Name>Code</Name></ItemProperty><ItemAttribute /></ShipSenseUniqueness>";
            settings.ShipSenseProcessedShipmentID = 0;
            settings.ShipSenseEndShipmentID = 0;

            settings.AutoCreateShipments = true;
            settings.ShipmentEditLimit = ShipmentsLoaderConstants.DefaultMaxAllowedOrders;

            settings.FedExFimsEnabled = false;
            settings.FedExFimsUsername = string.Empty;
            settings.FedExFimsPassword = string.Empty;

            settings.ShipEngineApiKey = string.Empty;
            settings.ShipEngineAccountID = string.Empty;

            settings.DhlEcommerceInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.DhlEcommerceInsurancePennyOne = false;

            adapter.SaveAndRefetch(settings);
        }
    }
}
