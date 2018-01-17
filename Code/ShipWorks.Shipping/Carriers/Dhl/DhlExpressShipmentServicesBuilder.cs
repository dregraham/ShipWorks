using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Service type builder for DHL Express shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentServicesBuilder), ShipmentTypeCode.DhlExpress, SingleInstance = true)]
    public class DhlExpressShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly DhlExpressShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipmentServicesBuilder(DhlExpressShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
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
            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<DhlExpressServiceType>()
                .Union(shipments?.Select(shipment => shipment.DhlExpress)
                    .Where(dhlShipment => dhlShipment != null)
                    .Select(dhlShipment => (DhlExpressServiceType) dhlShipment.Service) ??
                        Enumerable.Empty<DhlExpressServiceType>())
                .ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));
        }
    }
}
