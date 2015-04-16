using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using log4net;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries
{
    /// <summary>
    /// An implementation of the IUpsServiceManager interface for shipments originating in Canada.
    /// </summary>
    public class UpsCanadaServiceManager : IUpsServiceManager
    {
        private const string CanadaCountryCode = "CA";
        private const string USCountryCode = "US";
        private const string PuertoRicoCountryCode = "PR";

        // Just used as a catch-all for all other countries
        private const string InternationalCountryCode = "International"; 

        private readonly List<UpsServiceMapping> serviceMappings;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCanadaServiceManager"/> class.
        /// </summary>
        public UpsCanadaServiceManager()
            : this(LogManager.GetLogger(typeof (UpsCanadaServiceManager)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCanadaServiceManager"/> class. This
        /// constructor is used primarily for testing purposes.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UpsCanadaServiceManager(ILog logger)
        {
            serviceMappings = new List<UpsServiceMapping>();
            log = logger;

            // Canada/domestic service mappings
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpress, CanadaCountryCode, "01", "01", "24", "ES", WorldShipServiceDescriptions.UpsExpress, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpedited, CanadaCountryCode, "02", "08", "19", "EX", WorldShipServiceDescriptions.UpsExpedited, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsStandard, CanadaCountryCode, "11", "11", "25", "ST", WorldShipServiceDescriptions.UpsStandard, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpressSaver, CanadaCountryCode, "13", "65", "20", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpressEarlyAm, CanadaCountryCode, "14", "14", "23", "1DM", WorldShipServiceDescriptions.UpsExpressEarlyAm, false, false));

            // US service mappings
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpressSaver, USCountryCode, "13", "65", "28", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpressEarlyAm, USCountryCode, "54", "14", "21", "1DM", WorldShipServiceDescriptions.UpsExpressEarlyAm, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsStandard, USCountryCode, "11", "11", "03", "ST", WorldShipServiceDescriptions.UpsStandard, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpress, USCountryCode, "07", "01", "01", "ES", WorldShipServiceDescriptions.UpsExpress, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpedited, USCountryCode, "02", "08", "05", "EX", WorldShipServiceDescriptions.UpsExpedited, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.Ups3DaySelectFromCanada, USCountryCode, "12", "12", "33", "3DM", WorldShipServiceDescriptions.Ups3DaySelectFromCanada, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsCaWorldWideExpressSaver, USCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false));
            
            // Puerto Rico service mappings
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpedited, PuertoRicoCountryCode, "02", "08", "05", "EX", WorldShipServiceDescriptions.UpsExpedited, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsCaWorldWideExpressSaver, PuertoRicoCountryCode, "28", "65", "65", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false));
            
            // International service mappings
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsCaWorldWideExpress, InternationalCountryCode,  "07", "07", "01", "ES", WorldShipServiceDescriptions.UpsExpress, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsCaWorldWideExpressPlus, InternationalCountryCode, "54", "54", "21", "EP", WorldShipServiceDescriptions.WorldwideExpressPlus, false, false));
            serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsCaWorldWideExpressSaver, InternationalCountryCode, "65", "65", "28", "SV", WorldShipServiceDescriptions.UpsExpressSaver, false, false));
            //serviceMappings.Add(new UpsServiceMapping(UpsServiceType.UpsExpressEarlyAm, InternationalCountryCode, "14", "14", "54", "1DM", WorldShipServiceDescriptions.UpsExpressEarlyAm, false, false));
        }
        
        /// <summary>
        /// Gets the country code associated with this CarrierServiceManager.
        /// </summary>
        public string CountryCode
        {
            get
            {
                return "CA";
            }
        }

        /// <summary>
        /// Gets the a List of UpsServiceMapping that are available based on shipment data provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of the available services.</returns>
        public List<UpsServiceMapping> GetServices(ShipmentEntity shipment)
        {
            // Find the valid service types based on the ship country code
            List<UpsServiceMapping> servicesForShipment = serviceMappings.Where(m => m.DestinationCountryCode.ToUpperInvariant() == shipment.AdjustedShipCountryCode().ToUpperInvariant()).ToList();

            if (!servicesForShipment.Any())
            {
                // No services for the shipment were found based on the ship country code, so
                // use the international mappings
                servicesForShipment = serviceMappings.Where(m => m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant()).ToList();
            }

            return servicesForShipment;
        }

        /// <summary>
        /// Translates the given rate code and country code into the appropriate UpsServiceMapping.
        /// </summary>
        /// <param name="rateCode">The rate code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        /// <exception cref="UpsException"></exception>
        public UpsServiceMapping GetServicesByRateCode(string rateCode, string destinationCountryCode)
        {
            // Try to get the mapping based on the country code and rate code provided
            UpsServiceMapping mapping = serviceMappings.FirstOrDefault(m => m.RateServiceCode.ToUpperInvariant() == rateCode.ToUpperInvariant() 
                                                                    && m.DestinationCountryCode.ToUpperInvariant() == destinationCountryCode.ToUpperInvariant());
            
            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = serviceMappings.FirstOrDefault(m => m.RateServiceCode.ToUpperInvariant() == rateCode.ToUpperInvariant() 
                                                    && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }
            
            if (mapping == null)
            {
                // Canada is messed up - per the UPS Solutions expert, sometimes the rate code can is the transit code, so check 
                // the transit code if we haven't found the mapping yet
                mapping = GetServiceByTransitCode(rateCode, destinationCountryCode);
            }

            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown rate code was provided for a shipment originating in Canada having a destination of {0}. Rate code: {1}", destinationCountryCode, rateCode));
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
        /// <exception cref="UpsException"></exception>
        public UpsServiceMapping GetServicesByWorldShipDescription(string description, string destinationCountryCode)
        {
            UpsServiceMapping mapping = serviceMappings.FirstOrDefault(m => m.WorldShipDescription.ToUpperInvariant() == description.ToUpperInvariant() 
                                                && m.DestinationCountryCode.ToUpperInvariant() == destinationCountryCode.ToUpperInvariant());

            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = serviceMappings.FirstOrDefault(m => m.WorldShipDescription.ToUpperInvariant() == description.ToUpperInvariant() 
                                                && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }

            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown WorldShip description provided for a shipment originating in Canada having a destination of {0}. WorldShip description: {1}", destinationCountryCode, description));
                throw new UpsException(string.Format("An unknown WorldShip description was provided for a shipment with a(n) {0} destination.", destinationCountryCode));
            }

            return mapping;
        }


        /// <summary>
        /// Gets the service by transit code.
        /// </summary>
        /// <param name="transitCode">The transit code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        /// <exception cref="UpsException"></exception>
        public UpsServiceMapping GetServiceByTransitCode(string transitCode, string destinationCountryCode)
        {
            // Try to get the mapping based on the country code and rate code provided
            UpsServiceMapping mapping = serviceMappings.FirstOrDefault(m => m.TransitServiceCode.ToUpperInvariant() == transitCode.ToUpperInvariant()
                                                                            && m.DestinationCountryCode.ToUpperInvariant() == destinationCountryCode.ToUpperInvariant());

            if (mapping == null)
            {
                // No services for the shipment were found based on the ship country code, so try to use the international mappings
                mapping = serviceMappings.FirstOrDefault(m => m.TransitServiceCode.ToUpperInvariant() == transitCode.ToUpperInvariant()
                                                              && m.DestinationCountryCode.ToUpperInvariant() == InternationalCountryCode.ToUpperInvariant());
            }

            if (mapping == null)
            {
                // Still didn't find a mapping - throw an exception
                log.Error(string.Format("Unknown transit code provided for a shipment originating in Canada having a destination of {0}. Transit code: {1}", destinationCountryCode, transitCode));
                throw new UpsException(string.Format("An unknown transit code was provided for a shipment with a(n) {0} destination.", destinationCountryCode));
            }

            return mapping;
        }
    }
}
