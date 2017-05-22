using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Enum to identify the type of action queue gateway.  Mainly used for testing purposes.
    /// </summary>
    [Obfuscation(Exclude = false, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ActionQueueGatewayType
    {
        [Description("Standard")]
        Standard = 0,

        [Description("Error")]
        Error = 1,

        [Description("DefaultPrint")]
        DefaultPrint = 2
    }
}
