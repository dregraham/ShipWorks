using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Enum for license enforcement context
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EnforcementContext
    {
        [Description("No specific context")]
        NotSpecified,

        [Description("Logging into ShipWorks")]
        Login,

        [Description("Creating a label")]
        CreateLabel,

        [Description("Downloading orders")]
        Download, 

        [Description("Before adding the store")]
        OnAddingStore,

        /// <summary>
        /// This comes into play when Tango throws an error when adding a store
        /// because it will take the customer over the channel limit.
        /// </summary>
        [Description("Exceeding channel limit")]
        ExceedingChannelLimit
    }
}