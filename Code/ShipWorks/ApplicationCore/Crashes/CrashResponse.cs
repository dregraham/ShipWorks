using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Represents a crash response
    /// </summary>
    public class CrashResponse
    {
        const string defaultMessageForUser = "Thank you for helping us to improve ShipWorks.";

        bool stopReporting;

        /// <summary>
        /// Cannot crate directly
        /// </summary>
        private CrashResponse()
        {

        }

        /// <summary>
        /// Create the given crash response
        /// </summary>
        public static CrashResponse Read(string result)
        {
            XDocument document = XDocument.Parse(result);
            XElement root = document.Root;

            if (root.Element("error") != null)
            {
                throw new CrashSubmitException((string) root.Element("error"));
            }

            XElement caseElement = root.Element("case");
            CrashResponse response = new CrashResponse();

            response.FogBugzCase = (int) caseElement.Element("ixBug");
            response.ScoutMessage = (string) caseElement.Element("sScoutMessage");
            response.Occurrences = (int) caseElement.Element("c");
            response.stopReporting = (bool) caseElement.Element("fScoutStopReporting");

            return response;
        }

        /// <summary>
        /// Show the response window to the user
        /// </summary>
        public void ShowDialog(IWin32Window owner)
        {
            string message;

            if (string.IsNullOrEmpty(ScoutMessage))
            {
                message = "Thank you for helping us to improve ShipWorks.";
            }
            else
            {
                Match match = Regex.Match(ScoutMessage, "FixedFor:(?<version>.*)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    message = string.Format("This crash has been fixed for ShipWorks {0}.", match.Groups["version"].Value);
                }
                else
                {
                    message = ScoutMessage.Replace("\\n", "\n");
                }
            }

            if (stopReporting)
            {
                message += "\n\nNote: We are no longer monitoring reports of this crash.  If you need to contact us please do so directly.";
            }

            message += string.Format("\n\n[This crash has been logged as support case {0}{1}]",
                FogBugzCase,
                (Occurrences == 0) ? "" : string.Format("-{0}", Occurrences + 1));

            MessageHelper.ShowInformation(owner, message);
        }

        /// <summary>
        /// The case number in FogBugz
        /// </summary>
        public int FogBugzCase { get; private set; }

        /// <summary>
        /// The number of times the unique identifier for this case has been submitted
        /// </summary>
        public int Occurrences { get; private set; }

        /// <summary>
        /// The message specified by FogBugz to be displayed to the user.  Can be null if no message was indicated.
        /// </summary>
        public string ScoutMessage { get; private set; }
    }
}
