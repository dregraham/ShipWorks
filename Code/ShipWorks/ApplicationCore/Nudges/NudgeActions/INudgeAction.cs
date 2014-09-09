using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Nudges.NudgeActions
{
    /// <summary>
    /// A task to perform for a NudgeOption
    /// </summary>
    public interface INudgeAction
    {
        void Execute(Form owner);
    }
}
