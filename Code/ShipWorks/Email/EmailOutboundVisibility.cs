using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email
{
    /// <summary>
    /// Visibility of an outbound email to the user in the email messages window
    /// </summary>
    public enum EmailOutboundVisibility
    {
        /// <summary>
        /// Completely visible to the user
        /// </summary>
        Visible = 0,

        /// <summary>
        /// Completely invisible to the user
        /// </summary>
        Internal = 1,

        /// <summary>
        /// Only displayed in the Outbox, not Sent Items
        /// </summary>
        OutboxOnly = 2
    }
}
