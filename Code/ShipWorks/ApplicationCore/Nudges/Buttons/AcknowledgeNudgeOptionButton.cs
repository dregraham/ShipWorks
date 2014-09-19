using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Nudges.Buttons
{
    /// <summary>
    /// An implementation of the NudgeOptionButton that will just close the form the
    /// button resides within when it is clicked.
    /// </summary>
    public class AcknowledgeNudgeOptionButton : NudgeOptionButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgeNudgeOptionButton"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public AcknowledgeNudgeOptionButton(NudgeOption option)
            : base(option)
        { }

        /// <summary>
        /// Closes the form that this button resides within.
        /// </summary>
        public override void HandleClick()
        {
            Form form = FindForm();

            if (form != null)
            {
                form.Close();
            }
        }
    }
}
