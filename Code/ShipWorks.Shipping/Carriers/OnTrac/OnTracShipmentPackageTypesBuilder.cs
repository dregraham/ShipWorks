using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Tool to build collection with package type number and string based on shipments
    /// </summary>
    public class OnTracShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly IExcludedPackageTypeRepository excludedPackageTypeRepository;
        private readonly OnTracShipmentType shipmentType;

        public OnTracShipmentPackageTypesBuilder(OnTracShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository)
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
