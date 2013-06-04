using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// Source for sending emails that come from a list of messages
    /// </summary>
    public class EmailSendingMessageSource : EmailSendingSource
    {
        EmailAccountEntity account;
        List<long> messages;

        int initialCount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSendingMessageSource(EmailAccountEntity account, IEnumerable<long> messages)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            this.account = account;
            this.messages = messages.ToList();

            this.initialCount = this.messages.Count;
        }

        /// <summary>
        /// The EmailAccount that all messages from the source are from.
        /// </summary>
        public override EmailAccountEntity EmailAccount
        {
            get { return account; }
        }

        /// <summary>
        /// Get the next page of emails to sent.  Return up to max results at a time.
        /// </summary>
        public override ICollection<long> GetNextPage()
        {
            List<long> page = messages;
            messages = new List<long>();

            return page;
        }

        /// <summary>
        /// Get the total count of emails to send.
        /// </summary>
        public override int GetCount()
        {
            return initialCount;
        }

        /// <summary>
        /// Get the status message to use for progress display when its waiting to send.
        /// </summary>
        public override string ProgressPendingDescription
        {
            get
            {
                return string.Format("Pending send of {0} messages...", initialCount);
            }
        }
    }
}
