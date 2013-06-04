using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    /// <summary>
    /// Used for the authorization condition
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ProStoresAuthorizationStatus
    {
        [Description("Is authorized")]
        Authorized = 0,

        [Description("Was authorized on")]
        AuthorizedOn = 1,

        [Description("Is not authorized")]
        NotAuthorized = 2
    }
}
