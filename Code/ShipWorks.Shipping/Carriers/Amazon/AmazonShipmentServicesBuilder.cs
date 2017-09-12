using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Gets list of service types available for a selected Amazon shipment
    /// </summary>
    public class AmazonShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly AmazonShipmentType shipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShipmentServicesBuilder"/> class.
        /// </summary>
        public AmazonShipmentServicesBuilder(AmazonShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            // Always include the service that the shipments are currently configured with
            IEnumerable<AmazonServiceType> servicesUsedByShipments =
                shipments?.Select(s => EnumHelper.GetEnumByApiValue<AmazonServiceType>(s.Amazon.ShippingServiceID))
                    .Distinct() ??
                Enumerable.Empty<AmazonServiceType>();

            return shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository)
                .Cast<AmazonServiceType>()
                .Union(servicesUsedByShipments)
                .ToDictionary(service => (int)service, service => EnumHelper.GetDescription(service));
        }
    }
}