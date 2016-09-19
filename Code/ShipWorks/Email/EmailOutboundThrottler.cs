using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using Rebex.Net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// Throttles outgoing messages and connections based on account settings
    /// </summary>
    public sealed class EmailOutboundThrottler : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailOutboundThrottler));

        // Source for messages
        EmailSendingSource messageSource;
        EmailAccountEntity account;

        int emailsTotal = 0;
        int emailsRetrieved = 0;

        Smtp smtpConnection = null;
        int smtpUsageCount = 0;

        // This is based on our per-time-period throttling
        int maxEmailsAllowed = int.MaxValue;

        // Current page of queried messages to send
        ICollection<long> pendingEmail = new List<long>();

        /// <summary>
        /// Create a new instance that will send messages using the given source
        /// </summary>
        public EmailOutboundThrottler(EmailSendingSource messageSource)
        {
            if (messageSource == null)
            {
                throw new ArgumentNullException("messageSource");
            }

            this.messageSource = messageSource;
            this.account = messageSource.EmailAccount;

            this.emailsTotal = messageSource.GetCount();

            // See if we have a maximum number for a given timeframe
            if (account.LimitMessagesPerHour)
            {
                // See how many messages have been sent for this account in the last hour
                int hourCount = EmailOutboundCollection.GetCount(SqlAdapter.Default,
                    EmailOutboundFields.AccountID == account.EmailAccountID &
                    EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Sent &
                    EmailOutboundFields.SentDate >= DateTime.UtcNow.AddHours(-1));

                maxEmailsAllowed = Math.Max(0, account.LimitMessagesPerHourQuantity - hourCount);

                log.InfoFormat("Limiting messages to be sent to {0} due to delivery configuration.", maxEmailsAllowed);
            }
        }

        /// <summary>
        /// The account the throttler uses to send
        /// </summary>
        public EmailAccountEntity EmailAccount
        {
            get { return account; }
        }

        /// <summary>
        /// The number of messages that have been retrieved for sending
        /// </summary>
        public int MessagesRetrieved
        {
            get { return emailsRetrieved; }
        }

        /// <summary>
        /// The total number of messages the throttler is going to provide
        /// </summary>
        public int MessagesTotal
        {
            get { return emailsTotal; }
        }

        /// <summary>
        /// Get the description detail that should be displayed in progress windows when this throttler is still pending starting
        /// </summary>
        public string ProgressPendingDescription
        {
            get { return messageSource.ProgressPendingDescription; }
        }

        /// <summary>
        /// Retrieve the next message that is ready to be sent.  Will be null when there are no more to send.
        /// </summary>
        public long? RetrieveNextMessageToSend()
        {
            // If we don't have any more left to pick off our last page then requery
            if (pendingEmail.Count == 0)
            {
                QueryNextOutboundPage();
            }

            // If the query didn't return anything, we are done
            if (pendingEmail.Count == 0)
            {
                return null;
            }

            // Enforce the pause between messages if configured to do so
            if (account.LimitMessageInterval && emailsRetrieved > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(account.LimitMessageIntervalSeconds));
            }

            // Pop the next one
            long nextOutboundID = pendingEmail.First();
            pendingEmail.Remove(nextOutboundID);

            // This counts against the quantity we have retrieved
            emailsRetrieved++;

            // Return this one
            return nextOutboundID;
        }

        /// <summary>
        /// Get the next page of outbound messages to send
        /// </summary>
        private void QueryNextOutboundPage()
        {
            // Shouldn't be here if we already have some cached from the last page
            if (pendingEmail.Count > 0)
            {
                throw new InvalidOperationException("Should not be querying the next page while some still exist.");
            }

            // Get the next page of email's to send
            pendingEmail = messageSource.GetNextPage();

            // Update the total emails left to send
            emailsTotal = Math.Max(emailsTotal, emailsRetrieved + pendingEmail.Count);
        }

        /// <summary>
        /// Gets an open SMTP connecto to use for sending messages
        /// </summary>
        public Smtp GetSmtpConnection()
        {
            // If we have already sent the max amount, we can't give out the connection anymore.
            if (emailsRetrieved > maxEmailsAllowed)
            {
                throw new EmailThrottleException(string.Format("The maximum of {0} emails per hour has already been sent for this account.", account.LimitMessagesPerHourQuantity));
            }

            // See if we've sent too many messages using the current connection
            if (account.LimitMessagesPerConnection && smtpUsageCount >= account.LimitMessagesPerConnectionQuantity)
            {
                if (smtpConnection != null)
                {
                    log.InfoFormat("Disconnecting from SMTP due to max messages sent.");

                    CloseSmtpConnection();
                }
            }

            // See if we need to open a new connection
            if (smtpConnection == null)
            {
                log.InfoFormat("Loggging into SMTP for account {0}.", account.AccountName);

                smtpConnection = EmailUtility.LogonToSmtp(account);
                smtpUsageCount = 0;
            }
            else
            {
                var state = smtpConnection.GetConnectionState();

                if (!state.Connection)
                {
                    log.InfoFormat("Existing SMTP for account {0}: {1} found to be disconnected with code {2}.", account.AccountName, smtpConnection.State, state.NativeErrorCode);

                    throw new SmtpException("The mail server has closed the connection.", SmtpExceptionStatus.ConnectionClosed);
                }

                log.InfoFormat("Using existing SMTP for account {0}: {1}", account.AccountName, smtpConnection.State);
            }

            // Each query for the connection counts against the usage count
            smtpUsageCount++;

            return smtpConnection;
        }

        /// <summary>
        /// Close the currently logged on and open SMTP connection
        /// </summary>
        private void CloseSmtpConnection()
        {
            smtpConnection.Disconnect();
            smtpConnection.Dispose();
            smtpConnection = null;
        }

        /// <summary>
        /// Dispose any connections left open by the throttler.
        /// </summary>
        public void Dispose()
        {
            if (smtpConnection != null)
            {
                CloseSmtpConnection();
            }
        }
    }
}
