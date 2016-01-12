using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Configuration
{
    /// <summary>
    /// Interface for settings needed by the Shipping activities
    /// </summary>
    public interface IShippingConfiguration
    {
        /// <summary>
        /// Gets whether a new shipment should be auto-created for an order
        /// </summary>
        bool ShouldAutoCreateShipment(OrderEntity order);

        ///// <summary>
        ///// Wraps a call to determine if a user has permission on an entity
        ///// </summary>
        //bool UserHasPermission(PermissionType permissionType, long entityID);

        ///// <summary>
        ///// Determines address validation for a shipment
        ///// </summary>
        //bool GetAddressValidation(ShipmentEntity shipment);

        ///// <summary>
        ///// Wraps ShippingSettings.AutoCreateShipments
        ///// </summary>
        //bool AutoCreateShipments { get; }
    }
}
