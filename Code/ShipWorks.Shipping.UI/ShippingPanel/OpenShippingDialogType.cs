using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// How should the dialog be opened
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OpenShippingDialogType
    {
        /// <summary>
        /// Type isn't specified
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Open with all shipments
        /// </summary>
        [Description("AllShipments")]
        AllShipments,

        /// <summary>
        /// Open with the selected shipment
        /// </summary>
        [Description("SelectedShipment")]
        SelectedShipment,

        /// <summary>
        /// Open with selected orders
        /// </summary>
        [Description("SelectedOrders")]
        SelectedOrders,
    }
}
