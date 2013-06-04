using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using System.Diagnostics;
using ShipWorks.Templates.Tokens;
using log4net;
using Rebex.Mail;
using System.Data.SqlTypes;
using ShipWorks.Email.Accounts;

namespace ShipWorks.Email
{
    /// <summary>
    /// Contains all the non-Body information of an Email message
    /// </summary>
    public class EmailMessageHeader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailMessageHeader));

        long accountID;
        string to;
        string cc = string.Empty;
        string bcc = string.Empty;
        string subject;
        EmailOutboundVisibility visibility = EmailOutboundVisibility.Visible;

        /// <summary>
        /// The email account to be used for sending
        /// </summary>
        public long EmailAccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// Recipient list
        /// </summary>
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// CC list
        /// </summary>
        public string CC
        {
            get { return cc; }
            set { cc = value; }
        }

        /// <summary>
        /// BCC list
        /// </summary>
        public string BCC
        {
            get { return bcc; }
            set { bcc = value; }
        }

        /// <summary>
        /// Subject line
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// Controls the visibility of the generated message
        /// </summary>
        public EmailOutboundVisibility Visibility
        {
            get { return visibility; }
            set { visibility = value; }
        }

        /// <summary>
        /// Create an outbound email entity object based on the properties of the header
        /// </summary>
        public virtual EmailOutboundEntity CreateEmailOutbound()
        {
            if (EmailAccountID == -1)
            {
                throw new EmailException("No email accounts have been configured.");
            }

            EmailAccountEntity account = EmailAccountManager.GetAccount(EmailAccountID);
            if (account == null)
            {
                log.ErrorFormat("EmailAccount {0} no longer exists.", EmailAccountID);
                throw new EmailException("The requested email account no longer exists.");
            }

            EmailOutboundEntity emailOutbound = new EmailOutboundEntity();

            emailOutbound.ComposedDate = DateTime.UtcNow;
            emailOutbound.SentDate = DateTime.UtcNow;
            emailOutbound.Visibility = (int) visibility;

            emailOutbound.SendStatus = (int) EmailOutboundStatus.Ready;
            emailOutbound.SendAttemptCount = 0;
            emailOutbound.SendAttemptLastError = "";

            emailOutbound.AccountID = EmailAccountID;
            emailOutbound.FromAddress = string.Format("\"{0}\" <{1}>", account.DisplayName, account.EmailAddress);

            emailOutbound.ToList = To;
            emailOutbound.CcList = CC;
            emailOutbound.BccList = BCC;
            emailOutbound.Subject = Subject;

            emailOutbound.Encoding = null;
            emailOutbound.PlainPartResourceID = -1;

            return emailOutbound;
        }
    }
}
