using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// An interface for handling the different service types offered by a carrier (e.g. listing
    /// available services, translating between ShipWorks-centric values and carrier API values, etc.).
    /// </summary>
    public interface IUpsServiceManager
    {
        /// <summary>
        /// Gets the country code associated with this CarrierServiceManager.
        /// </summary>
        string CountryCode
        {
            get;
        }

        /// <summary>
        /// Gets the a List of UpsServiceMapping that are available based on shipment data provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of the available services.</returns>
        List<UpsServiceMapping> GetServices(ShipmentEntity shipment);

        /// <summary>
        /// Translates the given rate code and country code into the appropriate UpsServiceMapping.
        /// </summary>
        /// <param name="rateCode">The rate code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        UpsServiceMapping GetServicesByRateCode(string rateCode, string destinationCountryCode);

        /// <summary>
        /// Translates the given description and country code into the appropriate UpsServiceMapping.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        IUpsServiceMapping GetServicesByWorldShipDescription(string description, string destinationCountryCode);

        /// <summary>
        /// Gets the service by transit code.
        /// </summary>
        /// <param name="transitCode">The transit code.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <returns>An UpsServiceMapping object.</returns>
        UpsServiceMapping GetServiceByTransitCode(string transitCode, string destinationCountryCode);
    }
}
