using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExTermsOfSale
    {
        [Description("Free Carrier (FCA/FOB)")]
        FOB_or_FCA = 0,

        [Description("Carraige Insurance Paid (CIP/CIF)")]
        CIF_or_CIP = 1,

        [Description("Carriage Paid To (CPT/C&F)")]
        CFR_or_CPT = 2,

        [Description("Ex Works")]
        EXW = 3,

        [Description("Delivered Duty Unpaid")]
        DDU = 4,

        [Description("Delivered Duty Paid")]
        DDP = 5
    }
}
