using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Determines whether a note is visible to the public or for internal use only
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NoteVisibility
    {
        [Description("Internal")]
        Internal = 0,

        [Description("Public")]
        Public = 1
    }
}
