using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Service type builder for AmazonSWA shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentServicesBuilder), ShipmentTypeCode.AmazonSWA, SingleInstance = true)]
    public class AmazonSWAShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly AmazonSWAShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentServicesBuilder(AmazonSWAShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Build the dictionary of service type values and names
        /// </summary>
        /// <remarks>
        /// Gets all AmazonSWA service types, removes excluded ones, then adds any service types from the list of
        /// shipments if they are not already in the list
        /// </remarks>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<AmazonSWAServiceType>()
                .Union(shipments?.Select(shipment => shipment.AmazonSWA)
                    .Where(AmazonSWAShipment => AmazonSWAShipment != null)
                    .Select(AmazonSWAShipment => (AmazonSWAServiceType) AmazonSWAShipment.Service) ??
                        Enumerable.Empty<AmazonSWAServiceType>())
                .ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));
        }
    }
}
