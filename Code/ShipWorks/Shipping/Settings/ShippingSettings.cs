using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            List<int> activated = new List<int>(settings.ActivatedTypes);

            // If its configured, its activated
            if (!activated.Contains((int) shipmentTypeCode))
            {
                activated.Add((int) shipmentTypeCode);
                settings.ActivatedTypes = activated.ToArray();
            }

            // Save the changes, if any
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public static void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode)
        {
            CheckForChangesNeeded();
            ShippingSettingsEntity settings = Fetch();

            List<int> configured = new List<int>(settings.ConfiguredTypes);
            List<int> activated = new List<int>(settings.ActivatedTypes);

            // If its configured, its activated
            if (!activated.Contains((int) shipmentTypeCode))
            {
                activated.Add((int) shipmentTypeCode);
                settings.ActivatedTypes = activated.ToArray();
            }

            // Make sure its marked as configured
            if (!configured.Contains((int) shipmentTypeCode))
            {
                configured.Add((int) shipmentTypeCode);
                settings.ConfiguredTypes = configured.ToArray();
            }

            // Save the changes, if any
            ShippingSettings.Save(settings);
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
            settings.ExcludedTypes = new int[] { (int)ShipmentTypeCode.Endicia, (int)ShipmentTypeCode.Express1Endicia, (int)ShipmentTypeCode.Express1Stamps, (int)ShipmentTypeCode.PostalWebTools, (int)ShipmentTypeCode.Stamps };
            settings.DefaultType = (int) ShipmentTypeCode.None;

            settings.BlankPhoneOption = (int) ShipmentBlankPhoneOption.ShipperPhone;
            settings.BlankPhoneNumber = "999-999-9999";

            settings.InsurancePolicy = "";
            settings.InsuranceLastAgreed = null;

            settings.FedExUsername = null;
            settings.FedExPassword = null;
            settings.FedExMaskAccount = true;
            settings.FedExThermal = false;
            settings.FedExThermalType = (int) ThermalLanguage.EPL;
            settings.FedExThermalDocTab = false;
            settings.FedExThermalDocTabType = (int) ThermalDocTabType.Leading;
            settings.FedExInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.FedExInsurancePennyOne = false;

            settings.UpsAccessKey = null;
            settings.UpsThermal = false;
            settings.UpsThermalType = (int) ThermalLanguage.EPL;
            settings.UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.UpsInsurancePennyOne = false;

            settings.EndiciaThermal = false;
            settings.EndiciaThermalType = (int) ThermalLanguage.EPL;
            settings.EndiciaCustomsCertify = false;
            settings.EndiciaCustomsSigner = "";
            settings.EndiciaThermalDocTab = false;
            settings.EndiciaThermalDocTabType = (int) ThermalDocTabType.Leading;
            settings.EndiciaAutomaticExpress1 = false;
            settings.EndiciaAutomaticExpress1Account = 0;
            settings.EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.EndiciaUspsAutomaticExpedited = false;
            settings.EndiciaUspsAutomaticExpeditedAccount = 0;

            settings.Express1EndiciaThermal = false;
            settings.Express1EndiciaThermalType = (int)ThermalLanguage.EPL;
            settings.Express1EndiciaCustomsCertify = false;
            settings.Express1EndiciaCustomsSigner = "";
            settings.Express1EndiciaThermalDocTab = false;
            settings.Express1EndiciaThermalDocTabType = (int)ThermalDocTabType.Leading;
            settings.Express1EndiciaSingleSource = false;

            settings.Express1StampsThermal = false;
            settings.Express1StampsThermalType = (int)ThermalLanguage.EPL;
            settings.Express1StampsSingleSource = false;

            settings.WorldShipLaunch = false;
            settings.UpsMailInnovationsEnabled = false;

            settings.StampsThermal = false;
            settings.StampsThermalType = (int) ThermalLanguage.EPL;
            settings.StampsAutomaticExpress1 = false;
            settings.StampsAutomaticExpress1Account = 0;
            settings.StampsUspsAutomaticExpedited = false;
            settings.StampsUspsAutomaticExpeditedAccount = 0;

            settings.EquaShipThermal = false;
            settings.EquaShipThermalType = (int) ThermalLanguage.EPL;

            settings.OnTracThermal = false;
            settings.OnTracThermalType = (int) ThermalLanguage.EPL;
            settings.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            settings.OnTracInsurancePennyOne = false;

            settings.IParcelThermal = false;
            settings.IParcelThermalType = (int)ThermalLanguage.EPL;
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

            settings.UspsThermal = false;
            settings.UspsThermalType = (int)ThermalLanguage.EPL;

            adapter.SaveAndRefetch(settings);
        }
    }
}
