using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Package type builder for UPS WorldShip shipments
    /// </summary>
    public class WorldShipShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly IExcludedPackageTypeRepository excludedPackageTypeRepository;
        private readonly WorldShipShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipShipmentPackageTypesBuilder(WorldShipShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedPackageTypeRepository = excludedPackageTypeRepository;
        }

        /// <summary>
        /// Gets the package type dictionary for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return shipmentType.BuildPackageTypeDictionary(shipments.ToList(), excludedPackageTypeRepository);
        }
    }
}
