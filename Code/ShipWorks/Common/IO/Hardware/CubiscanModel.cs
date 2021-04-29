using System.ComponentModel;
using System.Reflection;


namespace ShipWorks.Common.IO.Hardware
{
    [Obfuscation]
    public enum CubiscanModel
    {
        [Description("100L")]
        Model100L = 0,
        
        [Description("110")]
        Model110 = 1,
        
        [Description("125")]
        Model125 = 2,
        
        [Description("150")]
        Model150 = 3,
        
        [Description("225")]
        Model225 = 4
    }
}