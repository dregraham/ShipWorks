using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Interface for settings needed by the Shipping activities
    /// </summary>
    public class ShippingConfiguration : IShippingConfiguration
    {
        /// <summary>
        /// Wraps a call to determine if a user has permission on an entity
        /// </summary>
        public bool UserHasPermission(PermissionType permissionType, long orderID)
        {
            return UserSession.Security.HasPermission(permissionType, orderID);
        }

        /// <summary>
        /// Determines address validation for a shipment
        /// </summary>
        public bool GetAddressValidation(ShipmentEntity shipment)
        {
            // TODO: Implement this
            return true;
        }

        /// <summary>
        /// Wraps ShippingSettings.AutoCreateShipments
        /// </summary>
        public bool AutoCreateShipments
        {
            get
            {
                return ShippingSettings.Fetch().AutoCreateShipments;
            }
        }
    }
}
