using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Service type builder for Postal shipments
    /// </summary>
    public class PostalShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly IExcludedPackageTypeRepository excludedPackageTypeRepository;
        private readonly PostalShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PostalShipmentPackageTypesBuilder(PostalShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedPackageTypeRepository = excludedPackageTypeRepository;
        }

        /// <summary>
        /// Gets the package type diction for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return shipmentType.BuildPackageTypeDictionary(shipments.ToList(), excludedPackageTypeRepository);
        }
    }
}
