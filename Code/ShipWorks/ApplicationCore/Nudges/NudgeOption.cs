using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Nudges.Buttons;

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
        public NudgeOption(int index, string text, Nudge owner, string action, string result)
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
        /// Gets the owning Nudge.
        /// </summary>
        public Nudge Owner { get; private set; }

        /// <summary>
        /// The task to perform for this NudgeOption
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Result to be returned to Tango
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Logs that this option was selected.
        /// </summary>
        public void Log()
        {
            ITangoWebClient webClient = new TangoWebClientFactory().CreateWebClient();
            webClient.LogNudgeOption(this);
        }

        public NudgeOptionButton CreateButton()
        {
            // Only one type of nudge option button at this time
            NudgeOptionButton button = new AcknowledgeNudgeOptionButton(this);
            
            //if (string.Compare(Action, "acknowledge", StringComparison.OrdinalIgnoreCase) == 0)
            //{
            //    button = new AcknowledgeNudgeOptionButton(this);
            //}
            
            //if (button == null)
            //{
            //    throw new NudgeException(string.Format("Unable to create a button for the {0} nudge option. The {1} action was not resolved.", Text, Action));
            //}

            button.Text = Text;
            
            return button;
        }
    }
}
