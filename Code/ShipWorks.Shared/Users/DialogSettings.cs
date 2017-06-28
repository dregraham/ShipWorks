using System.Linq;
using System.Reflection;
using Interapptive.Shared.UI;

namespace ShipWorks.Shared.Users
{
    /// <summary>
    /// User specific dialog settings
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class DialogSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DialogSettings()
        {
            DismissedNotifications = Enumerable.Empty<UserConditionalNotificationType>().ToArray();
        }

        /// <summary>
        /// List of notifications that have been dismissed
        /// </summary>
        public UserConditionalNotificationType[] DismissedNotifications { get; set; }
    }
}
