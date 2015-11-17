using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using Rebex.Net;
using Rebex.Mail;
using Rebex.Mime.Headers;
using ShipWorks.Data;
using System.IO;
using ShipWorks.ApplicationCore;
using ShipWorks.Templates.Processing;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using System.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Utility;
using System.Net.Sockets;
using Interapptive.Shared;
using ShipWorks.Users;

namespace ShipWorks.Email
{
    /// <summary>
    /// Class for sending and recieving email
    /// </summary>
    public static class EmailCommunicator
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailCommunicator));

        // Indiciates if we are currently emailing
        static volatile bool isEmailing = false;

        // The progress dlg currently displayed, or null if not displayed.
        static ProgressDlg progressDlg = null;

        // The busy token
        static ApplicationBusyToken busyToken;

        // The current (or if not emailing, most recent) set of progress items
        static ProgressProvider progressProvider = new ProgressProvider();

        // The current queue of email accounts that are pending sending
        static List<PendingThrottler> emailQueue = new List<PendingThrottler>();
        static object emailQueueLock = new object();

        static DateTime lastAutoEmailCheck = DateTime.MinValue;

        #region class PendingThrottler

        class PendingThrottler
        {
            public EmailOutboundThrottler Throttler { get; set; }
            public ProgressItem ProgressItem { get; set; }
        }

        #endregion

        /// <summary>
        /// Raised when a email is about to start
        /// </summary>
        public static event EventHandler EmailStarting;

        /// <summary>
        /// Raised after a email communication completes, regardless of the outcome
        /// </summary>
        public static event EmailCommunicationCompleteEventHandler EmailCommunicationComplete;

        /// <summary>
        /// Indicates if a emailing operation is currently in progress
        /// </summary>
        public static bool IsEmailing
        {
            get { return isEmailing; }
        }

        /// <summary>
        /// Indicates if the progress window is currently visible
        /// </summary>
        public static bool IsProgressVisible
        {
            get { return progressDlg != null; }
        }

        /// <summary>
        /// Show the progress window modally
        /// </summary>
        public static void ShowProgressDlg(IWin32Window parent)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            using (progressDlg = CreateProgressWindow())
            {
                progressDlg.ShowDialog(parent);
            }

            progressDlg = null;
        }

        /// <summary>
        /// Initiate an auto-email for any accounts that are ready.
        /// </summary>
        public static void StartAutoEmailingIfNeeded()
        {
            // Don't check more often than every 15 seconds
            if (lastAutoEmailCheck + TimeSpan.FromSeconds(15) > DateTime.UtcNow)
            {
                return;
            }

            lastAutoEmailCheck = DateTime.UtcNow;

            List<long> readyList = new List<long>();

            // Check each account
            foreach (EmailAccountEntity account in EmailAccountManager.EmailAccounts)
            {
                // If not auto-sending, then ignore it
                if (!account.AutoSend)
                {
                    continue;
                }

                // See if the time has passed
                if (account.AutoSendLastTime + TimeSpan.FromMinutes(account.AutoSendMinutes) < DateTime.UtcNow)
                {
                    readyList.Add(account.EmailAccountID);

                    log.InfoFormat("Account '{0}' is ready to auto-email.", account.DisplayName);
                }
            }

            // Start the auto-email of the ready accounts
            StartEmailingAccounts(readyList);

            // Make sure the local email account cache gets updated with changed we made to last autodownload times.  If a modal window is open,
            // then the heartbeat won't be going through the code that does this, and we could end up trying to autoemail over and over and over
            // until the modal window was closed (if we didn't do this)
            if (readyList.Count > 0)
            {
                EmailAccountManager.CheckForChangesNeeded();
            }
        }

        /// <summary>
        /// Initiate emailing for all email accounts that have unsent messages.  Returns false if there were no such accounts.
        /// </summary>
        public static bool StartEmailingAccounts()
        {
            // Email all accounts, including normally hidden "Owned" accounts - like the Yahoo! account used for recieving\sending yahoo stuf
            return StartEmailingAccounts(EmailAccountManager.GetEmailAccounts(true).Select(a => a.EmailAccountID));
        }

        /// <summary>
        /// Initiate emailing for the given list of email accounts.  Returns true if some had messages, and sending began, or false
        /// if no accounts had any messages to send.
        /// </summary>
        public static bool StartEmailingAccounts(IEnumerable<long> accounts)
        {
            Debug.Assert(!Program.ExecutionMode.IsUIDisplayed || !Program.MainForm.InvokeRequired);

            bool started = false;

            foreach (long accountID in accounts)
            {
                EmailAccountEntity account = EmailAccountManager.GetAccount(accountID);
                if (account == null)
                {
                    log.WarnFormat("EmailAccount {0} has gone away and no email will be sent.", accountID);
                }
                else
                {
                    // Mark the time when a send of the entire account was last done
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        account.AutoSendLastTime = DateTime.UtcNow;
                        adapter.SaveAndRefetch(account);
                    }

                    EmailSendingSource sendingSource = new EmailSendingAccountSource(account);

                    // Don't do anything if there are none to send
                    if (sendingSource.GetCount() > 0)
                    {
                        started = true;
                        AddToEmailQueue(new EmailOutboundThrottler(sendingSource));
                    }
                }
            }

            return started;
        }

        /// <summary>
        /// Initiate the emailing of the given messages.  If skipDontSendBefore is true messages whose DontSendUntil date has not yet come will be skipped.  Otherwise they will be tried, but if its
        /// still too early, an error will be displayed. Either way they dont actually get sent.
        /// </summary>
        public static void StartEmailingMessages(IEnumerable<EmailOutboundEntity> messages, bool skipDontSendBefore = true)
        {
            Debug.Assert(!Program.ExecutionMode.IsUIDisplayed || !Program.MainForm.InvokeRequired);

            // Make sure the DB is in a state where we can start doing stuff
            if (ConnectionSensitiveScope.IsActive || !UserSession.IsLoggedOn)
            {
                return;
            }

            IEnumerable<EmailOutboundEntity> messagesToSend = messages;

            if (skipDontSendBefore)
            {
                messagesToSend = messages.Where(m => m.DontSendBefore == null || m.DontSendBefore <= DateTime.UtcNow);
            }

            // We have to split up the messages by account
            foreach (IGrouping<long, EmailOutboundEntity> accountGroup in messagesToSend.GroupBy(m => m.AccountID))
            {
                long accountID = accountGroup.Key;

                EmailAccountEntity account = EmailAccountManager.GetAccount(accountID);
                if (account == null)
                {
                    log.WarnFormat("EmailAccount {0} has gone away and no email will be sent.", accountID);
                    return;
                }

                // Create a clone of the account so we are not affected by async changes
                account = new EmailAccountEntity(account.Fields.Clone());

                AddToEmailQueue(new EmailOutboundThrottler(new EmailSendingMessageSource(account, accountGroup.Select(m => m.EmailOutboundID))));
            }
        }

        /// <summary>
        /// Add the given throttler to the list of email sources to send from
        /// </summary>
        private static void AddToEmailQueue(EmailOutboundThrottler throttler)
        {
            Debug.Assert(!Program.ExecutionMode.IsUIDisplayed || !Program.MainForm.InvokeRequired);

            lock (emailQueueLock)
            {
                log.InfoFormat("Adding throttler for ({0}) to email queue.", throttler.EmailAccount.AccountName);

                // We are going to be starting a new email session so create a new progress set.  If the progress window
                // is open right now, it will just keep showing the old stuff.  That's basically OK, b\c the only way
                // it could be open and us starting a new email is if its an auto-email.  And I don't think we'd want
                // to just clear out what they were looking at.
                if (!isEmailing)
                {
                    progressProvider = new ProgressProvider();
                }

                // Create the progress item for this account
                ProgressItem progressItem = new ProgressItem(throttler.EmailAccount.AccountName);
                progressProvider.ProgressItems.Add(progressItem);

                // List how many messages are likely to go up front
                progressItem.Detail = throttler.ProgressPendingDescription;

                // Add to the email throttler to the qeue
                PendingThrottler pendingThrottler = new PendingThrottler { Throttler = throttler, ProgressItem = progressItem };
                emailQueue.Add(pendingThrottler);

                // Ensure our email thread is working.
                if (!isEmailing)
                {
                    Debug.Assert(busyToken == null);

                    // If we are in a context sensitive scope, we have to wait until next time.  If we are on the UI, we'll always get it. 
                    // We only may not if we are running in the background.
                    if (!ApplicationBusyManager.TryOperationStarting("emailing", out busyToken))
                    {
                        return;
                    }

                    isEmailing = true;

                    Thread thread = new Thread(ExceptionMonitor.WrapThread(EmailWorkerThread));
                    thread.Name = "EmailThread";
                    thread.Start();

                    // Raise the starting event
                    if (EmailStarting != null)
                    {
                        EmailStarting(null, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Entry point function for emailing on the background thread
        /// </summary>
        private static void EmailWorkerThread(object state)
        {
            log.InfoFormat("Starting email worker thread.");

            int nextIndexToEmail = 0;

            while (true)
            {
                EmailOutboundThrottler throttler = null;
                ProgressItem progressItem = null;

                lock (emailQueueLock)
                {
                    // If there are no more left in the list to email then break out of the immediate while loop.
                    if (nextIndexToEmail >= emailQueue.Count || progressProvider.CancelRequested)
                    {
                        // Let the operation manager know we are done
                        ApplicationBusyManager.OperationComplete(busyToken);
                        busyToken = null;

                        // Clear the queue and reset our event so we stop back up at that top while loop
                        emailQueue.Clear();

                        // No longer emailing
                        isEmailing = false;

                        log.InfoFormat("Emailing complete");

                        // Raise the completed event
                        if (EmailCommunicationComplete != null)
                        {
                            EmailCommunicationComplete(null, new EmailCommunicationCompleteEventArgs(progressProvider.HasErrors));
                        }           

                        // Escape, we are all done.
                        return;
                    }
                    // Pull the next throttler to use off the queue
                    else
                    {
                        PendingThrottler pendingThrottler = emailQueue[nextIndexToEmail];

                        throttler = pendingThrottler.Throttler;
                        progressItem = pendingThrottler.ProgressItem;

                        nextIndexToEmail++;
                    }
                }

                progressItem.Starting();
                progressItem.Detail = "Preparing...";

                log.InfoFormat("Start emailing for account {0}", throttler.EmailAccount.AccountName);

                bool errors = SendMessagesForThrottler(throttler, progressItem);

                // If its not already marked as failed see what we need to mark it as
                if (progressItem.Status != ProgressItemStatus.Failure)
                {
                    if (errors)
                    {
                        progressItem.Failed(new EmailException("One or more messages failed to send.  See the outbox for details."));
                    }
                    else
                    {
                        progressItem.Completed();
                    }
                }
            }
        }

        /// <summary>
        /// Send all the messages for the given throttler
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static bool SendMessagesForThrottler(EmailOutboundThrottler throttler, ProgressItem progress)
        {
            bool errors = false;

            try
            {
                long? outboxItemID = throttler.RetrieveNextMessageToSend();

                // progress.Status would be failure if we couldn't log on to the mail server
                while (progress.Status != ProgressItemStatus.Failure && !progress.IsCancelRequested)
                {
                    // No more messages to process
                    if (outboxItemID == null)
                    {
                        progress.PercentComplete = 100;
                        progress.Detail = "Done";
                        return errors;
                    }

                    progress.Detail = string.Format("Sending message {0} of {1}...", throttler.MessagesRetrieved, throttler.MessagesTotal);
                    progress.PercentComplete = (100 * (throttler.MessagesRetrieved - 1)) / throttler.MessagesTotal;

                    try
                    {
                        using (SqlEntityLock messageLock = new SqlEntityLock(outboxItemID.Value, "Send Email"))
                        {
                            EmailOutboundEntity outboxItem = new EmailOutboundEntity(outboxItemID.Value);
                            SqlAdapter.Default.FetchEntity(outboxItem);
                            
                            // If its null, its been deleted.  Or if its already send, just ignore it either way.
                            if (outboxItem.Fields.State != EntityState.Fetched || outboxItem.SendStatus == (int) EmailOutboundStatus.Sent)
                            {
                                log.InfoFormat("Outbox item {0} has been deleted or already sent.", outboxItemID);
                            }
                            else
                            {
                                outboxItem.SendAttemptCount++;
                                outboxItem.SendAttemptLastError = "";
                                outboxItem.SentDate = DateTime.UtcNow;

                                try
                                {
                                    if (outboxItem.AccountID != throttler.EmailAccount.EmailAccountID)
                                    {
                                        throw new EmailException("The email account to be used has changed. The message will be sent during the next of the updated account.", EmailExceptionErrorNumber.EmailAccountChanged);
                                    }

                                    // Somehow this snuck in
                                    if (outboxItem.DontSendBefore != null && outboxItem.DontSendBefore > DateTime.UtcNow)
                                    {
                                        throw new EmailException("The message is configured to not be sent until " + StringUtility.FormatFriendlyDateTime(outboxItem.DontSendBefore.Value) + ".", EmailExceptionErrorNumber.DelaySending);
                                    }

                                    // Need to create a Rebex message from our outbox definition
                                    MailMessage mailMessage = CreateMailMessageFromOutbox(outboxItem);

                                    // Send the message
                                    Smtp smtpConnection = throttler.GetSmtpConnection();

                                    try
                                    {
                                        smtpConnection.Send(mailMessage);
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        // I'm not sure why this is being thrown seeing that we check the recipeint before sending. Maybe this will give us some insight.
                                        string errorMessage = string.Format("Argument Exception when sending email to: {0}", mailMessage.To);
                                        log.Error(errorMessage, ex);
                                        throw new EmailException(errorMessage, ex);
                                    }

                                    // Successfully sent
                                    outboxItem.SendStatus = (int) EmailOutboundStatus.Sent;
                                }
                                catch (EmailException ex)
                                {
                                    log.Error("Failed sending mail message", ex);

                                    outboxItem.SendStatus = ex.RetryAllowed ? (int) EmailOutboundStatus.Retry : (int) EmailOutboundStatus.Failed;
                                    outboxItem.SendAttemptLastError = ex.Message;

                                    // If its a logon or throttle exception we don't keep going
                                    if (ex is EmailLogonException || ex is EmailThrottleException)
                                    {
                                        progress.Failed(ex);
                                    }
                                }
                                catch (SmtpException ex)
                                {
                                    log.Error("Failed sending mail message", ex);

                                    outboxItem.SendStatus = (int) EmailOutboundStatus.Retry;
                                    outboxItem.SendAttemptLastError = ex.Message;

                                    // If we are now disconnected, stop
                                    if (ex.Status == SmtpExceptionStatus.ConnectionClosed)
                                    {
                                        progress.Failed(ex);
                                    }
                                }
                                catch (TlsException ex)
                                {
                                    log.Error("Failed sending mail message", ex);

                                    outboxItem.SendStatus = (int) EmailOutboundStatus.Retry;
                                    outboxItem.SendAttemptLastError = ex.Message;

                                    // Go ahead and stop
                                    progress.Failed(ex);
                                }
                                catch (SocketException ex)
                                {
                                    log.Error("Failed sending mail message", ex);

                                    outboxItem.SendStatus = (int)EmailOutboundStatus.Retry;
                                    outboxItem.SendAttemptLastError = ex.Message;

                                    // Go ahead and stop
                                    progress.Failed(ex);
                                }
                                catch (InvalidOperationException ex)
                                {
                                    log.Error("Failed sending mail message", ex);

                                    outboxItem.SendStatus = (int) EmailOutboundStatus.Failed;
                                    outboxItem.SendAttemptLastError = ex.Message;

                                    // Go ahead and stop
                                    progress.Failed(ex);
                                }
 
                                // See if we need to the errors flag
                                if (outboxItem.SendStatus != (int) EmailOutboundStatus.Sent)
                                {
                                    errors = true;
                                }

                                try
                                {
                                    // Save the new state of the message
                                    using (SqlAdapter adapter = new SqlAdapter())
                                    {
                                        adapter.SaveEntity(outboxItem);
                                    }
                                }
                                catch (ORMConcurrencyException ex)
                                {
                                    log.Error(string.Format("Looks like the email {0} got deleted.", outboxItemID), ex);
                                }
                            }
                        }
                    }
                    catch (SqlAppResourceLockException)
                    {
                        log.InfoFormat("Could not obtain email lock for message {0}.", outboxItemID);
                    }

                    // Get the next item to send
                    outboxItemID = throttler.RetrieveNextMessageToSend();
                }
            }
            finally
            {
                throttler.Dispose();
            }

            return errors;
        }

        /// <summary>
        /// Create a Rebex mail message based on the given outbox item
        /// </summary>
        private static MailMessage CreateMailMessageFromOutbox(EmailOutboundEntity outboxItem)
        {
            // Validate the addresses early
            EmailUtility.ValidateAddresses(outboxItem.ToList, outboxItem.CcList, outboxItem.BccList);

            MailMessage mailMessage = new MailMessage();

            // If an encoding has been set suggest it as the default.  Rebex though will automatically figure out the best encoding that doesn't lose
            // any precision if it's not set.  Setting it though would help Rebex pick UTF-32 over UTF-8 for example, if UTF-32 was the intended encoding.
            if (outboxItem.Encoding != null)
            {
                mailMessage.DefaultCharset = Encoding.GetEncoding(outboxItem.Encoding);
            }

            // From
            mailMessage.From = outboxItem.FromAddress;

            // Recipients
            AddRecipients(mailMessage.To, outboxItem.ToList);


            AddRecipients(mailMessage.CC, outboxItem.CcList);
            AddRecipients(mailMessage.Bcc, outboxItem.BccList);

            // Subject
            mailMessage.Subject = outboxItem.Subject;

            // Date
            mailMessage.Date = new MailDateTime(outboxItem.SentDate.ToLocalTime());

            // The plain text part
            DataResourceReference plainEmailTextDataResourceReference = DataResourceManager.LoadResourceReference(outboxItem.PlainPartResourceID);
            if (plainEmailTextDataResourceReference == null)
            {
                log.ErrorFormat("An error has occurred processing EmailOutboundEntity with ID of {0}. Could not get Resource for PlainPartResourceID {1}.", outboxItem.EmailOutboundID, outboxItem.PlainPartResourceID);
                throw new EmailException("An error has occurred retrieving the body of the email.", EmailExceptionErrorNumber.EmailBodyProcessingFailed);
            }

            string plainEmailText = LoadDataResourceReferenceText(plainEmailTextDataResourceReference, outboxItem.EmailOutboundID);

            mailMessage.BodyText = plainEmailText;

            // Apply the html part
            if (outboxItem.HtmlPartResourceID != null)
            {
                ApplyHtmlMimeImages(mailMessage, outboxItem);
            }

            return mailMessage;
        }

        /// <summary>
        /// Apply all the images from the html for the messages as mime attachments
        /// </summary>
        private static void ApplyHtmlMimeImages(MailMessage mailMessage, EmailOutboundEntity outboxItem)
        {
            List<DataResourceReference> resourceReferences = DataResourceManager.GetConsumerResourceReferences(outboxItem.EmailOutboundID);

            TemplateHtmlImageProcessor imageProcessor = new TemplateHtmlImageProcessor();
            imageProcessor.LocalImages = true;
            imageProcessor.OnlineImages = false;

            // Maps the data resources to the ContentID's we have given them.  We want to give them ID's that match the actual name
            // of the file they came from.  But since the files could come from different paths - and have the exact same filename - 
            // we have to make sure we don't create duplicates.
            Dictionary<long, string> contentIDs = new Dictionary<long, string>();

            // Load the html part, ensuring that it exists.  According to the resource manager, this should only fail if the resource was
            // manually deleted, but even in that case, we can handle it more gracefully than a crash.
            DataResourceReference htmlTextDataResourceReference = DataResourceManager.LoadResourceReference(outboxItem.HtmlPartResourceID.Value);
            if (htmlTextDataResourceReference == null)
            {
                log.ErrorFormat("An error has occurred processing EmailOutboundEntity with ID of {0}. Could not get Resource for HtmlPartResourceID {1}.", outboxItem.EmailOutboundID, outboxItem.HtmlPartResourceID);
                throw new EmailException("An error has occurred retrieving the body of the email.", EmailExceptionErrorNumber.EmailBodyProcessingFailed);
            }

            // Try to get the html text for the email
            string htmlText = LoadDataResourceReferenceText(htmlTextDataResourceReference, outboxItem.EmailOutboundID);

            // We have to convert all the local image references to cid's for inline html images
            string htmlContent = imageProcessor.Process(
                htmlText, 
                (HtmlAttribute attribute, Uri srcUri, string imageName) =>
                {
                    // See if we can find the matching resource.  Could be more than one: If they had two images that were exactly the same, yet named different filenames, 
                    // they'd each be logged as a unique reference, but would reference the same resource.
                    DataResourceReference resourceReference = resourceReferences.Where(r => string.Compare(r.Filename, attribute.Value, true) == 0).FirstOrDefault();

                    // Should always be there
                    Debug.Assert(resourceReference != null);
                    if (resourceReference != null)
                    {
                        // See if this resource has been attached yet
                        string contentID;
                        if (!contentIDs.TryGetValue(resourceReference.ResourceID, out contentID))
                        {
                            contentID = string.Format("{0}_{1}{2}", 
                                Path.GetFileNameWithoutExtension(resourceReference.Label), 
                                mailMessage.Resources.Count,
                                Path.GetExtension(resourceReference.Label));

                            // Create the attachment
                            LinkedResource mailResource = new LinkedResource(
                                resourceReference.GetCachedFilename(),
                                GetMimeType(Path.GetExtension(resourceReference.Filename)));
                            mailResource.ContentId = contentID;

                            // Add the attachment
                            mailMessage.Resources.Add(mailResource);

                            // In case this resource is used again we'll just reference the contentID we already added
                            contentIDs.Add(resourceReference.ResourceID, contentID);
                        }

                        // Update the html to reference the CID format
                        attribute.Value = "cid:" + contentID;
                    }
                });

            // Assign the html to the message
            mailMessage.BodyHtml = htmlContent;
        }

        /// <summary>
        /// Attempts to read all text for the desired data resource reference
        /// </summary>
        /// <exception cref="EmailException">If an UnauthorizedAccessException or IOException (only when the file is currently being used) is thrown, 
        /// an EmailException will be thrown with the inner exception being the originally thrown exception.  If an IOException is thrown and it is not a "being used by another process" exception,
        /// the original IOException will be rethrown.</exception>
        private static string LoadDataResourceReferenceText(DataResourceReference dataResourceReference, long emailOutboundID)
        {
            string htmlText = string.Empty;
            try
            {
                htmlText = dataResourceReference.ReadAllText();
            }
            catch (IOException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("being used by another process".ToUpperInvariant()))
                {
                    log.ErrorFormat("An error has occurred processing EmailOutboundEntity with ID of {0}. Could not get Resource for ReferenceID {1}.", emailOutboundID, dataResourceReference.ReferenceID);
                    throw new EmailException("An error has occurred retrieving the body of the email.", ex, EmailExceptionErrorNumber.EmailBodyProcessingFailed);
                }

                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.ErrorFormat("An error has occurred processing EmailOutboundEntity with ID of {0}. Could not get Resource for ReferenceID {1}.", emailOutboundID, dataResourceReference.ReferenceID);
                throw new EmailException("An error has occurred retrieving the body of the email.", ex, EmailExceptionErrorNumber.EmailBodyProcessingFailed);
            }

            return htmlText;
        }

        /// <summary>
        /// Add all the recipients to the address list
        /// </summary>
        private static void AddRecipients(MailAddressCollection addressList, string recipients)
        {
            foreach (string recipient in EmailUtility.SplitAddressList(recipients))
            {
                addressList.Add(recipient);
            }
        }

        /// <summary>
        /// Get the probably MIME type based on the given file extension
        /// </summary>
        private static string GetMimeType(string extension)
        {
            string postfix = null;

            if (extension.StartsWith("."))
            {
                extension = extension.Remove(0, 1);
            }

            extension = extension.ToLowerInvariant();

            switch (extension)
            {
                case "jpeg":
                case "bmp":
                case "gif":
                case "ief":
                case "tiff":
                case "png":
                    postfix = extension;
                    break;

                case "jpg":
                    postfix = "jpeg";
                    break;

                case "tif":
                    postfix = "tiff";
                    break;

                default:
                    postfix = extension;
                    break;
            }

            return string.Format("image/{0}", postfix);
        }

        /// <summary>
        /// Create the window that will be used for displaying progess
        /// </summary>
        private static ProgressDlg CreateProgressWindow()
        {
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Sending Email";
            progressDlg.Description = "ShipWorks is sending email.";

            progressDlg.ActionColumnHeaderText = "Account";

            // Implement the hiding
            progressDlg.AllowCloseWhenRunning = true;
            progressDlg.AutoCloseWhenComplete = true;

            progressDlg.CloseTextWhenRunning = "Hide";
            progressDlg.CloseTextWhenComplete = "Close";

            return progressDlg;
        }
    }
}
