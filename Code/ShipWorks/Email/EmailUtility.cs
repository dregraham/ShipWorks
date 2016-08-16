using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Rebex.Net;
using System.Windows.Forms;
using System.Collections;
using ShipWorks.Email.Accounts;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;
using Interapptive.Shared.UI;
using Rebex.Mime.Headers;
using Rebex.Mime;
using log4net;
using System.Data.SqlClient;
using Interapptive.Shared.Security;

namespace ShipWorks.Email
{
    /// <summary>
    /// Helper functions for working with email
    /// </summary>
    public static class EmailUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EmailUtility));

        /// <summary>
        /// Indicates if the given email address is valid
        /// </summary>
        public static bool IsValidEmailAddress(string email)
        {
            if (email == null)
            {
                return false;
            }

            // Very simple
            return email.Contains("@") && email.Contains(".");
        }

        /// <summary>
        /// Split the list of zero or more email addresses into a list of individual addresses
        /// </summary>
        public static List<string> SplitAddressList(string addressList)
        {
            if (string.IsNullOrEmpty(addressList))
            {
                return new List<string>();
            }

            string[] split = addressList.Split(',', ';');

            return split.Where(a => a.Trim().Length > 0).ToList();
        }

        /// <summary>
        /// Validate the addresses to which an email is being sent
        /// </summary>
        public static void ValidateAddresses(string to, string cc, string bcc)
        {
            // Has to be someone to send to
            if (to.Trim().Length == 0)
            {
                throw new EmailException("A 'To' email address was not specified.", EmailExceptionErrorNumber.MissingToField);
            }

            // Validate the email addresses
            foreach (string recipient in
                SplitAddressList(to).Concat(
                SplitAddressList(cc).Concat(
                SplitAddressList(bcc))))
            {
                try
                {
                    MailAddress address = new MailAddress(recipient);

                    if (!EmailUtility.IsValidEmailAddress(address.Address))
                    {
                        throw new EmailException(string.Format("The email address '{0}' is not valid.", recipient), EmailExceptionErrorNumber.InvalidEmailAddress);
                    }
                }
                catch (MimeException ex)
                {
                    log.Error(string.Format("Unable to part '{0}' into MailAddress", recipient), ex);

                    throw new EmailException(string.Format("The email address '{0}' is not valid.", recipient), ex, EmailExceptionErrorNumber.InvalidEmailAddress);
                }
            }
        }

        /// <summary>
        /// Create a new email account with defaults
        /// </summary>
        public static EmailAccountEntity CreateNewAccount(EmailIncomingServerType serverType)
        {
            EmailAccountEntity emailAccount = new EmailAccountEntity();

            emailAccount.DisplayName = "";
            emailAccount.EmailAddress = "";

            emailAccount.IncomingServer = "";
            emailAccount.IncomingServerType = (int) serverType;
            emailAccount.IncomingPort = 110;
            emailAccount.IncomingSecurityType = (int) EmailIncomingSecurityType.Unsecure;
            emailAccount.IncomingUsername = "";
            emailAccount.IncomingPassword = "";

            emailAccount.OutgoingServer = "";
            emailAccount.OutgoingPort = 25;
            emailAccount.OutgoingSecurityType = (int) SmtpSecurity.Unsecure;
            emailAccount.OutgoingCredentialSource = (int) EmailSmtpCredentialSource.None;
            emailAccount.OutgoingUsername = "";
            emailAccount.OutgoingPassword = "";

            emailAccount.AutoSend = true;
            emailAccount.AutoSendMinutes = 15;
            emailAccount.AutoSendLastTime = DateTime.UtcNow;

            emailAccount.LimitMessagesPerConnection = true;
            emailAccount.LimitMessagesPerConnectionQuantity = 50;

            emailAccount.LimitMessagesPerHour = false;
            emailAccount.LimitMessagesPerHourQuantity = 100;

            emailAccount.LimitMessageInterval = false;
            emailAccount.LimitMessageIntervalSeconds = 5;

            emailAccount.InternalOwnerID = null;

            return emailAccount;
        }

        /// <summary>
        /// Connect and logon to an Smtp account using the specified account settings
        /// </summary>
        public static Smtp LogonToSmtp(EmailAccountEntity account)
        {
            TlsParameters tls = new TlsParameters();
            tls.CertificateVerifier = CertificateVerifier.AcceptAll;

            Smtp smtp = new Smtp();

            try
            {
                smtp.Connect(account.OutgoingServer, account.OutgoingPort, tls, (SmtpSecurity)account.OutgoingSecurityType);
            }
            catch (UriFormatException ex)
            {
                log.Error(string.Format("Cannot parse hostname {0}", account.OutgoingServer), ex);
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (SmtpException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (TlsException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }

            EmailSmtpCredentialSource credentialSource = (EmailSmtpCredentialSource) account.OutgoingCredentialSource;

            // See if we have to logon in some way
            if (credentialSource != EmailSmtpCredentialSource.None)
            {
                // Have to login and out of pop
                if (credentialSource == EmailSmtpCredentialSource.PopBeforeSmtp)
                {
                    using (Pop3 pop3 = LogonToPop3(account))
                    {
                        pop3.Disconnect();
                    }
                }
                else
                {
                    string username;
                    string password;

                    // Use user pass from incoming
                    if (account.OutgoingCredentialSource == (int) EmailSmtpCredentialSource.SameAsIncoming)
                    {
                        username = account.IncomingUsername;
                        password = SecureText.Decrypt(account.IncomingPassword, account.IncomingUsername);
                    }
                    // Use specified user pass from outgoing
                    else
                    {
                        username = account.OutgoingUsername;
                        password = SecureText.Decrypt(account.OutgoingPassword, account.OutgoingUsername);
                    }

                    if (username.Length == 0 || password.Length == 0)
                    {
                        throw new EmailLogonException("A username and password must be specified for the outgoing mail server.");
                    }

                    try
                    {
                        log.InfoFormat("Logging into mail server using credential source {0} with username {1}", account.OutgoingCredentialSource, username);

                        // Logon to the SMTP server
                        smtp.Login(username, password);
                    }
                    catch (SmtpException ex)
                    {
                        throw new EmailLogonException(ex.Message, ex);
                    }
                }
            }

            return smtp;
        }

        /// <summary>
        /// Connect and logon to a pop account using the specified account settings
        /// </summary>
        public static Pop3 LogonToPop3(EmailAccountEntity account)
        {
            if (account.IncomingServerType != (int) EmailIncomingServerType.Pop3)
            {
                throw new EmailLogonException("Cannot login as POP3 when using an incoming IMAP server.");
            }

            TlsParameters tls = new TlsParameters();
            tls.CertificateVerifier = CertificateVerifier.AcceptAll;

            Pop3 pop3 = new Pop3();

            try
            {
                pop3.Connect(account.IncomingServer, account.IncomingPort, tls, (Pop3Security) account.IncomingSecurityType);

                string username = account.IncomingUsername;
                string password = SecureText.Decrypt(account.IncomingPassword, account.IncomingUsername);

                if (username.Length == 0 || password.Length == 0)
                {
                    throw new EmailLogonException("A username and password must be specified for the incoming mail server.");
                }

                pop3.Login(username, password);
            }
            catch (ArgumentException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (TlsException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (Pop3Exception ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }

            return pop3;
        }

        /// <summary>
        /// Connect and logon to an imap account with the given account settings
        /// </summary>
        public static Imap LogonToImap(EmailAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (account.IncomingServerType != (int) EmailIncomingServerType.Imap)
            {
                throw new EmailLogonException("Cannot login as IMAP when using an incoming POP3 server.");
            }

            TlsParameters tls = new TlsParameters();
            tls.CertificateVerifier = CertificateVerifier.AcceptAll;

            Imap imap = new Imap();

            try
            {
                imap.Connect(account.IncomingServer, account.IncomingPort, tls, (ImapSecurity) account.IncomingSecurityType);

                string username = account.IncomingUsername;
                string password = SecureText.Decrypt(account.IncomingPassword, account.IncomingUsername);

                if (username.Length == 0 || password.Length == 0)
                {
                    throw new EmailLogonException("A username and password must be specified for the incoming mail server.");
                }

                imap.Login(username, password);
            }
            catch (ArgumentException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (TlsException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }
            catch (ImapException ex)
            {
                throw new EmailLogonException(ex.Message, ex);
            }

            return imap;
        }

        /// <summary>
        /// Load the available email accounts from which to send.  If one is already selected,
        /// the selection will be maintained (if it still exists).  If there are no accounts
        /// the UI will be disabled.
        /// </summary>
        public static void LoadEmailAccounts(ComboBox accountComboBox)
        {
            LoadEmailAccounts(accountComboBox, EmailAccountManager.EmailAccounts);
        }

        /// <summary>
        /// Load from the given email accounts.  If one is already selected,
        /// the selection will be maintained (if it still exists).  If there are no accounts
        /// the UI will be disabled.
        /// </summary>
        public static void LoadEmailAccounts(ComboBox accountComboBox, IEnumerable<EmailAccountEntity> accounts)
        {
            long accountID = -1;
            if (accountComboBox.SelectedIndex >= 0)
            {
                accountID = (long) accountComboBox.SelectedValue;
            }

            accountComboBox.DisplayMember = "Key";
            accountComboBox.ValueMember = "Value";

            accountComboBox.DataSource = accounts.Select
                (a => new DictionaryEntry(a.AccountName, a.EmailAccountID))
                .ToList();

            accountComboBox.SelectedValue = accountID;

            if (accountComboBox.SelectedIndex < 0 && accountComboBox.Items.Count > 0)
            {
                accountComboBox.SelectedIndex = 0;
            }

            accountComboBox.Enabled = accountComboBox.Items.Count > 0;
        }

        /// <summary>
        /// Delete the given collection of outbound emails after first confirming with the user that this should be done.
        /// </summary>
        public static void DeleteOutboundEmail(IEnumerable<long> keys, Control owner, BackgroundExecutorCompletedEventHandler<long> completedHandler)
        {
            DialogResult result = MessageHelper.ShowQuestion(owner,
                keys.Take(2).Count() == 2 ? "Delete all selected email messages?" : "Delete the selected email message?");

            if (result != DialogResult.OK)
            {
                return;
            }

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(owner,
                "Delete Email",
                "ShipWorks is deleting email messages.",
                "Deleting {0} of {1}");

            executor.ExecuteCompleted += completedHandler;
            executor.ExecuteAsync((entityID, state, issueAdder) =>
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Then delete the email record
                    adapter.DeleteEntity(new EmailOutboundEntity(entityID));

                    adapter.Commit();
                }

            }, keys);
        }

        /// <summary>
        /// Get the set of keys related to the given email
        /// </summary>
        public static List<long> GetRelatedKeys(long emailID, EmailOutboundRelationType relationType)
        {
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(EmailOutboundRelationFields.ObjectID, 0, "ObjectID", "");

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                EmailOutboundRelationFields.EmailOutboundID == emailID &
                EmailOutboundRelationFields.RelationType == (int) relationType);

            // Do the fetch
            using (SqlDataReader reader = (SqlDataReader) SqlAdapter.Default.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, true))
            {
                List<long> keys = new List<long>();

                while (reader.Read())
                {
                    keys.Add(reader.GetInt64(0));
                }

                return keys;
            }
        }
    }
}
