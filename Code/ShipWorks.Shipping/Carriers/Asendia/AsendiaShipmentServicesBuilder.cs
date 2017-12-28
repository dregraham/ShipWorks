using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Service type builder for Asendia shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentServicesBuilder), ShipmentTypeCode.Asendia, SingleInstance = true)]
    public class AsendiaShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly AsendiaShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaShipmentServicesBuilder(AsendiaShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Build the dictionary of service type values and names
        /// </summary>
        /// <remarks>
        /// Gets all Asendia service types, removes excluded ones, then adds any service types from the list of 
        /// shipments if they are not already in the list
        /// </remarks>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<AsendiaServiceType>()
                .Union(shipments?.Select(shipment => shipment.Asendia)
                    .Where(AsendiaShipment => AsendiaShipment != null)
                    .Select(AsendiaShipment => AsendiaShipment.Service) ??
                        Enumerable.Empty<AsendiaServiceType>())
                .ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));
        }
    }
}
