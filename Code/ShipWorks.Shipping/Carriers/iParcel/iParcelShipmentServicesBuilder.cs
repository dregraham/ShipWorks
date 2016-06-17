using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using System.Linq;
using Interapptive.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Service type builder for iParcel shipments
    /// </summary>
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly iParcelShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType"></param>
        /// <param name="excludedServiceTypeRepository"></param>
        public iParcelShipmentServicesBuilder(iParcelShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Build the dictionary of service type values and names
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            // Always include the service that the shipments are currently configured with
            IEnumerable<iParcelServiceType> servicesUsedByShipments = shipments?.Select(s => (iParcelServiceType)s.IParcel.Service).Distinct() ?? 
                Enumerable.Empty<iParcelServiceType>();

            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<iParcelServiceType>()
                .Union(servicesUsedByShipments)
                .ToDictionary(service => (int)service, service => EnumHelper.GetDescription(service));
        }
    }
}