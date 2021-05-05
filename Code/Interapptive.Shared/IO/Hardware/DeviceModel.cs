using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.IO.Hardware
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DeviceModel
    {
        [SortOrder(100)]
        [Description("100L")]
        Model100L = 0,
        
        [SortOrder(110)]
        [Description("110")]
        Model110 = 1,
        
        [SortOrder(125)]
        [Description("125")]
        Model125 = 2,
        
        [SortOrder(150)]
        [Description("150")]
        Model150 = 3,
        
        [SortOrder(225)]
        [Description("225")]
        Model225 = 4,
        
        [SortOrder(75)]
        [Description("75")]
        Model75 = 5
    }
}