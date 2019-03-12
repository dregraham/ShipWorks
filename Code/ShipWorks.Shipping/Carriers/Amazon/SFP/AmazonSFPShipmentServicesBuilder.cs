﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Collections;
using System.Collections.Specialized;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Gets list of service types available for a selected Amazon shipment
    /// </summary>
    public class AmazonSFPShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly AmazonSFPShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly IAmazonSFPServiceTypeRepository amazonServiceTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSFPShipmentServicesBuilder"/> class.
        /// </summary>
        public AmazonSFPShipmentServicesBuilder(AmazonSFPShipmentType shipmentType,
            IExcludedServiceTypeRepository excludedServiceTypeRepository,
            IAmazonSFPServiceTypeRepository amazonServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.amazonServiceTypeRepository = amazonServiceTypeRepository;
        }

        /// <summary>
        /// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            // Get all of the services from the shipments
            IEnumerable<string> shipmentsServices = shipments.Select(s => s.Amazon?.ShippingServiceID).Distinct();

            // Get all of the amazon services
            List<AmazonServiceTypeEntity> allServiceTypes = amazonServiceTypeRepository.Get();

            // Build a dictionary of all of AmazonServiceTypeID and Description values using the given shipments ShippingServiceID
            Dictionary<int, string> shipmentsServiceTypes = allServiceTypes?
                .Where(s => shipmentsServices.Contains(s.ApiValue))
                .ToDictionary(s => s.AmazonServiceTypeID, s => s.Description) ?? new Dictionary<int, string>();

            // combine that with a a dictionary of available services
            IEnumerable<KeyValuePair<int, string>> services = GetAvailableServiceTypes(allServiceTypes).Union(shipmentsServiceTypes);

            return GetSortedServiceTypeDictionary(services);
        }

        /// <summary>
        /// Get a sorted Dictionary from the serviceTypes
        /// </summary>
        private Dictionary<int, string> GetSortedServiceTypeDictionary(IEnumerable<KeyValuePair<int, string>> serviceTypes)
        {
            List<KeyValuePair<int, string>> uspsServices = serviceTypes.Where(s => s.Value.ToUpper().StartsWith("USPS")).ToList();

            IEnumerable<KeyValuePair<int, string>> everythingElse = serviceTypes.Where(s => !uspsServices.Contains(s)).OrderBy(v => v.Value.Split(' ')[0]);

            uspsServices.AddRange(everythingElse);
            
            return uspsServices.ToDictionary(s => s.Key, s => s.Value);
        }

        /// <summary>
        /// Get all of the services which are available from the given list of services
        /// </summary>
        private Dictionary<int, string> GetAvailableServiceTypes(List<AmazonServiceTypeEntity> allServiceTypes)
        {
            // Get all of the avaialble services from the shipment type (this excludes the ones that have been excluded by the user)
            IEnumerable<int> availableServices = shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository);

            // Build a dictionary of AmazonServiceTypeID and Description from the available service types
            return allServiceTypes.Where(s => availableServices.Contains(s.AmazonServiceTypeID))
                .ToDictionary(s => s.AmazonServiceTypeID, s => s.Description);
        }
    }
}