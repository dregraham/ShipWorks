using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Enum for BigCommerce authentication types
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum BigCommerceAuthenticationType
    {
        // Basic auth
        [Description("Basic")]
        Basic = 0,

        // OAuth
        [Description("OAuth")]
        Oauth = 1
    }
}
