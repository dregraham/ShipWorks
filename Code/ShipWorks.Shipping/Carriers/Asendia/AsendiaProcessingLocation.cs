using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum AsendiaProcessingLocation
    {
        [Description("BUF")]
        BUF,
        
        [Description("CAL")]
        CAL,

        [Description("JFK")]
        JFK,

        [Description("LAX")]
        LAX,

        [Description("MIA")]
        MIA,

        [Description("ORD")]
        ORD,

        [Description("PHL")]
        PHL,

        [Description("SFO")]
        SFO,

        [Description("SLC")]
        SLC,

        [Description("TOR")]
        TOR
    }
}
