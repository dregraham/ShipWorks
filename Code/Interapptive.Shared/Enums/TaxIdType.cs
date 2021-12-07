using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Unit Type
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum TaxIdType
    {
        [Description("VAT")]
        Vat = 0,

        [Description("EORI")]
        Eori = 1,

        [Description("SSN")]
        Ssn = 2,

        [Description("EIN")]
        Ein = 3,

        [Description("TIN")]
        Tin = 4,

        [Description("IOSS")]
        Ioss = 5,

        [Description("PAN")]
        Pan = 6,

        [Description("VOEC")]
        Voec = 7
    }
}