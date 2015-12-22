using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Settings;
using log4net;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries
{
    /// <summary>
    /// Ups US Service Manager
    /// </summary>
    public class UpsUsServiceManager : IUpsServiceManager
    {
        private readonly ShipmentEntity shipment;

        private const string CaCountryCode = "CA";
        private const string UsCountryCode = "US";
        private const string PrCountryCode = "PR";
        private const string GuCountryCode = "GU";
        private const string AsCountryCode = "AS";
        private const string MhCountryCode = "MH";
        private const string FmCountryCode = "FM";
        private const string MpCountryCode = "MP";
        private const string PwCountryCode = "PW";
        private const string ViCountryCode = "VI";
        private const string MxCountryCode = "MX";

        // Just used as a catch-all for all other countries
        private const string InternationalCountryCode = "INTERNATIONAL";

        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCanadaServiceManager"/> class.
        /// </summary>
        /// <param name="shipment"></param>
        public UpsUsServiceManager(ShipmentEntity shipment)
        {
            this.shipment = shipment;
            log = LogManager.GetLogger(typeof(UpsUsServiceManager));
        }

        /// <summary>
        /// Gets the country code associated with this CarrierServiceManager.
        /// </summary>
        public string CountryCode
        {
            get
            {
                return "US";
            }
        }

        /// <summary>
        /// Gets the a List of UpsServiceMapping that are available based on shipment data provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of the available services.</returns>
        public List<UpsServiceMapping> GetServices(ShipmentEntity shipment)
        {
            return GetServiceTypes(shipment.AdjustedShipCountryCode(), (ShipmentTypeCode) shipment.ShipmentType);
        }

        /// <summary>
        /// Translates the given rate code and country code into the appropriate UpsServiceMapping.
        /// </summary>
        /// <param name="rateCode">The rate code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public UpsServiceMapping GetServicesByRateCode(string rateCode, string destinationCountryCode)
        {
            UpsServiceMapping mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.DestinationCountryCode == destinationCountryCode.Trim().ToUpperInvariant()
                                                               && m.RateServiceCode == rateCode.Trim().ToUpperInvariant());

            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.RateServiceCode.ToUpperInvariant() == rateCode.ToUpperInvariant()
                                                    && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }
            
            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown rate code was provided for a shipment originating in the US having a destination of {0}. Rate code: {1}", destinationCountryCode, rateCode));
                throw new UpsException(string.Format("An unknown rate code was provided for a shipment with a destination of {0}.", destinationCountryCode));
            }

            return mapping;
        }

        /// <summary>
        /// Translates the given description and country code into the appropriate UpsServiceMapping.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public UpsServiceMapping GetServicesByWorldShipDescription(string description, string destinationCountryCode)
        {
            UpsServiceMapping mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.DestinationCountryCode == destinationCountryCode.Trim().ToUpperInvariant()
                                                               && m.WorldShipDescription.ToUpperInvariant() == description.Trim().ToUpperInvariant());

            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.WorldShipDescription.ToUpperInvariant() == description.Trim().ToUpperInvariant()
                                                && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }

            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown WorldShip description provided for a shipment originating in the US having a destination of {0}. WorldShip description: {1}", destinationCountryCode, description));
                throw new UpsException(string.Format("An unknown WorldShip description was provided for a shipment with a(n) {0} destination.", destinationCountryCode));
            }

            return mapping;
        }

        /// <summary>
        /// Get the list of valid services for the given country
        /// </summary>
        private List<UpsServiceMapping> GetServiceTypes(string countryCode, ShipmentTypeCode shipmentTypeCode)
        {
            UpsShipmentType shipmentType = (UpsShipmentType)ShipmentTypeManager.GetType(shipmentTypeCode);

            // See if the requested country code has specific services defined
            bool hasCountryCode = LoadUpsServiceMappings().Any(stm => stm.DestinationCountryCode == countryCode.ToUpperInvariant());

            // Add SurePost if enabled.
            if (!hasCountryCode)
            {
                // It didn't, so default to the international services
                countryCode = InternationalCountryCode;
            }

            if (shipmentType.IsMailInnovationsEnabled())
            {
                return LoadUpsServiceMappings().Where(stm => stm.DestinationCountryCode == countryCode.ToUpperInvariant()).Distinct().ToList();
            }
            else
            {
                return LoadUpsServiceMappings().Where(stm => stm.IsMailInnovations == false && stm.DestinationCountryCode == countryCode.ToUpperInvariant()).Distinct().ToList();
            }
        }

        /// <summary>
        /// Load our cache of UpsServiceMappings
        /// </summary>
        [NDependIgnoreLongMethod]
        private List<UpsServiceMapping> LoadUpsServiceMappings()
        {
            List<UpsServiceMapping> tmpUpsServiceTypeMapping = new List<UpsServiceMapping>();

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsGround, UsCountryCode, "03", "03", "GND", "GND", WorldShipServiceDescriptions.Ground, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.Ups3DaySelect, UsCountryCode, "12", "12", "3DS", "3DS", WorldShipServiceDescriptions.Ups3DaySelect, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.Ups2DayAir, UsCountryCode, "02", "02", "2DA", "2DA", WorldShipServiceDescriptions.Ups2DayAir, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.Ups2DayAirAM, UsCountryCode, "59", "59", "2DM", "2DM", WorldShipServiceDescriptions.Ups2DayAirAm, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsNextDayAir, UsCountryCode, "01", "01", "1DA", "1DA", WorldShipServiceDescriptions.UpsNextDayAir, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsNextDayAirSaver, UsCountryCode, "13", "13", "1DP", "1DP", WorldShipServiceDescriptions.UpsNextDayAirSaver, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsNextDayAirAM, UsCountryCode, "14", "14", "1DM", "1DM", WorldShipServiceDescriptions.UpsNextDayAirAm, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(UsCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsGround, PrCountryCode, "03", "03","GND", "GND", WorldShipServiceDescriptions.Ground, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.Ups2DayAir, PrCountryCode, "02", "02", "2DA", "2DA", WorldShipServiceDescriptions.Ups2DayAir, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsNextDayAir, PrCountryCode, "01", "01", "01PR", "1DA", WorldShipServiceDescriptions.UpsNextDayAir, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(PrCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, GuCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(GuCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, AsCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(AsCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, MhCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(MhCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, FmCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(FmCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, MpCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(MpCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, PwCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(PwCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, ViCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.AddRange(GetQualifyingSurePostServices(ViCountryCode));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpress, CaCountryCode, "07", "07", "01", "ES", WorldShipServiceDescriptions.WorldwideExpress, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, CaCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpedited, CaCountryCode, "08", "08", "19", "EX", WorldShipServiceDescriptions.WorldwideExpedited, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsStandard, CaCountryCode, "11", "11", "08", "ST", WorldShipServiceDescriptions.UpsStandard, false, false));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpress, MxCountryCode, "07", "07", "01", "ES", WorldShipServiceDescriptions.WorldwideExpress, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, MxCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpedited, MxCountryCode, "08", "08", "19", "EX", WorldShipServiceDescriptions.WorldwideExpedited, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsStandard, MxCountryCode, "11", "11", "08", "ST", WorldShipServiceDescriptions.UpsStandard, false, false));

            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpress, InternationalCountryCode, "07", "07", "01", "ES", WorldShipServiceDescriptions.WorldwideExpress, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideSaver, InternationalCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.WorldwideSaver, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpedited, InternationalCountryCode, "08", "08", "19", "EX", WorldShipServiceDescriptions.WorldwideExpedited, false, false));
            tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.WorldwideExpressPlus, InternationalCountryCode, "54", "54", "21", "EP", WorldShipServiceDescriptions.WorldwideExpressPlus, false, false));

            tmpUpsServiceTypeMapping.AddRange(GetMiUpsServiceMappings());

            return tmpUpsServiceTypeMapping;
        }

        /// <summary>
        /// Load our cache of Mail Innovations UpsServiceMappings
        /// </summary>
        private List<UpsServiceMapping> GetMiUpsServiceMappings()
        {
            List<UpsServiceMapping> tmpUpsServiceTypeMapping = new List<UpsServiceMapping>();

           // Add all of the Mail Innovations services
            if (((UpsShipmentType)ShipmentTypeManager.GetType(shipment)).IsMailInnovationsEnabled())
            {
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsExpedited, UsCountryCode, "M4", "M4", string.Empty, "MID", WorldShipServiceDescriptions.UpsMailInnovationsExpedited, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsFirstClass, UsCountryCode, "M2", "M2", string.Empty, "MIF", WorldShipServiceDescriptions.UpsMailInnovationsFirstClass, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsPriority, UsCountryCode, "M3", "M3", string.Empty, "MIT", WorldShipServiceDescriptions.UpsMailInnovationsPriority, true, false));

                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsExpedited, PrCountryCode, "", "M4", string.Empty, "MID", WorldShipServiceDescriptions.UpsMailInnovationsExpedited, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsFirstClass, PrCountryCode, "", "M2", string.Empty, "MIF", WorldShipServiceDescriptions.UpsMailInnovationsFirstClass, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsPriority, PrCountryCode, "", "M3", string.Empty, "MIT", WorldShipServiceDescriptions.UpsMailInnovationsPriority, true, false));

                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsExpedited, ViCountryCode, "", "M4", string.Empty, "MID", WorldShipServiceDescriptions.UpsMailInnovationsExpedited, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsFirstClass, ViCountryCode, "", "M2", string.Empty, "MIF", WorldShipServiceDescriptions.UpsMailInnovationsFirstClass, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsPriority, ViCountryCode, "", "M3", string.Empty, "MIT", WorldShipServiceDescriptions.UpsMailInnovationsPriority, true, false));

                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsExpedited, GuCountryCode, "", "M4", string.Empty, "MID", WorldShipServiceDescriptions.UpsMailInnovationsExpedited, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsFirstClass, GuCountryCode, "", "M2", string.Empty, "MIF", WorldShipServiceDescriptions.UpsMailInnovationsFirstClass, true, false));
                tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsPriority, GuCountryCode, "", "M3", string.Empty, "MIT", WorldShipServiceDescriptions.UpsMailInnovationsPriority, true, false));

                // Add MI International to the other country codes.
                // When we do the Contains(CountryCode), for CA for example, if we don't add the country codes here, no MI will be displayed because
                // the code finds the UPS OnlineTools entries with CA and doesn't set to international.
                List<string> countryCodes = new List<string> {  CaCountryCode,
                                                                AsCountryCode,
                                                                MhCountryCode,
                                                                FmCountryCode,
                                                                MpCountryCode,
                                                                PwCountryCode,
                                                                MxCountryCode,
                                                                InternationalCountryCode};
                foreach (var countryCode in countryCodes)
                {
                    tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsIntEconomy, countryCode, "M6", "M6", string.Empty, "MIE", WorldShipServiceDescriptions.UpsMailInnovationsIntEconomy, true, false));
                    tmpUpsServiceTypeMapping.Add(new UpsServiceMapping(UpsServiceType.UpsMailInnovationsIntPriority, countryCode, "M5", "M5", string.Empty, "MIP", WorldShipServiceDescriptions.UpsMailInnovationsIntPriority, true, false));
                }
            }

            return tmpUpsServiceTypeMapping;
        }

        /// <summary>
        /// Gets any SurePost services that can be used depending on the country code and any edition restrictions.
        /// </summary>
        /// <param name="destinationCountryCode">The country code.</param>
        /// <returns>A collection of available SurePost services.</returns>
        private static List<UpsServiceMapping> GetQualifyingSurePostServices(string destinationCountryCode)
        {
            List<UpsServiceMapping> surePostServices = new List<UpsServiceMapping>();
            bool isDomesticCountry = new AddressAdapter {CountryCode = destinationCountryCode}.IsDomesticCountry();

            // Postal domestic country restrictions apply to SurePost the service
            if (isDomesticCountry && EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.UpsSurePost).Level == EditionRestrictionLevel.None)
            {
                surePostServices.Add(new UpsServiceMapping(UpsServiceType.UpsSurePostLessThan1Lb, destinationCountryCode, "92", "92", string.Empty, "USL", WorldShipServiceDescriptions.UpsSurePostLessThan1Lb, false, true));
                surePostServices.Add(new UpsServiceMapping(UpsServiceType.UpsSurePost1LbOrGreater, destinationCountryCode, "93", "93", string.Empty, "USG", WorldShipServiceDescriptions.UpsSurePost1LbOrGreater, false, true));
                surePostServices.Add(new UpsServiceMapping(UpsServiceType.UpsSurePostBoundPrintedMatter, destinationCountryCode, "94", string.Empty, "94", "USB", WorldShipServiceDescriptions.UpsSurePostBoundPrintedMatter, false, true));
                surePostServices.Add(new UpsServiceMapping(UpsServiceType.UpsSurePostMedia, destinationCountryCode, "95", "95", "USM", string.Empty, WorldShipServiceDescriptions.UpsSurePostMedia, false, true));
            }

            return surePostServices;
        }


        /// <summary>
        /// Gets the service by transit code.
        /// </summary>
        /// <param name="transitCode">The transit code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public UpsServiceMapping GetServiceByTransitCode(string transitCode, string destinationCountryCode)
        {
            UpsServiceMapping mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.DestinationCountryCode == destinationCountryCode.Trim().ToUpperInvariant()
                                                               && m.TransitServiceCode == transitCode.Trim().ToUpperInvariant());

            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = LoadUpsServiceMappings().FirstOrDefault(m => m.TransitServiceCode.ToUpperInvariant() == transitCode.ToUpperInvariant()
                                                    && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }

            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown transit code was provided for a shipment originating in the US having a destination of {0}. Rate code: {1}", destinationCountryCode, transitCode));
                throw new UpsException(string.Format("An unknown transit code was provided for a shipment with a destination of {0}.", destinationCountryCode));
            }

            return mapping;
        }
    }
}
