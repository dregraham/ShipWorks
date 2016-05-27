using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Interface to wrap FedExUtility static class
    /// </summary>
    public interface IFedExUtility
    {
        /// <summary>
        /// Gets the valid service types.
        /// </summary>
        List<FedExServiceType> GetValidServiceTypes(IEnumerable<ShipmentEntity> overriddenShipments);

        /// <summary>
        /// Gets the valid packaging types.
        /// </summary>
        List<FedExPackagingType> GetValidPackagingTypes(FedExServiceType serviceType);
    }
}