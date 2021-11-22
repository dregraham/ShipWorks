using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Unit Type
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum CustomsRecipientEntityType
    {
        [Description("Shipper")]
        [ApiValue("shipper")]
        Shipper,
        
        [Description("Recipient")]
        [ApiValue("recipient")]
        Recipient
    }
}