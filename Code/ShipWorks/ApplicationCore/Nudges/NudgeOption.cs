using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Nudges.NudgeActions;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Descriptions an action a user can perform on a nudge
    /// </summary>
    public class NudgeOption
    {
        private readonly int index;
        private readonly string text;
        private readonly INudgeAction action;

        /// <summary>
        /// Constructor
        /// </summary>
        public NudgeOption(int index, string text, INudgeAction action)
        {
            this.index = index;
            this.text = text;
            this.action = action;
        }

        /// <summary>
        /// Display order of this nudge option
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
        }

        /// <summary>
        /// Display text for this nudge option
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
        }

        /// <summary>
        /// The task to perform for this NudgeOption
        /// </summary>
        public INudgeAction Action
        {
            get
            {
                return action;
            }
        }
    }
}
