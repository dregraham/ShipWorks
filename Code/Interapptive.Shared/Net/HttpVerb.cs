using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Http Verbs/Methods
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum HttpVerb
    {
        [Description("GET")]
        Get,

        [Description("POST")]
        Post,

        [Description("PUT")]
        Put,

        [Description("PATCH")]
        Patch
    }
}
