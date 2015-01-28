using System;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Nudges.Buttons
{
    /// <summary>
    /// Button for a nudge option that will shutdown ShipWorks when it is clicked.
    /// </summary>
    public class ShutdownNudgeOptionButton : NudgeOptionButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownNudgeOptionButton"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public ShutdownNudgeOptionButton(NudgeOption option)
            : base(option)
        { }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public override void HandleClick()
        {
            Environment.Exit(0);
        }
    }
}
