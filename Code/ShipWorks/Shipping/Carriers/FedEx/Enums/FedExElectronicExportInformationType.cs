using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration of the Electronic Export Information types allowed by FedEx when shipping
    /// an internationally. This list was taken from the FedEx ShipManager product.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExElectronicExportInformationType
    {
        [Description("None")]
        None = 0,

        [Description("Automated Export System (AES)")]
        AES = 1,

        [Description("NO EEI 30.36")]
        EEI30306 = 2,

        [Description("NO EEI 30.37(a)")]
        EEI3037A = 3,

        [Description("NO EEI 30.37(e)")]
        EEI3037E = 4,

        [Description("NO EEI 30.37(f)")]
        EEI3037F = 5,

        [Description("NO EEI 30.37(g)")]
        EEI3037G = 6,

        [Description("NO EEI 30.37(h)")]
        EEI3037H = 7,

        [Description("NO EEI 30.37(i)")]
        EEI3037I = 8,

        [Description("NO EEI 30.37(j)")]
        EEI3037J = 9,

        [Description("NO EEI 30.37(k)")]
        EEI3037K = 10,

        [Description("NO EEI 30.37(o)")]
        EEI3037O = 11,

        [Description("NO EEI 30.37(q)")]
        EEI3037Q = 12,

        [Description("NO EEI 30.37(r)")]
        EEI3037R = 13,

        [Description("NO EEI 30.37(s)")]
        EEI3037S = 14,

        [Description("NO EEI 30.37(t)")]
        EEI3037T = 15,

        [Description("NO EEI 30.39")]
        EEI3039 = 16,

        [Description("NO EEI 30.40(a)")]
        EEI3040A = 17,

        [Description("NO EEI 30.40(b)")]
        EEI3040B = 18,

        [Description("NO EEI 30.40(c)")]
        EEI3040C = 19,

        [Description("NO EEI 30.40(d)")]
        EEI3040D = 20,

        [Description("NO EEI 30.2(d)(2)")]
        EEI302D2 = 21,
    }
}
