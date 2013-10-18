using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute for decorating enumeration values that should not be made visible to users
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class HiddenAttribute : Attribute
    {

    }
}
