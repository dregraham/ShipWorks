using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.DhlEcommerce, SingleInstance = true)]
    public class DhlEcommerceShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly DhlEcommerceShipmentType DhlEcommerceShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentPackageTypesBuilder(DhlEcommerceShipmentType DhlEcommerceShipmentType)
        {
            this.DhlEcommerceShipmentType = DhlEcommerceShipmentType;;
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return Enum.GetValues(typeof(DhlEcommercePackagingType)).Cast<DhlEcommercePackagingType>()
                .ToDictionary(p => (int) p, p => EnumHelper.GetDescription(p));
        }
    }
}
