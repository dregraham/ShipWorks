using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;
using log4net;
using System.Threading;
using Rebex.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Class for finding the most likely email settings for a given email account
    /// </summary>
    public class EmailSettingsSearcher
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailSettingsSearcher));

        // Provides progress through the status of the search
        ProgressProvider progressProvider = new ProgressProvider();

        // Optionall force the type of incoming server that is considered
        EmailIncomingServerType? forceIncomingServerType;

        /// <summary>
        /// Raised when an async search operation has completed
        /// </summary>
        public event EmailSettingsSearchCompletedEventHandler SearchCompleted;

        #region class SearchRequest

        class SearchRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public object UserState { get; set; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSettingsSearcher(EmailIncomingServerType? forceIncomingServerType)
        {
            this.forceIncomingServerType = forceIncomingServerType;
        }

        /// <summary>
        /// Exposes the current set of operations the search is working on and their status.
        /// </summary>
        public ProgressProvider ProgressProvider
        {
            get { return progressProvider; }
        }

        /// <summary>
        /// Initiates the search process asyncronously.
        /// </summary>
        public void SearchAsync(string emailAddress, string password)
        {
            SearchAsync(emailAddress, password, null);
        }

        /// <summary>
        /// Initiates the search process asyncronously.
        /// </summary>
        public void SearchAsync(string emailAddress, string password, object userState)
        {
            progressProvider.ProgressItems.Clear();

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(SearchCallback), new SearchRequest { EmailAddress = emailAddress, Password = password, UserState = userState });
        }

        /// <summary>
        /// Worker thread for executing a save request
        /// </summary>
        private void SearchCallback(object state) 
        {
            SearchRequest request = (SearchRequest) state;

            // The args for completion
            EmailSettingsSearchCompletedEventArgs completionArgs;

            try
            {
                // Initiate the worker.  It returns null if it was canceled
                EmailSettingsSearchResult searchResult = SearchWorker(request.EmailAddress, request.Password);

                // Everything was fine
                completionArgs = new EmailSettingsSearchCompletedEventArgs(searchResult, null, searchResult == null, request.UserState);
            }
            catch (Exception ex)
            {
                // Pass on the unknown exception as an error as is
                completionArgs = new EmailSettingsSearchCompletedEventArgs(null, ex, false, request.UserState);
            }

            // Callback the completion handler
            if (SearchCompleted != null)
            {
                SearchCompleted(this, completionArgs);
            }
        }

        /// <summary>
        /// Do the actual work of the search. Returns null if canceled, or the results otherwise.
        /// </summary>
        private EmailSettingsSearchResult SearchWorker(string emailAddress, string password)
        {
            ProgressItem progressOutgoingMail = new ProgressItem("Outgoing Mail");
            ProgressItem progressIncomingMail = new ProgressItem("Incoming Mail");

            // Add the progress items
            progressProvider.ProgressItems.Clear();
            progressProvider.ProgressItems.Add(progressOutgoingMail);
            progressProvider.ProgressItems.Add(progressIncomingMail);

            // Usernames to attempt using
            List<string> userNames = DeterminePotentialUsernames(emailAddress);

            // Hosts to attempt
            List<string> smtpHosts = new List<string>();
            List<string> imapHosts = new List<string>();
            List<string> pop3Hosts = new List<string>();
            DeterminePotentialHosts(emailAddress, smtpHosts, imapHosts, pop3Hosts);

            // Start the outgoing search progress
            progressOutgoingMail.Detail = "Searching for your settings...";
            progressOutgoingMail.Starting();

            // Do the outgoing search
            SmtpProbeResult smtpResult = ProbeSmtpSettings(progressOutgoingMail, smtpHosts, userNames, password);

            // See if they canceled
            if (progressProvider.CancelRequested)
            {
                return null;
            }

            // See if we found the settings
            if (smtpResult == null)
            {
                progressProvider.Terminate(new SmtpException("Failed to find settings."));
                return EmailSettingsSearchResult.FailedResult;
            }

            // Mark that progress as complete
            progressOutgoingMail.PercentComplete = 100;
            progressOutgoingMail.Detail = "Done";
            progressOutgoingMail.Completed();

            // Start the incoming search progress
            progressIncomingMail.Detail = "Searching for your settings....";
            progressIncomingMail.Starting();

            // Do the incoming search
            IncomingProbeResult incomingResult = ProbeIncomingSettings(progressIncomingMail, imapHosts, pop3Hosts, userNames, password);

            // See if they canceled
            if (progressProvider.CancelRequested)
            {
                return null;
            }
            
            // See if we found the settings
            if (incomingResult == null)
            {
                progressProvider.Terminate(new Pop3Exception("Failed to find settings."));
                return EmailSettingsSearchResult.FailedResult;
            }

            // Mark that progress as complete
            progressIncomingMail.PercentComplete = 100;
            progressIncomingMail.Detail = "Done";
            progressIncomingMail.Completed();

            return new EmailSettingsSearchResult(smtpResult, incomingResult);
        }

        /// <summary>
        /// Probe for smtp settings using the list of potential hosts and usernames
        /// </summary>
        private SmtpProbeResult ProbeSmtpSettings(IProgressReporter progress, List<string> smtpHosts, List<string> userNames, string password)
        {
            // List of probes to try in priority order
            List<Func<string, string, string, SmtpProbeResult>> probes = new List<Func<string, string, string, SmtpProbeResult>>();
            probes.Add(ProbeSmtpSecureExplicit);
            probes.Add(ProbeSmtpSecureImplicit);
            probes.Add(ProbeSmtpUnsecure);

            int count = 0;

            // Try each probe in priority order
            foreach (Func<string, string, string, SmtpProbeResult> probe in probes)
            {
                // Try each host
                foreach (string smtpHost in smtpHosts)
                {
                    // Try each username
                    foreach (string username in userNames)
                    {
                        if (progressProvider.CancelRequested)
                        {
                            return null;
                        }

                        try
                        {
                            SmtpProbeResult result = probe(smtpHost, username, password);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                        // server name invalid
                        catch (ArgumentException ex)
                        {
                            if (String.Compare(ex.ParamName, "serverName", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                log.Error("Probe failed.", ex);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        // Certificate problem
                        catch (TlsException ex)
                        {
                            log.Error("Probe failed.", ex);
                        }
                        // Auth, socket, (or other) problems
                        catch (SmtpException ex)
                        {
                            log.Error("Probe failed.", ex);
                        }

                        progress.PercentComplete = (100 * ++count) / (probes.Count * smtpHosts.Count * userNames.Count);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Probe for IMAP or POP settings using the list of potential hosts and usernames
        /// </summary>
        private IncomingProbeResult ProbeIncomingSettings(IProgressReporter progress, List<string> imapHosts, List<string> pop3Hosts, List<string> userNames, string password)
        {
            // List of IMAP probes to try in priority order
            List<Func<string, string, string, IncomingProbeResult>> imapProbes = new List<Func<string, string, string, IncomingProbeResult>>();

            if (forceIncomingServerType == null || forceIncomingServerType == EmailIncomingServerType.Imap)
            {
                imapProbes.Add(ProbeImapSecureImplicit);
                imapProbes.Add(ProbeImapSecureExplicit);
                imapProbes.Add(ProbeImapUnsecure);
            }

            // List of POP probes to try in priority order
            List<Func<string, string, string, IncomingProbeResult>> popProbes = new List<Func<string, string, string, IncomingProbeResult>>();

            if (forceIncomingServerType == null || forceIncomingServerType == EmailIncomingServerType.Pop3)
            {
                popProbes.Add(ProbePopSecureImplicit);
                popProbes.Add(ProbePopSecureExplicit);
                popProbes.Add(ProbePopUnsecure);
            }

            int totalProbes = (imapProbes.Count * imapHosts.Count * userNames.Count) + (popProbes.Count * pop3Hosts.Count * userNames.Count);
            int count = 0;

            // Try IMAP first
            foreach (Func<string, string, string, IncomingProbeResult> probe in imapProbes)
            {
                foreach (string imapHost in imapHosts)
                {
                    // Try each username
                    foreach (string username in userNames)
                    {
                        try
                        {
                            if (progressProvider.CancelRequested)
                            {
                                return null;
                            }

                            IncomingProbeResult result = probe(imapHost, username, password);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                        // Certificate problem
                        catch (TlsException ex)
                        {
                            log.Error("Probe failed.", ex);
                        }
                        // Auth, socket, (or other) problems
                        catch (ImapException ex)
                        {
                            log.Error("Probe failed.", ex);
                        }

                        progress.PercentComplete = (100 * ++count) / totalProbes;
                    }
                }
            }

            // Try each POP probe in priority order
            foreach (Func<string, string, string, IncomingProbeResult> probe in popProbes)
            {
                // Try each host
                foreach (string popHost in pop3Hosts)
                {
                    // Try each username
                    foreach (string username in userNames)
                    {
                        try
                        {
                            if (progressProvider.CancelRequested)
                            {
                                return null;
                            }

                            IncomingProbeResult result = probe(popHost, username, password);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                        // Certificate problem
                        catch (TlsException ex)
                        {
                            log.Error("Probe failed.", ex);
                        }
                       // Auth, socket, (or other) problems
                        catch (Pop3Exception ex)
                        {
                            log.Error("Probe failed.", ex);
                        }

                        progress.PercentComplete = (100 * ++count) / totalProbes;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Probe to determine if an impliclity secure connection can be made
        /// </summary>
        private SmtpProbeResult ProbeSmtpSecureImplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing SMTP implicit secure ({0}:{1})", host, username);
            return ProbeSmtp(host, username, password, 465, SmtpSecurity.Implicit);
        }

        /// <summary>
        /// Probe to determine if an explicitly secure connection can be made
        /// </summary>
        private SmtpProbeResult ProbeSmtpSecureExplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing SMTP explicit secure ({0}:{1})", host, username);
            return ProbeSmtp(host, username, password, 587, SmtpSecurity.Explicit);
        }

        /// <summary>
        /// Probe to determine if an unsecure connection can be made
        /// </summary>
        private SmtpProbeResult ProbeSmtpUnsecure(string host, string username, string password)
        {
            log.InfoFormat("Probing SMTP unsecure ({0}:{1})", host, username);
            return ProbeSmtp(host, username, password, 25, SmtpSecurity.Unsecure);
        }

        /// <summary>
        /// SMTP settings probe generic function
        /// </summary>
        private SmtpProbeResult ProbeSmtp(string host, string username, string password, int port, SmtpSecurity smtpSecurity)
        {
            TlsParameters tlsParameters = null;

            if (smtpSecurity != SmtpSecurity.Unsecure)
            {
                tlsParameters = new TlsParameters();
                tlsParameters.CertificateVerifier = CertificateVerifier.AcceptAll;
            }

            using (Smtp smtp = new Smtp())
            {
                smtp.Connect(host, port, tlsParameters, smtpSecurity);
                Debug.Assert(smtp.IsSecured == (smtpSecurity != SmtpSecurity.Unsecure));

                // Now try to login
                smtp.Login(username, password);
                Debug.Assert(smtp.IsAuthenticated);

                smtp.Disconnect();
            }

            // Return the valid connection information
            return new SmtpProbeResult { 
                Host = host, 
                Username = username, 
                Password = password, 
                Port = port, 
                SmtpSecurity = smtpSecurity };
        }

        /// <summary>
        /// Probe to determine if an impliclity secure connection can be made
        /// </summary>
        private IncomingProbeResult ProbeImapSecureImplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing IMAP implicit secure ({0}:{1})", host, username);
            return ProbeImap(host, username, password, 993, EmailIncomingSecurityType.Implicit);
        }

        /// <summary>
        /// Probe to determine if an explicitly secure connection can be made
        /// </summary>
        private IncomingProbeResult ProbeImapSecureExplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing IMAP explicit secure ({0}:{1})", host, username);
            return ProbeImap(host, username, password, 143, EmailIncomingSecurityType.Explicit);
        }

        /// <summary>
        /// Probe to determine if an unsecure connection can be made
        /// </summary>
        private IncomingProbeResult ProbeImapUnsecure(string host, string username, string password)
        {
            log.InfoFormat("Probing IMAP unsecure ({0}:{1})", host, username);
            return ProbeImap(host, username, password, 143, EmailIncomingSecurityType.Unsecure);
        }

        /// <summary>
        /// POP settings probe generic function
        /// </summary>
        private static IncomingProbeResult ProbeImap(string host, string username, string password, int port, EmailIncomingSecurityType incomingSecurity)
        {
            TlsParameters tlsParameters = null;

            if (incomingSecurity != EmailIncomingSecurityType.Unsecure)
            {
                tlsParameters = new TlsParameters();
                tlsParameters.CertificateVerifier = CertificateVerifier.AcceptAll;
            }

            using (Imap imap = new Imap())
            {
                // Casting to ImapSecurity is OK - we use the same raw values
                imap.Connect(host, port, tlsParameters, (ImapSecurity) incomingSecurity);
                Debug.Assert(imap.IsSecured == (incomingSecurity != EmailIncomingSecurityType.Unsecure));

                // Now try to login
                imap.Login(username, password);
                Debug.Assert(imap.IsAuthenticated);

                imap.Disconnect();
            }

            // Return the valid connection information
            return new IncomingProbeResult
            {
                Host = host,
                HostType = EmailIncomingServerType.Imap,
                Username = username,
                Password = password,
                Port = port,
                IncomingSecurity = incomingSecurity
            };
        }

        /// <summary>
        /// Probe to determine if an impliclity secure connection can be made
        /// </summary>
        private IncomingProbeResult ProbePopSecureImplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing POP implicit secure ({0}:{1})", host, username);
            return ProbePop(host, username, password, 995, EmailIncomingSecurityType.Implicit);
        }

        /// <summary>
        /// Probe to determine if an explicitly secure connection can be made
        /// </summary>
        private IncomingProbeResult ProbePopSecureExplicit(string host, string username, string password)
        {
            log.InfoFormat("Probing POP explicit secure ({0}:{1})", host, username);
            return ProbePop(host, username, password, 110, EmailIncomingSecurityType.Explicit);
        }

        /// <summary>
        /// Probe to determine if an unsecure connection can be made
        /// </summary>
        private IncomingProbeResult ProbePopUnsecure(string host, string username, string password)
        {
            log.InfoFormat("Probing POP unsecure ({0}:{1})", host, username);
            return ProbePop(host, username, password, 110, EmailIncomingSecurityType.Unsecure);
        }

        /// <summary>
        /// POP settings probe generic function
        /// </summary>
        private static IncomingProbeResult ProbePop(string host, string username, string password, int port, EmailIncomingSecurityType incomingSecurity)
        {
            TlsParameters tlsParameters = null;

            if (incomingSecurity != EmailIncomingSecurityType.Unsecure)
            {
                tlsParameters = new TlsParameters();
                tlsParameters.CertificateVerifier = CertificateVerifier.AcceptAll;
            }

            using (Pop3 pop3 = new Pop3())
            {
                // Casting to Pop3Security is OK - we use the same raw values
                pop3.Connect(host, port, tlsParameters, (Pop3Security) incomingSecurity);
                Debug.Assert(pop3.IsSecured == (incomingSecurity != EmailIncomingSecurityType.Unsecure));

                // Now try to login
                pop3.Login(username, password);
                Debug.Assert(pop3.IsAuthenticated);

                pop3.Disconnect();
            }

            // Return the valid connection information
            return new IncomingProbeResult
            {
                Host = host,
                HostType = EmailIncomingServerType.Pop3,
                Username = username,
                Password = password,
                Port = port,
                IncomingSecurity = incomingSecurity
            };
        }

        /// <summary>
        /// Based on the given email address, generate a list of usernames that it could be
        /// </summary>
        private List<string> DeterminePotentialUsernames(string emailAddress)
        {
            List<string> usernames = new List<string>();

            // Always the address itself
            usernames.Add(emailAddress);

            // If it has an @ (which it should), then just the username part
            if (emailAddress.IndexOf('@') > 0)
            {
                usernames.Add(emailAddress.Substring(0, emailAddress.IndexOf('@')));
            }

            return usernames;
        }

        /// <summary>
        /// Determining the list of potential SMTP and POP hosts based on the given email address
        /// </summary>
        private static void DeterminePotentialHosts(string emailAddress, List<string> smtpHosts, List<string> imapHosts, List<string> popHosts)
        {
            // First fine the domain
            string domain = emailAddress;
            if (emailAddress.IndexOf('@') > 0)
            {
                domain = emailAddress.Substring(emailAddress.IndexOf('@') + 1);
            }

            // To make comparisons easier
            domain = domain.ToLowerInvariant();

            // List of yahoo affiliates
            List<string> yahooPartners = new List<string> {
                "ameritech.net",
                "flash.net",
                "nvbell.net",
                "pacbell.net",
                "prodigy.net",
                "sbcglobal.net",
                "snet.net",
                "swbell.net",
                "wans.net"
            };

            // Yahoo! business or plus accounts
            if (domain == "yahoo.com")
            {
                smtpHosts.Add("smtp.bizmail.yahoo.com");
                smtpHosts.Add("plus.smtp.mail.yahoo.com");

                popHosts.Add("pop.bizmail.yahoo.com");
                popHosts.Add("plus.pop.mail.yahoo.com");
            }

            // Yahoo partners
            else if (yahooPartners.Contains(domain))
            {
                // Get just the main name
                string core = domain.Substring(0, domain.IndexOf('.'));

                // Add the correct host
                smtpHosts.Add("smtp." + core + ".yahoo.com");
                popHosts.Add("pop." + core + ".yahoo.com");
            }

            // Yahoo partner (special case)
            else if (domain == "verizon.net")
            {
                smtpHosts.Add("outgoing.yahoo.verizon.net");
                popHosts.Add("incoming.yahoo.verizon.net");
            }

            // Try just adding a "smtp" infront of it
            else
            {
                smtpHosts.Add("smtp." + domain);
                smtpHosts.Add("mail." + domain);

                imapHosts.Add("imap." + domain);

                popHosts.Add("pop." + domain);
                popHosts.Add("mail." + domain);
                popHosts.Add("pop3." + domain);
            }
        }
    }
}
