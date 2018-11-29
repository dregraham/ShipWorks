using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Unit Type
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum UnitType
    {
        [Description("Weight")]
        [ApiValue("Weight")]
        Weight,
        
        [Description("Length")]
        [ApiValue("Length")]
        Length
    }
}