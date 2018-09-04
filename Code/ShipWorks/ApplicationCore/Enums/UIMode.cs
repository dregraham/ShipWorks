using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Enums
{
    /// <summary>
    /// Shipworks UI Mode
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UIMode
    {
        [Description("Batch Mode")]
        Batch = 0,
        
        [Description("Order Lookup Mode")]
        OrderLookup = 1
    }
}
