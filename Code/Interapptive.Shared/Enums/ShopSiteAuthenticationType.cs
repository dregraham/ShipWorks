using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// This is the authentication type for shopsite stores
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum ShopSiteAuthenticationType
    {
        // Basic auth
        [Description("Basic")]
        Basic = 0,

        // OAuth
        [Description("OAuth")]
        Oauth = 1
    }
}
