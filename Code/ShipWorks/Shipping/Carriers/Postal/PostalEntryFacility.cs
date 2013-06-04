using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Entry Facility codes for postal service
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalEntryFacility
    {
        [Description("Other")]
        Other = 0,

        [Description("Destination BMC")]
        DBMC = 1,

        [Description("Destination Delivery Unit")]
        DDU = 2,

        [Description("Destionation SCF")]
        DSCF = 3,

        [Description("Origin BMC")]
        OBMC = 4
    }
}
