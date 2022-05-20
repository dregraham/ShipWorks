using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Service type builder for DHL eCommerce shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentServicesBuilder), ShipmentTypeCode.DhlEcommerce, SingleInstance = true)]
    public class DhlEcommerceShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly DhlEcommerceShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentServicesBuilder(DhlEcommerceShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository, IShippingManager shippingManager)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Build the dictionary of service type values and names
        /// </summary>
        /// <remarks>
        /// Gets all DHl service types, removes excluded ones, then adds any service types from the list of 
        /// shipments if they are not already in the list
        /// </remarks>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> overriddenShipments = shipments.Select(shippingManager.GetOverriddenStoreShipment).ToList();

            bool allInternational = overriddenShipments.None(shipmentType.IsDomestic);
            bool allDomestic = overriddenShipments.All(shipmentType.IsDomestic);
            bool allCanada = overriddenShipments.All(shipment => shipment.AdjustedShipCountryCode() == "CA");

            if (!allInternational && !allDomestic && !allCanada)
            {
                //If the shipments are mixed international and domestic return an empty dictionary
                //because we can't assign a single service to all the shipments
                return new Dictionary<int, string>();
            }

            var availableServices = shipmentType
                .GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<DhlEcommerceServiceType>().ToList();

            IEnumerable<DhlEcommerceServiceType> validServices = new List<DhlEcommerceServiceType>();

            if (allCanada)
            {
                validServices = availableServices.Where(s => EnumHelper.GetInternationalServiceAttribute(s).IsInternational);
            }
            else if (allInternational)
            {
                validServices = availableServices.Where(s => {
                    var serviceAttribute = EnumHelper.GetInternationalServiceAttribute(s);
                    return serviceAttribute.IsInternational && string.IsNullOrEmpty(serviceAttribute.CountryCodeRestriction);
                    });
            }
            else if (allDomestic)
            {
                validServices = availableServices.Where(s => !EnumHelper.GetInternationalServiceAttribute(s).IsInternational);
            }

            if (shipments.Any())
            {
                // Always include the service type that the shipment is currently configured in the 
                // event the shipment was configured prior to a service being excluded
                // Always include the service that the shipments are currently configured with
                // Only if the service type is a validServiceType
                var loadedServices = shipments.Select(s => (DhlEcommerceServiceType) s.DhlEcommerce.Service).Intersect(Enum.GetValues(typeof(DhlEcommerceServiceType)).Cast<DhlEcommerceServiceType>());
                validServices = validServices.Union(loadedServices);
            }

            return validServices.ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));
        }
    }
}
