using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.EquaShip.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EquaShipPackageType
    {
        [Description("Box")]
        Box = 0,

        [Description("Tube")]
        Tube = 1,

        [Description("Customer Supplied")]
        CustomerSupplied = 2,

        [Description("Letter/Envelope")]
        LetterEnvelope = 3,

        [Description("Nonstandard Container")]
        NonstandardContainer = 4
    }
}
