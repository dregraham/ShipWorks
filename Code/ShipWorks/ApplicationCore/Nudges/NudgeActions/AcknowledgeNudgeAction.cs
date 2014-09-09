﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Nudges.NudgeActions
{
    /// <summary>
    /// The Acknowledge action for nudge options
    /// </summary>
    public class AcknowledgeNudgeAction : INudgeAction
    {
        // This is what we will log to Tango
        private readonly string result;
        private readonly int nudgeID;

        /// <summary>
        /// Constructor
        /// </summary>
        public AcknowledgeNudgeAction(int nudgeID, string result)
        {
            this.result = result;
            this.nudgeID = nudgeID;
        }

        /// <summary>
        /// The task to perform
        /// </summary>
        public void Execute(Form owner)
        {
            owner.Close();
        }
    }
}
