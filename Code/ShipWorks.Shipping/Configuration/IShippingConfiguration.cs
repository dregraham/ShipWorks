using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Configuration
{
    /// <summary>
    /// Interface for settings needed by the Shipping activities
    /// </summary>
    public interface IShippingConfiguration
    {
        /// <summary>
        /// Wraps a call to determine if a user has permission on an entity
        /// </summary>
        bool UserHasPermission(PermissionType permissionType, long entityID);

        /// <summary>
        /// Determines address validation for a shipment
        /// </summary>
        bool GetAddressValidation(ShipmentEntity shipment);

        /// <summary>
        /// Wraps ShippingSettings.AutoCreateShipments
        /// </summary>
        bool AutoCreateShipments { get; }
    }
}
