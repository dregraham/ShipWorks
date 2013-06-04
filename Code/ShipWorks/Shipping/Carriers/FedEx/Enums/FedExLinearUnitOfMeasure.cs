using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration for the unit of measure to use for dimensions
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExLinearUnitOfMeasure
    {
        [Description("Inches")] 
        IN = 0,

        [Description("Centimeters")] 
        CM = 1
    }
}
