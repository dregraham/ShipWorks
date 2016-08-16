using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using log4net;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries
{
    /// <summary>
    /// Ups PR Service Manager
    /// </summary>
    public class UpsPuertoRicoServiceManager : IUpsServiceManager
    {
        private const string usCountryCode = "US";
        private const string puertoRicoCountryCode = "PR";
        private const string internationalCode = "International";

        List<UpsServiceMapping> services;

        readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPuertoRicoServiceManager"/> class.
        /// </summary>
        public UpsPuertoRicoServiceManager()
            : this(LogManager.GetLogger(typeof(UpsPuertoRicoServiceManager)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPuertoRicoServiceManager"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public UpsPuertoRicoServiceManager(ILog log)
        {
            this.log = log;

            services = new List<UpsServiceMapping>
            {
                new UpsServiceMapping(UpsServiceType.Ups2ndDayAirIntra, puertoRicoCountryCode, "01", "01", "01", "2DA", WorldShipServiceDescriptions.Ups2DayAir, false, false),

                new UpsServiceMapping(UpsServiceType.UpsNextDayAirAM, usCountryCode, "14", "14", "21", "1DM", WorldShipServiceDescriptions.UpsNextDayAirAm, false, false),
                new UpsServiceMapping(UpsServiceType.UpsNextDayAir, usCountryCode, "01", "01", "01", "1DA", WorldShipServiceDescriptions.UpsNextDayAir, false, false),
                new UpsServiceMapping(UpsServiceType.Ups2DayAir, usCountryCode, "02", "02", "02", "2DA", WorldShipServiceDescriptions.Ups2DayAir, false, false),
                new UpsServiceMapping(UpsServiceType.UpsGround, usCountryCode, "03", "03", "G", "GND", WorldShipServiceDescriptions.Ground, false, false),

                new UpsServiceMapping(UpsServiceType.WorldwideExpressPlus, internationalCode, "54", "54", "21", "EP", WorldShipServiceDescriptions.WorldwideExpressPlus, false, false),
                new UpsServiceMapping(UpsServiceType.WorldwideExpress, internationalCode, "07", "01", "01", "ES", WorldShipServiceDescriptions.WorldwideExpress, false, false),
                new UpsServiceMapping(UpsServiceType.UpsExpressSaver, internationalCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false),
                new UpsServiceMapping(UpsServiceType.WorldwideExpedited, internationalCode, "08", "08", "05", "EX", WorldShipServiceDescriptions.WorldwideExpedited, false, false)
            };
        }

        /// <summary>
        /// Gets the country code associated with this CarrierServiceManager.
        /// </summary>
        public string CountryCode
        {
            get
            {
                return "PR";
            }
        }

        /// <summary>
        /// Gets the a List of UpsServiceMapping that are available based on shipment data provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of the available services.</returns>
        public List<UpsServiceMapping> GetServices(ShipmentEntity shipment)
        {
            return services
                .Where(s => s.DestinationCountryCode == shipment.AdjustedShipCountryCode() || s.DestinationCountryCode == internationalCode)
                .ToList();
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
            UpsServiceMapping upsServiceMapping = services.SingleOrDefault(x => x.RateServiceCode == rateCode && x.DestinationCountryCode == destinationCountryCode);

            if (upsServiceMapping == null)
            {
                upsServiceMapping = services.SingleOrDefault(x => x.RateServiceCode == rateCode && x.DestinationCountryCode == internationalCode);
            }

            if (upsServiceMapping==null)
            {
                log.Error("Could not find rateCode '" + rateCode + "' in UpsPuertoRicoServiceManager.GetByRateCode");
                throw new UpsException("Invalid Rate returned by UPS.");
            }

            return upsServiceMapping;
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
            UpsServiceMapping upsServiceMapping = services.SingleOrDefault(x => x.WorldShipDescription == description && x.DestinationCountryCode == destinationCountryCode);

            if (upsServiceMapping == null)
            {
                upsServiceMapping = services.SingleOrDefault(x => x.WorldShipDescription == description && x.DestinationCountryCode == internationalCode);
            }

            if (upsServiceMapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown WorldShip description was provided for a shipment originating in Puerto Rico having a destination of {0}. Rate code: {1}", destinationCountryCode, description));
                throw new UpsException(string.Format("An unknown WorldShip description was provided for a shipment with a(n) {0} destination.", destinationCountryCode));
            }

            return upsServiceMapping;
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
            UpsServiceMapping upsServiceMapping;

            upsServiceMapping = services.SingleOrDefault(x => x.TransitServiceCode == transitCode && x.DestinationCountryCode == destinationCountryCode);
            
            if (upsServiceMapping == null)
            {
                upsServiceMapping = services.SingleOrDefault(x => x.TransitServiceCode == transitCode && x.DestinationCountryCode == internationalCode);
            }

            if (upsServiceMapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown Transit Code was provided for a shipment originating in Puerto Rico having a destination of {0}. Rate code: {1}", destinationCountryCode, transitCode));
                throw new UpsException(string.Format("An unknown Transit code was provided for a shipment with a(n) {0} destination.", destinationCountryCode));
            }

            return upsServiceMapping;
        }
    }
}
