using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.ApplicationCore.Nudges.NudgeActions
{
    public class TangoLoggerNudgeAction : INudgeAction
    {
        /// <summary>
        /// Executes an action that takes place as the result of a nudge option being selected. This action simply logs the 
        /// option that was selected back to Tango.
        /// </summary>
        /// <param name="nudgeOption">The nudge option that triggered the action.</param>
        public void Execute(NudgeOption nudgeOption)
        {
            ITangoWebClient webClient = new TangoWebClientFactory().CreateWebClient();
            webClient.LogNudgeOption(nudgeOption);
        }
    }
}
