using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for settings needed by the Shipping Panel
    /// </summary>
    public interface IShippingPanelConfigurator
    {
        /// <summary>
        /// Wraps a call to determine if a user has permission on an entity
        /// </summary>
        bool UserHasPermission(PermissionType permissionType, long entityID);

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        bool GetAddressValidation(ShipmentEntity shipment);

        /// <summary>
        /// Wraps ShippingSettings.AutoCreateShipments
        /// </summary>
        bool AutoCreateShipments { get; }
    }
}
