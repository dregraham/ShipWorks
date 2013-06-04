using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// The priority of a print job
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PrintJobPriority
    {
        /// <summary>
        /// Normal priority jobs are those directly initiate by the user that the user is waiting on.
        /// </summary>
        Normal = 5,

        /// <summary>
        /// Low priority jobs are those initiated by the action processer that the user is not directly waiting on.
        /// </summary>
        Low = 1
    }
}
