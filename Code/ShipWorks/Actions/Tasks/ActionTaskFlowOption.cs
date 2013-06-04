using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Enumerates the ways a task can flow after it completes
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionTaskFlowOption
    {
        /// <summary>
        /// Move on to the next step, or quit if there is no next step.
        /// </summary>
        [Description("Go to the next step")]
        NextStep = 0,

        /// <summary>
        /// Quit processing completely
        /// </summary>
        [Description("Quit")]
        Quit = 1,

        /// <summary>
        /// Suspend processing when there is an error, but keep going after the error is resolved.
        /// </summary>
        [Description("Suspend")]
        Suspend = 2
    }
}
