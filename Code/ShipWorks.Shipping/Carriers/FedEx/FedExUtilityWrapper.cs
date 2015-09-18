using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Wraps FedExUtility
    /// </summary>
    public class FedExUtilityWrapper : IFedExUtility
    {
        /// <summary>
        /// Gets the valid service types.
        /// </summary>
        public List<FedExServiceType> GetValidServiceTypes(IEnumerable<ShipmentEntity> overriddenShipments) =>
                    FedExUtility.GetValidServiceTypes(overriddenShipments.ToList());
    }
}