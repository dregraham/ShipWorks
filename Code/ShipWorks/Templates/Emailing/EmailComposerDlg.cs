using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using System.Collections;
using ShipWorks.Templates.Processing;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.Utility;
using Divelements.SandGrid;
using System.Net.Mail;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI.Controls.Html;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Properties;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMainMenuItem = Divelements.SandRibbon.MainMenuItem;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Window for composing a new email message
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class EmailComposerDlg : Form
    {
        TemplateEntity initialTemplate;
        TemplateEntity lastTemplate;

        // The keys from which content is being generated
        List<long> keys;

        // The template results for the current template selection
        IList<TemplateResult> templateResults;

        // We need to know what the last active row was when the selection changes
        GridRow activeRow;

        // The type of entity of the key collection passed in
        EntityType targetEntityType;

        // The emails added successfully to the outbox
        List<EmailOutboundEntity> emailsGenerated = new List<EmailOutboundEntity>();

        #region class MesssageDraft

        enum MessageDraftState
        {
            Unsent,
            Success,
            Error,
            Cancel
        }

        class MessageDraft : EmailTemplateMessageHeader
        {
            public MessageDraft(TemplateEntity template, IList<TemplateResult> results, long storeID) :
                base(template, results, storeID)
            {

            }

            public string Body { get; set; }
            public MessageDraftState State { get; set; }
            public string ErrorMessage { get; set; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailComposerDlg(TemplateEntity template, IEnumerable<long> keys)
        {
            InitializeComponent();

            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }

            if (!keys.Any())
            {
                throw new ArgumentException("keys cannot be empty");
            }

            targetEntityType = EntityUtility.GetEntityType(keys.First());

            splitContainer.Panel1Collapsed = true;

            initialTemplate = template;

            // Make a copy of the list so changes don't affect us
            this.keys = keys.ToList();

            WindowStateSaver saver = new WindowStateSaver(this);
            saver.ManageSplitter(splitContainer);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            EmailUtility.LoadEmailAccounts(emailAccount);

            emailAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);
        }

        /// <summary>
        /// Window has become visible
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            templateList.LoadTemplates();
            templateList.SelectedTemplate = initialTemplate;
        }

        /// <summary>
        /// The messages that were successfully added to the outbox
        /// </summary>
        public List<EmailOutboundEntity> EmailsGenerated
        {
            get
            {
                return emailsGenerated;
            }
        }

        /// <summary>
        /// Open the window for managing email accounts
        /// </summary>
        private void OnManageEmailAccounts(object sender, EventArgs e)
        {
            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);

                EmailUtility.LoadEmailAccounts(emailAccount);
            }
        }

        /// <summary>
        /// The template used for email generation has changed
        /// </summary>
        private void OnChangeTemplate(object sender, EventArgs e)
        {
            // Have to let the popup have time to close
            BeginInvoke(new MethodInvoker(BackgroundPrepareMessagesInitiate));
        }

        /// <summary>
        /// Initiate the preparation of background messages.  This function is called in the UI thread.
        /// </summary>
        private void BackgroundPrepareMessagesInitiate()
        {
            // Create the progress for template processing
            ProgressItem progressTemplate = new ProgressItem("Preparing");
            progressTemplate.Starting();

            // Create the progress for draft generation
            ProgressItem progressDrafts = new ProgressItem("Generating");

            ProgressProvider progressProvider = new ProgressProvider();
            progressProvider.ProgressItems.Add(progressTemplate);
            progressProvider.ProgressItems.Add(progressDrafts);

            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Generate Email";
            progressDlg.Description = "ShipWorks is generating messages.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(BackgroundPrepareMessages, "preparing email"), new object[] { templateList.SelectedTemplate, delayer });
        }

        /// <summary>
        /// Prepare all the message content in the background
        /// </summary>
        private void BackgroundPrepareMessages(object state)
        {
            object[] data = (object[]) state;
            TemplateEntity template = (TemplateEntity) data[0];
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) data[1];
            ProgressItem progressItem = delayer.ProgressDlg.ProgressProvider.ProgressItems[0];

            Exception error = null;

            if (template.Type == (int) TemplateType.Thermal)
            {
                error = new TemplateException(
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be emailed.");
            }
            else
            {
                try
                {
                    templateResults = TemplateProcessor.ProcessTemplate(template, keys, progressItem);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }

            // If there was an error (or cancel would qualify) end now
            if (error != null)
            {
                BeginInvoke(new MethodInvoker<ProgressDisplayDelayer, Exception>(BackgroundPrepareMessagesFailed), delayer, error);
            }
            else
            {
                progressItem.Completed();

                CreateMessageDrafts(delayer, delayer.ProgressDlg.ProgressProvider.ProgressItems[1]);
            }
        }

        /// <summary>
        /// Create a list of message drafts to be sent based on the current template result set
        /// </summary>
        private void CreateMessageDrafts(ProgressDisplayDelayer delayer, ProgressItem progress)
        {
            IEnumerable<List<TemplateResult>> input;
            List<MessageDraft> output = new List<MessageDraft>();

            // Label templates result in a single sheet
            if (templateList.SelectedTemplate.Type == (int) TemplateType.Label)
            {
                if (templateResults.Count > 0)
                {
                    input = new List<TemplateResult>[] { (List<TemplateResult>) templateResults };
                }
                else
                {
                    input = new List<TemplateResult>[0];
                }
            }
            else
            {
                input = templateResults.Select(r => new List<TemplateResult> { r });
            }

            EmailSettingsResolver settingsResolver = new EmailSettingsResolver();
            settingsResolver.Tag = delayer;
            settingsResolver.ResolveSettings += new EmailSettingsResolveEventHandler(OnResolveEmailSettings);

            int total = input.Count();

            progress.Starting();

            // Go through each item in the list
            foreach (List<TemplateResult> draftInput in input)
            {
                if (progress.IsCancelRequested)
                {
                    break;
                }

                progress.Detail = string.Format("{0} of {1}", output.Count + 1, total);

                // Execute the work
                MessageDraft draft = CreateMessageDraft(draftInput, settingsResolver);
                if (draft != null)
                {
                    output.Add(draft);

                    progress.PercentComplete = (100 * output.Count) / total;
                }
                else
                {
                    delayer.ProgressDlg.ProgressProvider.Cancel();
                }
            }

            // Mark the progress as complete
            progress.Completed();

            // Raise the complete event
            BeginInvoke(new MethodInvoker<List<MessageDraft>, ProgressDisplayDelayer, bool>(BackgroundCreateDraftsCompleted), 
                output,
                delayer, 
                progress.IsCancelRequested);
        }

        /// <summary>
        /// Create a new mail draft using the defaults of the given result
        /// </summary>
        private MessageDraft CreateMessageDraft(List<TemplateResult> results, EmailSettingsResolver settingsResolver)
        {
            TemplateEntity template = templateList.SelectedTemplate;

            long? storeID = settingsResolver.DetermineStore(template, results);

            // Null return means it was canceled 
            if (storeID == null)
            {
                return null;
            }

            // Create the new blank mail message and draft to hold it
            MessageDraft draft = new MessageDraft(template, results, storeID.Value);

            return draft;
        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        private void BackgroundCreateDraftsCompleted(List<MessageDraft> drafts, ProgressDisplayDelayer delayer, bool canceled)
        {
            delayer.NotifyComplete();

            if (canceled)
            {
                CancelPrepareMessages();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Set the current template as the last selected template
            lastTemplate = templateList.SelectedTemplate;

            ClearPreviousMessages();

            // Add each draft to our list
            foreach (MessageDraft draft in drafts)
            {
                AddMailDraft(draft);
            }

            // Update the count
            labelMessageCount.Text = string.Format("Messages: {0}", messageList.Rows.Count);

            // If there is only one result we don't need the side panel
            splitContainer.Panel1Collapsed = messageList.Rows.Count <= 1;

            // Select the first message
            if (messageList.Rows.Count > 0)
            {
                messageList.Rows[0].Selected = true;
            }
            else
            {
                composerHeaderGroup.Panel.Enabled = false;
                ClearComposer();

                MessageHelper.ShowWarning(this, TemplateHelper.NoResultsErrorMessage);
            }
        }

        /// <summary>
        /// The preparation of the template result message content has been canceled or failed
        /// </summary>
        private void BackgroundPrepareMessagesFailed(ProgressDisplayDelayer delayer, Exception error)
        {
            Debug.Assert(error != null);

            delayer.NotifyComplete();

            // Check for error conditions
            TemplateException templateEx = error as TemplateException;
            if (templateEx != null)
            {
                if (!(templateEx is TemplateCancelException))
                {
                    MessageHelper.ShowError(this, templateEx.Message);
                }

                CancelPrepareMessages();
            }
            else
            {
                throw new InvalidOperationException(error.Message, error);
            }
        }

        /// <summary>
        /// The process of preparing messages has been canceled
        /// </summary>
        private void CancelPrepareMessages()
        {
            // If there was a last template already selected, set it back
            if (lastTemplate != null)
            {
                // No need to trigger the event - the current data will already be what its supposed to be
                templateList.SelectedTemplateChanged -= this.OnChangeTemplate;
                templateList.SelectedTemplate = lastTemplate;
                templateList.SelectedTemplateChanged += this.OnChangeTemplate;
            }
            else
            {
                // If we hadn't gotten anywhere yet we just have to close
                Close();
            }
        }

        /// <summary>
        /// Resolve email settings in cases where multiple are possible
        /// </summary>
        void OnResolveEmailSettings(object sender, EmailSettingsResolveEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EmailSettingsResolveEventHandler(OnResolveEmailSettings), sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) ((EmailSettingsResolver) sender).Tag;

            delayer.EnsureShown();

            using (EmailSettingsResolveDlg dlg = new EmailSettingsResolveDlg(e))
            {
                dlg.ShowDialog(delayer.ProgressDlg);
            }
        }

        /// <summary>
        /// Create and add a new mail message result to the grid
        /// </summary>
        private void AddMailDraft(MessageDraft draft)
        {
            GridRow messageRow = new GridRow(new string[] { (messageList.Rows.Count + 1).ToString(), draft.Subject });
            messageRow.Tag = draft;

            // Add to the grid
            messageList.Rows.Add(messageRow);
        }

        /// <summary>
        /// Clear any messages that we had previously created
        /// </summary>
        private void ClearPreviousMessages()
        {
            activeRow = null;

            messageList.SelectionChanged -= OnSelectedMessageChanged;
            messageList.Rows.Clear();
            messageList.SelectionChanged += OnSelectedMessageChanged;
        }

        /// <summary>
        /// The selected email message has changed
        /// </summary>
        private void OnSelectedMessageChanged(object sender, SelectionChangedEventArgs e)
        {
            if (activeRow != null)
            {
                SaveMessage(activeRow);
            }

            if (messageList.SelectedElements.Count > 0)
            {
                activeRow = messageList.SelectedElements[0] as GridRow;

                LoadMessage(activeRow);
            }
            else
            {
                ClearComposer();
            }

            composerHeaderGroup.Panel.Enabled = messageList.SelectedElements.Count > 0;
        }

        /// <summary>
        /// Save the current message state to the given row
        /// </summary>
        private void SaveMessage(GridRow row)
        {
            MessageDraft draft = (MessageDraft) row.Tag;

            if (emailAccount.Items.Count > 0)
            {
                draft.EmailAccountID = (long) emailAccount.SelectedValue;
            }

            draft.To = toText.Text;
            draft.CC = ccText.Text;
            draft.BCC = bccText.Text;
            draft.Subject = subject.Text;
            draft.Body = htmlEditor.Html;
        }

        /// <summary>
        /// Load the message state from the message in the given row
        /// </summary>
        private void LoadMessage(GridRow row)
        {
            MessageDraft draft = (MessageDraft) row.Tag;

            composerHeaderGroup.ValuesPrimary.Heading = string.Format("Message {0}", row.Index + 1);

            emailAccount.SelectedValue = draft.EmailAccountID;

            // Select the default if none was selected
            if (emailAccount.SelectedIndex < 0 && emailAccount.Items.Count > 0)
            {
                emailAccount.SelectedIndex = 0;
            }

            toText.Text = draft.To;
            ccText.Text = draft.CC;
            bccText.Text = draft.BCC;
            subject.Text = draft.Subject;

            LoadEmailAddressSuggestions(draft.TemplateResults);

            // Update Icon and Text to reflect state
            if (draft.State == MessageDraftState.Success)
            {
                composerHeaderGroup.ValuesPrimary.Image = Resources.check16;
                composerHeaderGroup.ValuesPrimary.Description = "Sent to Outbox";
                composerHeaderGroup.StateNormal.HeaderPrimary.Content.LongText.Color1 = Color.Green;
            }
            else if (draft.State == MessageDraftState.Error)
            {
                composerHeaderGroup.ValuesPrimary.Image = Resources.error16;
                composerHeaderGroup.ValuesPrimary.Description = draft.ErrorMessage;
                composerHeaderGroup.StateNormal.HeaderPrimary.Content.LongText.Color1 = Color.Red;
            }
            else
            {
                composerHeaderGroup.ValuesPrimary.Image = null;
                composerHeaderGroup.ValuesPrimary.Description = "";
            }

            // If its sent, its uneditable
            panelBody.Enabled = (draft.State != MessageDraftState.Success);

            // See if we've generated the content yet
            if (draft.Body == null)
            {
                BackgroundLoadMessageBodyInitiate(draft);
            }
            else
            {
                htmlEditor.Html = draft.Body;
            }
        }

        /// <summary>
        /// Load suggestinos for the email button drop-downs
        /// </summary>
        private void LoadEmailAddressSuggestions(IList<TemplateResult> results)
        {
            // So basically for label or report templates, where more than one input was used to generate it, we
            // can't offer any good suggestions.
            if (results.Count != 1 || results[0].XPathSource.Input.ContextKeys.Count != 1)
            {
                ClearEmailAddresSuggestions(toButton);
                ClearEmailAddresSuggestions(ccButton);
                ClearEmailAddresSuggestions(bccButton);

                return;
            }

            TemplateResult result = results[0];
            TemplateInput input = result.XPathSource.Input;

            // This will get the order or customer record, depending on the target entity type
            EntityBase2 entity = DataProvider.GetRelatedEntities(input.ContextKeys[0], targetEntityType)[0];
            PersonAdapter billInfo = new PersonAdapter(entity, "Bill");
            PersonAdapter shipInfo = new PersonAdapter(entity, "Ship");

            LoadEmailAddressSuggestions(toButton, toText, shipInfo, billInfo);
            LoadEmailAddressSuggestions(ccButton, ccText, shipInfo, billInfo);
            LoadEmailAddressSuggestions(bccButton, bccText, shipInfo, billInfo);
        }

        /// <summary>
        /// Clear the suggestion list for the given button
        /// </summary>
        private void ClearEmailAddresSuggestions(DropDownButton button)
        {
            SandContextPopup popup = new SandContextPopup();
            popup.Font = new Font(Font, FontStyle.Regular);
            SandMenu menu = new SandMenu();
            popup.Items.Add(menu);

            SandMenuItem item = new SandMenuItem("No Suggestions");
            item.Enabled = false;
            menu.Items.Add(item);

            button.SplitSandPopupMenu = popup;
        }

        /// <summary>
        /// Load the address selections into the given button
        /// </summary>
        private void LoadEmailAddressSuggestions(DropDownButton button, TextBox target, PersonAdapter shipInfo, PersonAdapter billInfo)
        {
            SandContextPopup popup = new SandContextPopup();
            popup.Font = new Font(Font, FontStyle.Regular);

            SandMenu menu = new SandMenu();
            popup.Items.Add(menu);

            SandMainMenuItem shipItem = new SandMainMenuItem();
            shipItem.Text = "Shipping Email";
            shipItem.Description = string.Format("\"{0} {1}\" <{2}>", shipInfo.FirstName, shipInfo.LastName, shipInfo.Email);
            shipItem.Tag = target;
            shipItem.Activate += OnAddSuggestedEmail;
            menu.Items.Add(shipItem);

            SandMainMenuItem billItem = new SandMainMenuItem();
            billItem.GroupName = "NewGroup";
            billItem.Text = "Billing Email";
            billItem.Description = string.Format("\"{0} {1}\" <{2}>", billInfo.FirstName, billInfo.LastName, billInfo.Email);
            billItem.Tag = target;
            billItem.Activate += OnAddSuggestedEmail;
            menu.Items.Add(billItem);

            button.SplitSandPopupMenu = popup;
        }

        /// <summary>
        /// Add an email suggestion
        /// </summary>
        private void OnAddSuggestedEmail(object sender, EventArgs e)
        {
            SandMainMenuItem menuItem = (SandMainMenuItem) sender;
            TextBox targetBox = (TextBox) menuItem.Tag;

            if (targetBox.Text.Trim().Length != 0)
            {
                targetBox.Text += ", ";
            }

            targetBox.Text += menuItem.Description;
        }

        /// <summary>
        /// Clear the composer of all its content
        /// </summary>
        private void ClearComposer()
        {
            toText.Text = "";
            ccText.Text = "";
            bccText.Text = "";
            subject.Text = "";

            composerHeaderGroup.ValuesPrimary.Image = null;
            composerHeaderGroup.ValuesPrimary.Description = "";

            htmlEditor.Html = "";
        }

        /// <summary>
        /// Prepare and load the message body in the background
        /// </summary>
        private void BackgroundLoadMessageBodyInitiate(MessageDraft draft)
        {
            ProgressItem progressItem = new ProgressItem("Loading");
            progressItem.Detail = "Loading body content...";
            progressItem.CanCancel = false;
            progressItem.Starting();

            ProgressProvider provider = new ProgressProvider();
            provider.ProgressItems.Add(progressItem);

            ProgressDlg progressDlg = new ProgressDlg(provider);
            progressDlg.Title = "Loading Body";
            progressDlg.Description = "ShipWorks is loading the message body.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(BackgroundLoadMessageBody, "loading email"), new object[] { draft, delayer });
        }

        /// <summary>
        /// Load the body content of the draft 
        /// </summary>
        private void BackgroundLoadMessageBody(object state)
        {
            object[] data = (object[]) state;
            MessageDraft draft = (MessageDraft) data[0];
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) data[1];

            Exception error = null;

            try
            {
                LoadMessageBody(draft);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            BeginInvoke(new MethodInvoker<MessageDraft, ProgressDisplayDelayer, Exception>(BackgroundLoadMessageBodyCompleted), draft, delayer, error);
        }

        /// <summary>
        /// The backgorund loading of the message body has completed
        /// </summary>
        private void BackgroundLoadMessageBodyCompleted(MessageDraft draft, ProgressDisplayDelayer delayer, Exception error)
        {
            delayer.NotifyComplete();

            // Check for error conditions
            TemplateException templateEx = error as TemplateException;
            if (templateEx != null)
            {
                MessageHelper.ShowError(this, templateEx.Message);

                htmlEditor.Html = TemplateResultFormatter.FormatError(templateEx, TemplateOutputFormat.Html);
            }
            else if (error != null)
            {
                throw new InvalidOperationException(error.Message, error);
            }
            else
            {
                htmlEditor.Html = draft.Body;
            }
        }

        /// <summary>
        /// Load the body content for the given draft
        /// </summary>
        private void LoadMessageBody(MessageDraft draft)
        {
            if (!InvokeRequired)
            {
                throw new InvalidOperationException("This can only be called on a non-GUI thread.");
            }

            HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(this);

            try
            {
                htmlControl.Html = TemplateResultFormatter.FormatHtml(
                    draft.TemplateResults,
                    TemplateResultUsage.Email,
                    TemplateResultFormatSettings.FromTemplate(templateList.SelectedTemplate));

                htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                // Process sure size now that its ready
                TemplateSureSizeProcessor.Process(htmlControl);

                draft.Body = htmlControl.Html;
            }
            finally
            {
                htmlControl.BeginInvoke(new MethodInvoker(htmlControl.Dispose));
            }
        }

        /// <summary>
        /// The subject text is changing
        /// </summary>
        private void OnSubjectTextChanged(object sender, EventArgs e)
        {
            if (activeRow != null)
            {
                activeRow.Cells[1].Text = subject.Text;
            }
        }

        /// <summary>
        /// Send the email messages
        /// </summary>
        private void OnSend(object sender, EventArgs e)
        {
            // First check that there are any email accounts to send with
            if (EmailAccountManager.EmailAccounts.Count == 0)
            {
                if (UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts))
                {
                    if (MessageHelper.ShowQuestion(this,
                        "There are no email accounts configured for sending email.\n\n" +
                        "Setup an account now?") == DialogResult.OK)
                    {
                        using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
                        {
                            dlg.ShowDialog(this);
                        }
                    }
                }
                else
                {
                    MessageHelper.ShowError(this,
                        "There are no email accounts configured.  An administrator must log on to setup email accounts.");
                }

                return;
            }

            // Save any current edits
            if (activeRow != null)
            {
                SaveMessage(activeRow);
            }

            ProgressItem progress = new ProgressItem("Outbox");
            progress.Detail = "Adding messages to outbox...";
            progress.Starting();

            ProgressProvider provider = new ProgressProvider();
            provider.ProgressItems.Add(progress);

            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(provider);
            progressDlg.Title = "Emailing";
            progressDlg.Description = "ShipWorks is emailing...";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(BackgroundAddToOutbox, "preparing email"), delayer);
        }

        /// <summary>
        /// Background thread for adding all the messages to the outbox
        /// </summary>
        [NDependIgnoreLongMethod]
        private void BackgroundAddToOutbox(object state)
        {
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) state;
            ProgressItem progress = delayer.ProgressDlg.ProgressProvider.ProgressItems[0];

            // Get all the rows that have not yet been successfully sent
            List<GridRow> unsentRows = messageList.Rows.OfType<GridRow>().Where(r => ((MessageDraft) r.Tag).State != MessageDraftState.Success).ToList();

            foreach (GridRow usentRow in unsentRows)
            {
                MessageDraft draft = usentRow.Tag as MessageDraft;

                // If cancelled then don't try to send
                if (progress.IsCancelRequested)
                {
                    draft.State = MessageDraftState.Cancel;
                    Invoke((MethodInvoker) delegate { usentRow.Cells[0].Image = Resources.cancel16; });
                }
                else
                {
                    try
                    {
                        EmailUtility.ValidateAddresses(draft.To, draft.CC, draft.BCC);

                        // Load the body if its not already
                        if (draft.Body == null)
                        {
                            LoadMessageBody(draft);
                        }

                        EmailOutboundEntity emailOutbound = EmailOutbox.AddMessage(draft, draft.Body, null);

                        emailsGenerated.Add(emailOutbound);

                        // Mark as success
                        draft.State = MessageDraftState.Success;
                        Invoke((MethodInvoker) delegate { usentRow.Cells[0].Image = Resources.check16; });

                        // As soon as there is one success, we can't allow changing the template
                        Invoke((MethodInvoker) delegate { templateList.Enabled = false; });
                    }
                    catch (PermissionException)
                    {
                        // Mark as error
                        draft.State = MessageDraftState.Error;
                        draft.ErrorMessage = "You do not have permission to send this message.";
                        Invoke((MethodInvoker) delegate { usentRow.Cells[0].Image = Resources.error16; });
                    }
                    catch (EmailException ex)
                    {
                        // Mark as error
                        draft.State = MessageDraftState.Error;
                        draft.ErrorMessage = ex.Message;
                        Invoke((MethodInvoker) delegate { usentRow.Cells[0].Image = Resources.error16; });
                    }
                    catch (TemplateException ex)
                    {
                        // Mark as error
                        draft.State = MessageDraftState.Error;
                        draft.ErrorMessage = ex.Message;
                        Invoke((MethodInvoker) delegate { usentRow.Cells[0].Image = Resources.error16; });
                    }

                    // Update how much we are done
                    progress.PercentComplete = (100 * (unsentRows.IndexOf(usentRow) + 1)) / unsentRows.Count;
                }
            }

            BeginInvoke(new MethodInvoker<ProgressDisplayDelayer, Exception>(BackgroundAddToOutboxCompleted), delayer, null);
        }

        /// <summary>
        /// The sending of emails in the background has completed
        /// </summary>
        private void BackgroundAddToOutboxCompleted(ProgressDisplayDelayer delayer, Exception error)
        {
            delayer.NotifyComplete();

            // Get all the rows that have not yet been successfully sent
            List<GridRow> unsentRows = messageList.Rows.OfType<GridRow>().Where(r => ((MessageDraft) r.Tag).State != MessageDraftState.Success).ToList();

            // If none, we sent them all, so close
            if (unsentRows.Count == 0)
            {
                Close();
            }
            else
            {
                // See if there are any errors
                List<GridRow> errors = unsentRows.Where(r => ((MessageDraft) r.Tag).State == MessageDraftState.Error).ToList();

                GridRow rowToSelect;

                // If there are any, display that fact
                if (errors.Count == 1)
                {
                    MessageHelper.ShowError(this, ((MessageDraft) errors[0].Tag).ErrorMessage);
                    rowToSelect = errors[0];
                }
                else if (errors.Count > 1)
                {
                    MessageHelper.ShowError(this, "An error occurred while sending some of the messages.\n\nSelect each message to view detailed error information.");
                    rowToSelect = errors[0];
                }
                else
                {
                    // Reload the UI in case the state of the selected row changed
                    rowToSelect = activeRow;
                }

                // Select and load the content of the row that needs selected
                if (rowToSelect.Selected)
                {
                    LoadMessage(rowToSelect);
                }
                else
                {
                    rowToSelect.Selected = true;
                }
            }

        }
    }
}
