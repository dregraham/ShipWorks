using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Result of a shipment update operation
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    internal enum ShipmentUpdateOperationResult
    {
        [Description("Loaded")]
        Loaded,

        [Description("Deleted")]
        Deleted
    }
}