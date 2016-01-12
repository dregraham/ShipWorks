using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using System.Threading;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Controls.Html;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using Rebex.Mail;
using Rebex.Net;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using System.IO;
using ShipWorks.ApplicationCore;
using ShipWorks.Email;
using ShipWorks.Users.Security;
using System.Diagnostics;
using Interapptive.Shared;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Class for generating email messages and queing them to the outbox
    /// </summary>
    public class EmailGenerator
    {        
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailGenerator));

        // The template to use.  This is a cloned snapshot of the template at the time the job was created.
        TemplateEntity template;

        // The entities used in template processing.  May be null if we were given the processint templateresults directly
        List<long> entityKeys;

        // The result of template processing
        IList<TemplateResult> templateResults;

        // Provides progress through the status of the save
        ProgressProvider progressProvider = new ProgressProvider();

        /// <summary>
        /// Used to resolve which email settings to use
        /// </summary>
        EmailSettingsResolver settingsResolver = new EmailSettingsResolver();

        /// <summary>
        /// Raised when the user must specify which set of email settings to use.  Must be handled.
        /// </summary>
        public event EmailSettingsResolveEventHandler ResolveSettings;

        /// <summary>
        /// Raised when a send date delay for an email is being determined.  If not handled, then there is no delayed send date.
        /// </summary>
        public event EmailSendDateDelayProviderEventHandler ProvideSendDateDelay;

        /// <summary>
        /// Raised when emailing is complete.
        /// </summary>
        public event EmailGenerationCompletedEventHandler EmailGenerationCompleted;

        // The userState for the save operation
        object userState;

        // The list of emails generated
        List<EmailOutboundEntity> emailsGenerated = new List<EmailOutboundEntity>();

        // Indicates if any messages were not generated due to security problems
        int securityDenials = 0;

        /// <summary>
        /// Private constructor.  Use the Create method instead.
        /// </summary>
        private EmailGenerator()
        {
            // Forward the resolve settings event from the resolver
            settingsResolver.ResolveSettings += new EmailSettingsResolveEventHandler(OnResolveSettings);
        }

        /// <summary>
        /// Create a new EmailSender based on the default settings from the template and the given set of keys
        /// </summary>
        public static EmailGenerator Create(TemplateEntity template, IEnumerable<long> entityKeys)
        {
            if (template.Type == (int) TemplateType.Thermal)
            {
                throw new EmailException(
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be emailed.", EmailExceptionErrorNumber.InvalidTemplateSelected);
            }

            EmailGenerator generator = new EmailGenerator();
            generator.template = template;

            generator.entityKeys = entityKeys.ToList();

            return generator;
        }

        /// <summary>
        /// Create a new EmailSender based on the default settings form the template, and using the given pre-processed results
        /// </summary>
        public static EmailGenerator Create(TemplateEntity template, IList<TemplateResult> templateResults)
        {
            if (template.Type == (int) TemplateType.Thermal)
            {
                throw new EmailException(
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be emailed.", EmailExceptionErrorNumber.InvalidTemplateSelected);
            }

            EmailGenerator generator = new EmailGenerator();
            generator.template = template;

            generator.templateResults = templateResults.ToList();

            return generator;
        }

        /// <summary>
        /// Exposes the current set of operations the email is doing and their status.
        /// </summary>
        public ProgressProvider ProgressProvider
        {
            get { return progressProvider; }
        }

        /// <summary>
        /// The userState object for the currently executing async operation
        /// </summary>
        public object UserState
        {
            get { return userState; }
        }

        /// <summary>
        /// Generate email outbox items syncronously.  Returns true if success, or false if canceled.  Exceptions are thrown on error.
        /// </summary>
        public List<EmailOutboundEntity> Generate()
        {
            try
            {
                emailsGenerated = new List<EmailOutboundEntity>();
                EmailWorker();

                return emailsGenerated;
            }
            catch (TemplateCancelException)
            {
                throw new InvalidOperationException("Should not be possible to cancel the syncronous operation.");
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to email using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in an EmailException
                throw new EmailException(message, ex, EmailExceptionErrorNumber.InvalidTemplateSelected);
            }
        }

        /// <summary>
        /// Initiates the generation process asyncronously.
        /// </summary>
        public void GenerateAsync(object userState)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired);

            progressProvider.ProgressItems.Clear();
            this.userState = userState;

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(EmailRequestCallback, "emailing"));
        }

        /// <summary>
        /// Worker thread for executing an email request
        /// </summary>
        private void EmailRequestCallback(object state)
        {
            // The args for completion
            EmailGenerationCompletedEventArgs completionArgs;

            try
            {
                // Initiate the worker.  It returns false if it was canceled
                bool canceled = !EmailWorker();

                // Everything was fine
                completionArgs = new EmailGenerationCompletedEventArgs(emailsGenerated, securityDenials, null, canceled, userState);
            }
            catch (TemplateCancelException)
            {
                // Don't pass on the exception, mark it as cancelled
                completionArgs = new EmailGenerationCompletedEventArgs(emailsGenerated, securityDenials, null, true, userState);
            }
            catch (TemplateException ex)
            {
                string message = "An error occurred while trying to email using the selected template:\n\n" + ex.Message;

                // Wrap TemplateException in a SaveException
                completionArgs = new EmailGenerationCompletedEventArgs(emailsGenerated, securityDenials, new EmailException(message, ex, EmailExceptionErrorNumber.InvalidTemplateSelected), false, userState);
            }
            catch (Exception ex)
            {
                // Pass on the unknown exception as an error as is
                completionArgs = new EmailGenerationCompletedEventArgs(emailsGenerated, securityDenials, ex, false, userState);
            }

            // Callback the completion handler
            if (EmailGenerationCompleted != null)
            {
                EmailGenerationCompleted(this, completionArgs);
            }

            this.userState = null;
        }

        /// <summary>
        /// Do the actual work of the email generation. Returns false if canceled, true otherwise.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool EmailWorker()
        {
            // Need the progress item
            ProgressItem progress = new ProgressItem("Preparing");
            progressProvider.ProgressItems.Add(progress);

            // Prepare the results
            if (templateResults == null)
            {
                templateResults = TemplateProcessor.ProcessTemplate(template, entityKeys, progress);
            }

            // Add a progress item for the actual generation
            ProgressItem emailProgress = new ProgressItem("Outbox");
            progressProvider.ProgressItems.Add(emailProgress);
            emailProgress.Starting();

            HtmlControl htmlControl = null;

            // Create the html control that will be used
            bool processHtml = TemplateHelper.GetEffectiveOutputFormat(template) == TemplateOutputFormat.Html;
            if (processHtml)
            {
                htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(null);
            }

            try
            {
                emailProgress.Detail = "Adding messages to outbox...";

                // Create the settings to use for formatting the html
                TemplateResultFormatSettings formatSettings = TemplateResultFormatSettings.FromTemplate(template);

                while (formatSettings.NextResultIndex < templateResults.Count && 
                       !progressProvider.CancelRequested)
                {
                    int startResult = formatSettings.NextResultIndex;
                    int endResult;

                    string htmlContent = null;
                    string plainContent = null;

                    // If the results are saved as HTML, we have to do all our html processing
                    if (processHtml)
                    {
                        htmlControl.Html = TemplateResultFormatter.FormatHtml(
                            templateResults,
                            TemplateResultUsage.Email,
                            formatSettings);

                        htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                        // Process sure size now that its ready
                        TemplateSureSizeProcessor.Process(htmlControl);

                        // Since it may be opened or viewed later in word, we have to do this, since word screws up without it
                        HtmlUtility.SetExplicitImageSizes(htmlControl);

                        try
                        {
                            // The final html is the content to save
                            htmlContent = htmlControl.Html;
                            endResult = formatSettings.NextResultIndex;
                        }
                        catch (InvalidCastException ex)
                        {
                            // In certain situations, getting the HTML from the control fails. In this case,
                            // it seems better to notify the user that the template failed instead of crashing ShipWorks since
                            // we had a customer who could no longer run ShipWorks because an action that used a template would
                            // run at startup and crash immediately.
                            throw new TemplateException("Could not generate template", ex);
                        }
                    }
                    // Otherwise just determine the results to use ourself
                    else
                    {
                        plainContent = templateResults[formatSettings.NextResultIndex++].ReadResult();
                        endResult = startResult + 1;
                    }

                    List<TemplateResult> resultsUsed = templateResults.Skip(startResult).Take(endResult - startResult).ToList();

                    // Send the message now
                    if (AddToOutbox(resultsUsed, htmlContent, plainContent))
                    {
                        // Update how much we are done
                        emailProgress.PercentComplete = (100 * formatSettings.NextResultIndex) / templateResults.Count;
                    }
                    else
                    {
                        progressProvider.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                emailProgress.Failed(ex);

                throw;
            }
            finally
            {
                // Has to be disposed on the UI thread
                if (htmlControl != null)
                {
                    htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));
                }
            }

            // Mark it as completed
            emailProgress.Completed();

            // Return true if we were not canceled
            return emailProgress.Status != ProgressItemStatus.Canceled;
        }

        /// <summary>
        /// Create and send an email message based on the results used and the given content
        /// </summary>
        private bool AddToOutbox(List<TemplateResult> resultsUsed, string htmlContent, string plainContent)
        {
            long? storeID = settingsResolver.DetermineStore(template, resultsUsed);
            
            // Null return means it was canceled 
            if (storeID == null)
            {
                return false;
            }

            // Create the header and populate it based on our context
            EmailTemplateMessageHeader header = new EmailTemplateMessageHeader(template, resultsUsed, storeID.Value);

            // Determine what the delayed send date should be
            DateTime? delayUntilDate = null;
            if (ProvideSendDateDelay != null)
            {
                EmailSendDateDelayProviderEventArgs delayArgs = new EmailSendDateDelayProviderEventArgs(resultsUsed.Select(r => r.XPathSource.Input).ToList());
                ProvideSendDateDelay(this, delayArgs);

                delayUntilDate = delayArgs.DelayUntilDate;

                if (delayUntilDate != null)
                {
                    log.InfoFormat("Email delayed until {0}", delayUntilDate);
                }
            }

            try
            {
                // Add the message to the outbox
                EmailOutboundEntity emailOutbound = EmailOutbox.AddMessage(header, htmlContent, plainContent, delayUntilDate);

                // Add this to the list we have generated
                emailsGenerated.Add(emailOutbound);
            }
            catch (PermissionException)
            {
                securityDenials++;
            }

            return true;
        }

        /// <summary>
        /// The settings resolver calls this when it needs to know what settings to use to send the email
        /// </summary>
        void OnResolveSettings(object sender, EmailSettingsResolveEventArgs e)
        {
            if (ResolveSettings != null)
            {
                ResolveSettings(this, e);
            }
            else
            {
                if (e.Customer != null)
                {
                    throw new EmailException("A customer has orders from more than one store, and ShipWorks could not determine which template email settings to use.", EmailExceptionErrorNumber.IndeterminateTemplateSettings);
                }
                else
                {
                    throw new EmailException("The input has orders from more than one store, and ShipWorks could not determine which template email settings to use.", EmailExceptionErrorNumber.IndeterminateTemplateSettings);
                }
            }
        }
    }
}
