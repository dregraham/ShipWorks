using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Casing to use for the Hex digits in query string
    /// </summary>
    public enum QueryStringEncodingCasing
    {
        /// <summary>
        /// .NET default (lower)
        /// </summary>
        Default = 0,

        /// <summary>
        /// Lower case (.NET default)
        /// </summary>
        Lower = 1,

        /// <summary>
        /// Upper case
        /// </summary>
        Upper = 1
    }
}
