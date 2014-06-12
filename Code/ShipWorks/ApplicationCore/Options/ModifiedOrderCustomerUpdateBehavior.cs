using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Method for logging in
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ModifiedOrderCustomerUpdateBehavior
    {
        [Description("Never copy")]
        NeverCopy = 0,

        [Description("Copy if blank")]
        CopyIfBlank = 1,

        [Description("Always copy")]
        AlwaysCopy = 2
    }
}
