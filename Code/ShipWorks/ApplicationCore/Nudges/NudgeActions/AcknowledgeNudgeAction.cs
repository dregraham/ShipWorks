using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Nudges.NudgeActions
{
    /// <summary>
    /// The Acknowledge action for nudge options
    /// </summary>
    public class AcknowledgeNudgeAction : INudgeAction
    {
        public string Execute()
        {
            return "OKClicked";
        }
    }
}
