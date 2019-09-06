using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// The type of order splitter: Local or Hub
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum OrderSplitterType
    {
        [Description("Local")]
        Local = 0,

        [Description("Hub")]
        Hub = 1
    }
}
