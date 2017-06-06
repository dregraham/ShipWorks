using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Source of a wizard action
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OpenedFromSource
    {
        /// <summary>
        /// The opener has not been specified
        /// </summary>
        NotSpecified,

        /// <summary>
        /// Initial ShipWorks setup
        /// </summary>
        InitialSetup,

        /// <summary>
        /// Quick Start dialog
        /// </summary>
        QuickStart,

        /// <summary>
        /// Manager dialog for the relevant wizard
        /// </summary>
        Manager,

        /// <summary>
        /// Processing a label
        /// </summary>
        Processing,

        /// <summary>
        /// Nudge
        /// </summary>
        Nudge
    }
}
