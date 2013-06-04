using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum LabelFormatType
    {
        [Description("Standard")]
        Standard = 0,

        [Description("Thermal (Any)")]
        Thermal = 1,

        [Description("Eltron (EPL)")]
        EPL = 2,

        [Description("Zebra (ZPL)")]
        ZPL = 3
    }
}
