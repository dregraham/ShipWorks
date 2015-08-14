using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using System.Threading;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Common.IO.Hardware.Printers;

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

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            entity = new ShippingSettingsEntity(true);
            SqlAdapter.Default.FetchEntity(entity);

            needCheckForChanges = false;
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
                    SqlAdapter.Default.FetchEntity(entity);
                    needCheckForChanges = false;
                }

                return EntityUtility.CloneEntity(entity);
            }
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

            List<int> configured = new List<int>(settings.ConfiguredTypes);

            bool isConfigured = configured.Contains((int) shipmentTypeCode);

            if (!isConfigured)
            {
                Messenger.Current.Send(new ConfiguringCarrierMessage(settings, shipmentTypeCode));
            }

            Activate(shipmentTypeCode, settings);
            SetDefaultProviderIfNecessary(settings, shipmentTypeCode);

            // Make sure its marked as configured
            if (!isConfigured)
            {
                configured.Add((int) shipmentTypeCode);
                settings.ConfiguredTypes = configured.ToArray();
            }

            // Save the changes, if any
            Save(settings);

            if (!isConfigured)
            {
                new ShippingManagerWrapper().UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);
                Messenger.Current.Send(new CarrierConfiguredMessage(settings, shipmentTypeCode));   
            }
        }

        /// <summary>
        /// Activate the shipmentTypeCode, if necessary
        /// </summary>
        private static void Activate(ShipmentTypeCode shipmentTypeCode, ShippingSettingsEntity settings)
        {
            List<int> activated = new List<int>(settings.ActivatedTypes);

            // If its configured, its activated
            if (!activated.Contains((int) shipmentTypeCode))
            {
                activated.Add((int) shipmentTypeCode);
                settings.ActivatedTypes = activated.ToArray();
            }
        }

        /// <summary>
        /// Set the default provider to the specified value if necessary
        /// </summary>
        private static void SetDefaultProviderIfNecessary(ShippingSettingsEntity settings, ShipmentTypeCode shipmentTypeCode)
        {
            if (!settings.ConfiguredTypes.Any() && settings.DefaultType == (int) ShipmentTypeCode.None)
            {
                settings.DefaultType = (int) shipmentTypeCode;
            }
        }

        /// <summary>
        /// Create a single instance of the database row for a new shipworks database instance
        /// </summary>
        public static void CreateInstance(SqlAdapter adapter)
        {
            ShippingSettingsEntity settings = new ShippingSettingsEntity(true);

            settings.ActivatedTypes = new int[0];
            settings.ConfiguredTypes = new int[0];

            // Only want to show the single USPS provider by default
            settings.ExcludedTypes = new int[] { (int) ShipmentTypeCode.Endicia, (int) ShipmentTypeCode.Express1Endicia, (int) ShipmentTypeCode.Express1Usps, (int) ShipmentTypeCode.PostalWebTools, (int) ShipmentTypeCode.iParcel, (int) ShipmentTypeCode.OnTrac };
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
            settings.Express1EndiciaThermalDocTabType = (int)ThermalDocTabType.Leading;
            settings.Express1EndiciaSingleSource = false;

            settings.Express1UspsSingleSource = false;

            settings.WorldShipLaunch = false;
            settings.UpsMailInnovationsEnabled = false;

            settings.UspsAutomaticExpress1 = false;
            settings.UspsAutomaticExpress1Account = 0;
            settings.UspsInsuranceProvider = (int) InsuranceProvider.ShipWorks;

            settings.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.OnTracInsurancePennyOne = false;

            settings.IParcelInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.IParcelInsurancePennyOne = false;

            settings.UpsMailInnovationsEnabled = false;
            settings.WorldShipMailInnovationsEnabled = false;

            settings.BestRateExcludedTypes = new int[0];
            settings.ShipSenseEnabled = true;
            settings.ShipSenseUniquenessXml = "<ShipSenseUniqueness><ItemProperty><Name>SKU</Name><Name>Code</Name></ItemProperty><ItemAttribute /></ShipSenseUniqueness>";
            settings.ShipSenseProcessedShipmentID = 0;
            settings.ShipSenseEndShipmentID = 0;

            settings.AutoCreateShipments = true;

            settings.FedExFimsEnabled = false;
            settings.FedExFimsUsername = string.Empty;
            settings.FedExFimsPassword = string.Empty;

            adapter.SaveAndRefetch(settings);
        }
    }
}
