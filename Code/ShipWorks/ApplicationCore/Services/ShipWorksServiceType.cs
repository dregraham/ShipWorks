using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Services
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipWorksServiceType
    {
        [Description("ShipWorks Scheduler")]
        [ApiValue("scheduler")]
        Scheduler = 0
    }
}
