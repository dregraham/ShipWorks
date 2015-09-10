using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for settings needed by the Shipping Panel
    /// </summary>
    public class ShippingPanelConfigurator : IShippingPanelConfigurator
    {
        /// <summary>
        /// Wraps a call to determine if a user has permission on an entity
        /// </summary>
        public bool UserHasPermission(PermissionType permissionType, long orderID)
        {
            return UserSession.Security.HasPermission(permissionType, orderID);
        }

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public bool GetAddressValidation(ShipmentEntity shipment)
        {
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

        /// <summary>
        /// Get a list of available shipment types
        /// </summary>
        public IEnumerable<ShipmentType> AvailableShipmentTypes => ShipmentTypeManager.EnabledShipmentTypes;
    }
}
