using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebex.Net;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Result of searching for user's email settings
    /// </summary>
    public class EmailSettingsSearchResult
    {
        bool success;
        static EmailSettingsSearchResult failedResult = new EmailSettingsSearchResult { success = false };

        string smtpHost;
        int smtpPort;
        string smtpUsername;
        string smtpPassword;
        SmtpSecurity smtpSecurity;

        string incomingHost;
        EmailIncomingServerType incomingHostType;
        int incomingPort;
        string incomingUsername;
        string incomingPassword;
        EmailIncomingSecurityType incomingSecurity;

        /// <summary>
        /// Private constructor for failed result
        /// </summary>
        private EmailSettingsSearchResult()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSettingsSearchResult"/> class.
        /// </summary>
        /// <param name="smtpResult">The SMTP result.</param>
        /// <param name="incomingResult">The incoming result.</param>
        public EmailSettingsSearchResult(SmtpProbeResult smtpResult, IncomingProbeResult incomingResult)
        {
            success = true;

            smtpHost = smtpResult.Host;
            smtpPort = smtpResult.Port;
            smtpUsername = smtpResult.Username;
            smtpPassword = smtpResult.Password;
            smtpSecurity = smtpResult.SmtpSecurity;

            incomingHost = incomingResult.Host;
            incomingHostType = incomingResult.HostType;
            incomingPort = incomingResult.Port;
            incomingUsername = incomingResult.Username;
            incomingPassword = incomingResult.Password;
            incomingSecurity = incomingResult.IncomingSecurity;
        }

        /// <summary>
        /// Creates an instance of the search results that indicates failure
        /// </summary>
        public static EmailSettingsSearchResult FailedResult
        {
            get { return failedResult; }
        }

        /// <summary>
        /// Indicates if the search was a success.  If false the values of the other properties are invalid.
        /// </summary>
        public bool Success
        {
            get { return success; }
        }

        public string SmtpHost
        {
            get { return smtpHost; }
        }

        public int SmtpPort
        {
            get { return smtpPort; }
        }

        public string SmtpUsername
        {
            get { return smtpUsername; }
        }

        public string SmtpPassword
        {
            get { return smtpPassword; }
        }

        public SmtpSecurity SmtpSecurity
        {
            get { return smtpSecurity; }
        }

        public string IncomingHost
        {
            get { return incomingHost; }
        }

        public EmailIncomingServerType IncomingHostType
        {
            get { return incomingHostType; }
        }

        public int IncomingPort
        {
            get { return incomingPort; }
        }

        public string IncomingUsername
        {
            get { return incomingUsername; }
        }

        public string IncomingPassword
        {
            get { return incomingPassword; }
        }

        public EmailIncomingSecurityType IncomingSecurity
        {
            get { return incomingSecurity; }
        }
    }
}
