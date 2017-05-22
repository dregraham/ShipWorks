using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.BigCommerce.Enums
{
    // Enum to search by order created date or modified date
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BigCommerceWebClientOrderDateSearchType
    {
        [Description("Created Date")]
        CreatedDate = 0,

        [Description("Modified Date")]
        ModifiedDate = 1
    }
}
