using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Enum that represents units of measure for weight
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum WeightUnitOfMeasure
    {
        [Description("Pounds")]
        Pounds = 0,

        [Description("Ounces")]
        Ounces = 1,

        [Description("Kilograms")]
        Kilograms = 2,

        [Description("Grams")]
        Grams = 3,

        [Description("Tonnes")]
        Tonnes = 4
    }
}
