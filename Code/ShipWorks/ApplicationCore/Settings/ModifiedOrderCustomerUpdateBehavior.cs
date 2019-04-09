using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Method for logging in
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ModifiedOrderCustomerUpdateBehavior
    {
        [Description("Never update the customer")]
        NeverCopy = 0,

        [Description("Only update the customer if blank or matching")]
        CopyIfBlankOrMatching = 1,

        [Description("Always update the customer")]
        AlwaysCopy = 2
    }
}
