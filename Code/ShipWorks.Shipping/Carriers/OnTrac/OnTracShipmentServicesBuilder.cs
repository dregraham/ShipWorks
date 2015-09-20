using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Service type builder for OnTrac shipments
    /// </summary>
    public class OnTracShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly OnTracShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType"></param>
        /// <param name="excludedServiceTypeRepository"></param>
        public OnTracShipmentServicesBuilder(OnTracShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Build the dictionary of service type values and names
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<OnTracServiceType>()
                .Union(shipments?.Select(x => x.OnTrac)
                    .Where(x => x != null)
                    .Select(x => (OnTracServiceType)x.Service) ??
                    Enumerable.Empty<OnTracServiceType>())
                .Where(x => x != OnTracServiceType.None)
                .ToDictionary(serviceType => (int)serviceType, serviceType => EnumHelper.GetDescription(serviceType));
        }
    }
}