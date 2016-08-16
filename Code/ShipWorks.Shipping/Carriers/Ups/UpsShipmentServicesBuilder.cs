using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using System;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public class UpsShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly Func<ShipmentEntity, IUpsServiceManagerFactory> serviceManagerFactory;
        private readonly UpsOltShipmentType shipmentType;

        public UpsShipmentServicesBuilder(UpsOltShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository,
            Func<ShipmentEntity, IUpsServiceManagerFactory> serviceManagerFactory)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.serviceManagerFactory = serviceManagerFactory;
        }

        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> overriddenShipments = shipments.Select(ShippingManager.GetOverriddenStoreShipment).ToList();

            List<UpsServiceType> availableServices = shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository).Select(s => (UpsServiceType)s).ToList();

            // If a distinct on country code only returns a count of 1, all countries are the same
            bool allSameCountry = overriddenShipments.Select(s => string.Format("{0} {1}", s.AdjustedShipCountryCode(), s.AdjustedOriginCountryCode())).Distinct().Count() == 1;

            // If they are all of the same service class, we can load the service classes
            if (allSameCountry)
            {
                ShipmentEntity overriddenShipment = overriddenShipments.First();

                IUpsServiceManager carrierServiceManager = serviceManagerFactory(overriddenShipment).Create(overriddenShipment);

                // Get a list of service types that are valid for the overriddenShipments
                List<UpsServiceType> validServiceTypes = carrierServiceManager.GetServices(overriddenShipment)
                    .Select(s => s.UpsServiceType).ToList();

                // only include service types that are valid and enabled (availalbeServices)
                List<UpsServiceType> upsServiceTypesToLoad = validServiceTypes.Intersect(availableServices).ToList();

                if (shipments.Any())
                {
                    // Always include the service type that the shipment is currently configured in the
                    // event the shipment was configured prior to a service being excluded
                    // Always include the service that the shipments are currently configured with
                    // Only if the ServiceType is valid for the shipment type
                    IEnumerable<UpsServiceType> loadedServices = shipments.Select(s => (UpsServiceType)s.Ups.Service)
                        .Intersect(validServiceTypes).Distinct();
                    upsServiceTypesToLoad = upsServiceTypesToLoad.Union(loadedServices).ToList();
                }

                return upsServiceTypesToLoad
                    .ToDictionary(type => (int)type, type => EnumHelper.GetDescription(type));
            }

            return new Dictionary<int, string>();
        }
    }
}