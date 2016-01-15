using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Interface for settings needed by the Shipping activities
    /// </summary>
    public class ShippingConfiguration : IShippingConfiguration
    {
        private readonly IShippingSettings shippingSettings;
        private readonly ISecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingConfiguration(ISecurityContext securityContext, IShippingSettings shippingSettings)
        {
            this.securityContext = securityContext;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Gets whether a new shipment should be auto-created for an order
        /// </summary>
        public bool ShouldAutoCreateShipment(OrderEntity order) =>
            !order.Shipments.Any() &&
                shippingSettings.AutoCreateShipments &&
                securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID);
    }
}
