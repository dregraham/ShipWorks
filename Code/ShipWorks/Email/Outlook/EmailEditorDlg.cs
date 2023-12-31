﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Email.Accounts;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// Window for editing emails that are in the outbox
    /// </summary>
    public partial class EmailEditorDlg : Form
    {
        long emailID;
        EmailOutboundEntity email;
        SqlEntityLock messageLock;
        static readonly ILog log = LogManager.GetLogger(typeof(EmailEditorDlg));

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailEditorDlg(long emailID)
        {
            InitializeComponent();

            this.emailID = emailID;

            WindowStateSaver.Manage(this);

            UserSession.Security.DemandPermission(PermissionType.RelatedObjectSendEmail, emailID);
        }

        /// <summary>
        /// Detect any errors early and prevent the window from showing if there are any.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            try
            {
                messageLock = new SqlEntityLock(emailID, "Edit Email");

                email = (EmailOutboundEntity) DataProvider.GetEntity(emailID);
                if (email == null)
                {
                    MessageHelper.ShowInformation(this, "The selected message has been deleted.");
                    DialogResult = DialogResult.Cancel;
                }

                else if (email.SendStatus == (int) EmailOutboundStatus.Sent)
                {
                    MessageHelper.ShowInformation(this, "The messages has already been sent.");
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(this, "Another ShipWorks computer is currently editing or sending the message.");
                DialogResult = DialogResult.Cancel;
            }

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            emailAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);

            EmailUtility.LoadEmailAccounts(emailAccount);

            headerGroup.ValuesPrimary.Heading = email.Subject;

            to.Text = email.ToList;
            cc.Text = email.CcList;
            bcc.Text = email.BccList;
            subject.Text = email.Subject;

            emailAccount.SelectedValue = email.AccountID;
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
        /// The window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (email.HtmlPartResourceID != null)
            {
                // Make sure all our image resources are loaded
                DataResourceManager.LoadConsumerResourceReferences(emailID);

                // We need to get our base href added for displaying images
                DataResourceReference htmlResource = DataResourceManager.LoadResourceReference(email.HtmlPartResourceID.Value);

                if (htmlResource != null)
                {
                    TemplateHtmlDocument htmlDocument = new TemplateHtmlDocument(htmlResource.ReadAllText(), TemplateResultUsage.ShipWorksDisplay);
                    htmlEditor.Html = htmlDocument.CompleteHtml;   
                }
                else
                {
                    // According to the resource manager, this should only fail if the resource was
                    // manually deleted, but even in that case, we can handle it more gracefully than a crash.
                    log.ErrorFormat("An error has occurred loading EmailOutboundEntity with ID of {0}. Could not get Resource for HtmlPartResourceID {1}.", emailID, email.HtmlPartResourceID);
                    MessageHelper.ShowError(this, "ShipWorks could not load the contents of this email.");
                }
            }
            else
            {
                DataResourceReference textResource = DataResourceManager.LoadResourceReference(email.PlainPartResourceID);

                if (textResource != null)
                {
                    htmlEditor.Html = TemplateResultFormatter.EncodeForHtml(textResource.ReadAllText(), TemplateOutputFormat.Text);
                }
                else
                {
                    // According to the resource manager, this should only fail if the resource was
                    // manually deleted, but even in that case, we can handle it more gracefully than a crash.
                    log.ErrorFormat("An error has occurred loading EmailOutboundEntity with ID of {0}. Could not get Resource for PlainPartResourceID {1}.", emailID, email.PlainPartResourceID);
                    MessageHelper.ShowError(this, "ShipWorks could not load the contents of this email.");
                }
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (messageLock != null)
            {
                messageLock.Dispose();
                messageLock = null;
            }
        }

        /// <summary>
        /// User accepting saving the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                EmailUtility.ValidateAddresses(to.Text, cc.Text, bcc.Text);

                if (emailAccount.SelectedIndex >= 0)
                {
                    email.AccountID = (long) emailAccount.SelectedValue;
                }

                email.ToList = to.Text;
                email.CcList = cc.Text;
                email.BccList = bcc.Text;
                email.Subject = subject.Text;

                email.SendStatus = (int)EmailOutboundStatus.Ready;

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    EmailOutbox.UpdateMessage(email, htmlEditor.Html, null, adapter);
                    adapter.SaveAndRefetch(email);

                    adapter.Commit();
                }

                DialogResult = DialogResult.OK;
            }
            catch (EmailException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
