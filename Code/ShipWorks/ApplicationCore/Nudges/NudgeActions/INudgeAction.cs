using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Nudges.NudgeActions
{
    /// <summary>
    /// A task to perform for a NudgeOption
    /// </summary>
    public interface INudgeAction
    {
        /// <summary>
        /// Executes an action that takes place as the result of a nudge option being selected.
        /// </summary>
        /// <param name="nudgeOption">The nudge option that triggered the action.</param>
        void Execute(NudgeOption nudgeOption);
    }
}
