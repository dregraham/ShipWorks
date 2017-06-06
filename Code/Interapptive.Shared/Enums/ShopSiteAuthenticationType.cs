using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// This is the authentication type for shopsite stores
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
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
