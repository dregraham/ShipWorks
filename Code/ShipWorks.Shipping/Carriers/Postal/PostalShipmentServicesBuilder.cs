﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Service type builder for Postal shipments
    /// </summary>
    public class PostalShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly PostalShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PostalShipmentServicesBuilder(PostalShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Build a list of service types available for the shipments
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> overriddenShipments = shipments.Select(ShippingManager.GetOverriddenStoreShipment).ToList();

            bool allInternational = overriddenShipments.None(s => s.ShipPerson.IsDomesticCountry());
            bool allDomestic = overriddenShipments.All(s => s.ShipPerson.IsDomesticCountry());

            List<PostalServiceType> availableServices = shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<PostalServiceType>()
                .Where(x => EnumHelper.GetDeprecated(x) == false && !x.IsHiddenFor(HiddenForContext.NewShipment))
                .ToList();

            // If they are all international we can load up all the international services
            if (allInternational)
            {
                // We need to build the list of international services to use as the data source taking into account only the available
                // service types as well as the service type that the shipment is already configured with
                List<PostalServiceType> allInternationalServices = PostalUtility.GetInternationalServices(shipmentType.ShipmentTypeCode);
                List<PostalServiceType> internationalServicesToLoad = allInternationalServices.Intersect(availableServices).ToList();
                if (shipments.Any())
                {
                    // Always include the service type that the shipment is currently configured in the
                    // event the shipment was configured prior to a service being excluded
                    internationalServicesToLoad =
                        internationalServicesToLoad.Union(new List<PostalServiceType>
                        {
                            (PostalServiceType) shipments.First().Postal.Service
                        }).ToList();
                }

                // Bind the drop down to the international services
                return internationalServicesToLoad.ToDictionary(s => (int) s, PostalUtility.GetPostalServiceTypeDescription);
            }

            // If they are all domestic we can load up all the domestic services
            if (allDomestic)
            {
                // We need to build the list of domestic services to use as the data source taking into account only the available
                // service types as well as the service type that the shipment is already configured with
                List<PostalServiceType> allDomesticServices = PostalUtility.GetDomesticServices(shipmentType.ShipmentTypeCode);
                List<PostalServiceType> domesticServicesToLoad = allDomesticServices.Intersect(availableServices).ToList();
                if (shipments.Any())
                {
                    // Always include the service type that the shipment is currently configured in the
                    // event the shipment was configured prior to a service being excluded
                    domesticServicesToLoad = domesticServicesToLoad.Union(allDomesticServices.Intersect(new List<PostalServiceType> { (PostalServiceType) shipments.First().Postal.Service })).ToList();
                }

                return domesticServicesToLoad.ToDictionary(s => (int) s, PostalUtility.GetPostalServiceTypeDescription);
            }

            // Otherwise there is nothing to choose from
            return new Dictionary<int, string>();
        }
    }
}
