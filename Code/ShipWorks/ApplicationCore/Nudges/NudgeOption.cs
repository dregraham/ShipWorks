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
        public NudgeOption(int nudgeOptionID, int index, string text, Nudge owner, NudgeOptionActionType action)
        {
            NudgeOptionID = nudgeOptionID;
            Index = index;
            Text = text;
            Action = action;
            Owner = owner;
            Result = string.Empty;
        }

        /// <summary>
        /// Gets the nudge option ID.
        /// </summary>
        public int NudgeOptionID { get; private set; }

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
        /// The action to perform when this nudge option is selected.
        /// </summary>
        public NudgeOptionActionType Action { get; private set; }

        /// <summary>
        /// Result to be returned to Tango. This can be customized to contain a value that is specific to the 
        /// context that the option was selected within (e.g. a shipping account was created, declined to create a 
        /// shipping account, etc.).
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Logs that this option was selected.
        /// </summary>
        public void Log()
        {
            ITangoWebClient webClient = new TangoWebClientFactory().CreateWebClient();
            webClient.LogNudgeOption(this);
        }

        /// <summary>
        /// Factory method for creating a button based on the action of the nudge option
        /// </summary>
        /// <returns>An instance of NudgeOptionButton.</returns>
        /// <exception cref="NudgeException">Thrown when the action type is not recognized.</exception>
        public NudgeOptionButton CreateButton()
        {
            NudgeOptionButton button;

            switch (Action)
            {
                case NudgeOptionActionType.None:
                {
                    button = new AcknowledgeNudgeOptionButton(this);
                    break;
                }

                case NudgeOptionActionType.Shutdown:
                {
                    button = new ShutdownNudgeOptionButton(this);
                    break;
                }
                
                case NudgeOptionActionType.RegisterStampsAccount:
                {
                    button = new RegisterUspsAccountNudgeOptionButton(this);
                    break;
                }

                default:
                {
                    throw new NudgeException(string.Format("Unable to create a button for the {0} nudge option. The {1} action was not resolved.", Text, Action));
                }
            }

            button.Text = Text;
            return button;
        }
    }
}
