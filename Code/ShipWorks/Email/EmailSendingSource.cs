using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// Base class for sources of emails for sending
    /// </summary>
    public abstract class EmailSendingSource
    {
        /// <summary>
        /// The EmailAccount that all messages from the source are from.
        /// </summary>
        public abstract EmailAccountEntity EmailAccount { get; }

        /// <summary>
        /// Get the next page of emails to sent.
        /// </summary>
        public abstract ICollection<long> GetNextPage();

        /// <summary>
        /// Get the total count of emails to send.  This is allowed to change over time.
        /// </summary>
        public abstract int GetCount();

        /// <summary>
        /// Get the status message to use for progress display when its waiting to send.
        /// </summary>
        public abstract string ProgressPendingDescription { get; }
    }
}
