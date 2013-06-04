using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// The supported encodings for module responses.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum GenericStoreResponseEncoding
    {
        [Description("UTF-8")]
        UTF8 = 0,

        [Description("Latin-1/ISO-8859-1")]
        Latin1 = 1,
    }
}
