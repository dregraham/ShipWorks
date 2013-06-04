using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// When a server message is in response to a previous message, this controls
    /// the action that is taken.
    /// </summary>
    enum ServerMessageResponseAction
    {
        /// <summary>
        /// The message is in response to the original message.  The message will
        /// only be shown to the user if the user saw the original message.
        /// </summary>
        FollowUp = 0,

        /// <summary>
        /// The message is sent only to dismiss the original message.  The original
        /// message should be dismissed, and the new message will not be shown.
        /// </summary>
        Dismiss = 1,
    }
}
