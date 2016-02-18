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

        [Description("From StoreManagerDlg")]
        StoreManagerDlg,

        [Description("From AddStoreWizard")]
        AddStoreWizard,
    }
}