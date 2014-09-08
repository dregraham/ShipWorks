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
        string Execute();
    }
}
