using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Enum with values pertaining to the result of a shipping panel shipment load operation.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShippingPanelLoadedShipmentResult
    {
        [Description("Success")]
        Success = 0,

        [Description("Not Created")]
        NotCreated = 1,

        [Description("Multiple")]
        Multiple = 2,

        [Description("Error")]
        Error = 3,

        [Description("Unsupported Shipment Type")]
        UnsupportedShipmentType = 4,

        [Description("Deleted")]
        Deleted = 5
    }
}
