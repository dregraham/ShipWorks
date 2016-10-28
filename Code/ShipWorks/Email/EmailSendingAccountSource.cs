using System;
using System.Collections.Generic;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// A source for sending email messages for an entire account
    /// </summary>
    public class EmailSendingAccountSource : EmailSendingSource
    {
        EmailAccountEntity account;

        // This makes it so we can get consistant query results, due to the query against the SendAttemptLastTime.
        DateTime createdTime = DateTime.UtcNow;

        // How many messages we pull from the database at once
        static int emailPageSize = 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSendingAccountSource(EmailAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            this.account = account;
        }

        /// <summary>
        /// Get the email account used by all messages provided by the sending source.
        /// </summary>
        public override EmailAccountEntity EmailAccount
        {
            get { return account; }
        }

        /// <summary>
        /// Get the total count of messages to send.
        /// </summary>
        public override int GetCount()
        {
            return EmailOutboundCollection.GetCount(SqlAdapter.Default, CreateOutboxQueryPredicate());
        }

        /// <summary>
        /// Get the number of pending messages
        /// </summary>
        public override string ProgressPendingDescription
        {
            get
            {
                return "Pending send of all messages...";
            }
        }

        /// <summary>
        /// Get the next page of emails to send, limited by the max amount given.
        /// </summary>
        public override ICollection<long> GetNextPage()
        {
            // We need to try to pull the ids of the context type
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(EmailOutboundFields.EmailOutboundID, 0, "EmailOutboundID", "");

            using (SqlAdapter adapter = new SqlAdapter())
            {
                List<long> keys = new List<long>();

                using (IDataReader reader = adapter.FetchDataReader(
                    resultFields,
                    new RelationPredicateBucket(CreateOutboxQueryPredicate()),
                    CommandBehavior.CloseConnection,
                    emailPageSize,
                    new SortExpression(EmailOutboundFields.ComposedDate | SortOperator.Ascending),
                    false))
                {
                    while (reader.Read())
                    {
                        keys.Add(reader.GetInt64(0));
                    }

                    return keys;
                }
            }
        }

        /// <summary>
        /// Create the predicate used to query the outbox for messages to send
        /// </summary>
        private IPredicateExpression CreateOutboxQueryPredicate()
        {
            // We want all messages that are Ready to be sent.  Ignore failed emails; the customer needs to fix them before retrying.
            IPredicateExpression statusPredicate = new PredicateExpression(
                EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Ready |
                EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Retry);

            // We only want messages from this account
            IPredicateExpression predicate = new PredicateExpression(EmailOutboundFields.AccountID == account.EmailAccountID);
            predicate.AddWithAnd(statusPredicate);

            // Only the time has come to send the message
            predicate.AddWithAnd(EmailOutboundFields.DontSendBefore == DBNull.Value | EmailOutboundFields.DontSendBefore <= DateTime.UtcNow);

            // Only messages that we havnt JUST tried to send.  So like if a messages fails, and then we loop around again due to our paging,
            // we won't pick it right back up for sending.
            predicate.AddWithAnd(EmailOutboundFields.SentDate < createdTime);

            return predicate;
        }
    }
}
