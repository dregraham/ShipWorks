using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Tool to build collection with package type number and string based on shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.DhlEcommerce, SingleInstance = true)]
    public class DhlEcommerceShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly DhlEcommerceShipmentType dhlEcommerceShipmentType;
        private readonly IExcludedPackageTypeRepository excludedPackageTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentPackageTypesBuilder(DhlEcommerceShipmentType dhlEcommerceShipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository)
        {
            this.dhlEcommerceShipmentType = dhlEcommerceShipmentType;
            this.excludedPackageTypeRepository = excludedPackageTypeRepository;
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments) => dhlEcommerceShipmentType.BuildPackageTypeDictionary(shipments.ToList(), excludedPackageTypeRepository);
    }
}
