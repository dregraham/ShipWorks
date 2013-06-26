using System.ComponentModel;
using System.Reflection;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipWorksServiceType
    {
        [Description("Scheduler")]
        Scheduler
    }
}
