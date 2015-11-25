using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Tool to build collection with package type number and string based on shipments
    /// </summary>
    public class FedExShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        private readonly FedExShipmentType fedExShipmentType;
        private readonly IFedExUtility fedExUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipmentPackageTypesBuilder(FedExShipmentType fedExShipmentType, IFedExUtility fedExUtility)
        {
            this.fedExShipmentType = fedExShipmentType;
            this.fedExUtility = fedExUtility;
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> shipmentList = shipments?.ToList() ?? new List<ShipmentEntity>();

            List<FedExServiceType> distinctShipmentTypes = shipmentList.Select(shipment => (FedExServiceType)shipment.FedEx.Service).Distinct().ToList();

            if (distinctShipmentTypes.Count != 1)
            {
                return new Dictionary<int, string>()
                {
                    { (int) FedExPackagingType.Custom, EnumHelper.GetDescription(FedExPackagingType.Custom) }
                };
            }

            List<FedExPackagingType> validPackagingTypes = fedExUtility.GetValidPackagingTypes(distinctShipmentTypes.First());

            IEnumerable<FedExPackagingType> availablePackageTypes =
                fedExShipmentType.GetAvailablePackageTypes()
                    .Union(shipmentList.Select(x => x.FedEx)
                        .Where(x => x != null)
                        .Select(x => x.PackagingType))
                    .Cast<FedExPackagingType>();

            return validPackagingTypes
                .Intersect(availablePackageTypes)
                .DefaultIfEmpty(validPackagingTypes.FirstOrDefault())
                .ToDictionary(p => (int) p, p => EnumHelper.GetDescription(p));
        }
    }
}
