using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Settings
{
    /// <summary>
    /// Shipworks UI Mode
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UIMode
    {
        [Description("Batch Mode")]
        Batch = 0,
        
        [Description("Order Lookup Mode")]
        OrderLookup = 1
    }
}
