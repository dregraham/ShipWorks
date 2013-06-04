using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EmailDisplayFormat
    {
        [Description("Name Only")]
        NameOnly = 0,

        [Description("Address Only")]
        AddressOnly = 1,

        [Description("Name and Address")]
        NameAndAddress = 2
    }
}
