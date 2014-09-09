using ShipWorks.ApplicationCore.Nudges.NudgeActions;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Descriptions an action a user can perform on a nudge
    /// </summary>
    public class NudgeOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeOption"/> class.
        /// </summary>
        public NudgeOption(int index, string text, Nudge owner, INudgeAction action, string result)
        {
            Index = index;
            Text = text;
            Action = action;
            Result = result;
            Owner = owner;
        }

        /// <summary>
        /// Display order of this nudge option
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Display text for this nudge option
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        public Nudge Owner { get; private set; }

        /// <summary>
        /// The task to perform for this NudgeOption
        /// </summary>
        public INudgeAction Action { get; private set; }

        /// <summary>
        /// Result to be returned to Tango
        /// </summary>
        public string Result { get; private set; }
    }
}
