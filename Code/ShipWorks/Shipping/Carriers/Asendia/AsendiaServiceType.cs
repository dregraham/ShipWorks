using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Available Asendia service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AsendiaServiceType
    {
        [Description("Asendia Priority Tracked")]
        [ApiValue("asendia_priority_tracked")]
        AsendiaPriorityTracked = 0,

        [Description("Asendia International Express")]
        [ApiValue("asendia_international_express")]
        AsendiaInternationalExpress = 1,

        [Description("Asendia IPA")]
        [ApiValue("asendia_ipa")]
        AsendiaIPA = 2,

        [Description("Asendia ISAL")]
        [ApiValue("asendia_isal")]
        AsendiaISAL = 3,

        [Description("Asendia PMI")]
        [ApiValue("asendia_pmi")]
        AsendiaPMI = 4,

        [Description("Asendia PMEI")]
        [ApiValue("asendia_pmei")]
        AsendiaPMEI = 5,

        [Description("Asendia ePacket")]
        [ApiValue("asendia_epacket")]
        AsendiaEPacket = 6,

        [Description("Asendia Other")]
        [ApiValue("asendia_other")]
        AsendiaOther = 7
    }
}
