using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Email.Accounts;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    /// <summary>
    /// Control for editing settings when importing flat files from email
    /// </summary>
    public partial class GenericFileSourceEmailControl : GenericFileSourceSettingsControlBase
    {
        GenericFileStoreEntity store = null;
        EmailAccountEntity emailAccount = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceEmailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the given store into the control
        /// </summary>
        public override void LoadStore(GenericFileStoreEntity store)
        {
            actionsControl.Initialize(

                // Success options
                new object[]
                {
                     new { Value = GenericFileSuccessAction.Move, Display = "Move the message" },
                     new { Value = GenericFileSuccessAction.MarkAsRead, Display = "Mark the message as read" },
                     new { Value = GenericFileSuccessAction.Delete, Display = "Delete the message" }
                },

                // Error options
                new object[]
                 {
                     new { Value = GenericFileErrorAction.Stop, Display = "Stop importing and display the error" },
                     new { Value = GenericFileErrorAction.Move, Display = "Move the message and continue importing" },
                     new { Value = GenericFileErrorAction.MarkAsRead, Display = "Mark as read and continue importing" }
                 });

            this.store = store;

            if (store.EmailAccountID != null)
            {
                emailAccount = EmailAccountManager.GetAccount(store.EmailAccountID.Value);
            }

            UpdateAccountUI();

            incomingFolder.Text = store.EmailIncomingFolder;

            onlyImportUnread.Checked = store.EmailOnlyUnread;

            // The following settings are shared in the database, but only make sense to load if we are the source they are for - otherwise
            // we'll just let them start off as the defauts.
            if (store.FileSource == (int) GenericFileSourceTypeCode.Email)
            {
                subjectMustMatch.Checked = store.NamePatternMatch != null;
                subjectMustMatchPattern.Text = store.NamePatternMatch ?? "";

                subjectCantMatch.Checked = store.NamePatternSkip != null;
                subjectCantMatchPattern.Text = store.NamePatternSkip ?? "";

                // Actions
                actionsControl.LoadStore(store);
            }
        }

        /// <summary>
        /// Save the settings from the control into the store
        /// </summary>
        public override bool SaveToEntity(GenericFileStoreEntity store)
        {
            if (store.EmailAccountID == null)
            {
                MessageHelper.ShowInformation(this, "Please setup the incoming email account that will be monitored for files to import.");
                return false;
            }

            if (string.IsNullOrEmpty(incomingFolder.Text))
            {
                MessageHelper.ShowInformation(this, "Please select the email folder to monitor for incoming message.");
                return false;
            }

            store.FileSource = (int) GenericFileSourceTypeCode.Email;

            // If the incoming folder is changing we have to reset our ID tracking to start over from the beginning of this new folder.
            if (string.Compare(store.EmailIncomingFolder, incomingFolder.Text, StringComparison.OrdinalIgnoreCase) != 0)
            {
                store.EmailFolderValidityID = 0;
                store.EmailFolderLastMessageID = 0;
            }

            store.EmailIncomingFolder = incomingFolder.Text;

            store.NamePatternMatch = subjectMustMatch.Checked ? subjectMustMatchPattern.Text : null;
            store.NamePatternSkip = subjectCantMatch.Checked ? subjectCantMatchPattern.Text : null;

            store.EmailOnlyUnread = onlyImportUnread.Checked;

            return actionsControl.SaveToEntity(store, store.EmailIncomingFolder);
        }
        /// <summary>
        /// Changing the must-match check box state
        /// </summary>
        private void OnCheckedChangedMustMatch(object sender, EventArgs e)
        {
            subjectMustMatchPattern.Enabled = subjectMustMatch.Checked;
        }

        /// <summary>
        /// Changing the cant-match check box state
        /// </summary>
        private void OnCheckedChangedCantMatch(object sender, EventArgs e)
        {
            subjectCantMatchPattern.Enabled = subjectCantMatch.Checked;
        }

        /// <summary>
        /// Create a new email account to be attached to the store
        /// </summary>
        private void OnNewAccount(object sender, EventArgs e)
        {
            if (GenericFileSourceEmailUtility.SetupNewAccount(store, this))
            {
                EmailAccountManager.CheckForChangesNeeded();
                emailAccount = EmailAccountManager.GetAccount(store.EmailAccountID.Value);

                UpdateAccountUI();
            }
        }
        /// <summary>
        /// Edit the existing email account for the store
        /// </summary>
        private void OnEditAccount(object sender, EventArgs e)
        {
            // If there is no account, this button is acting as the "new" button
            if (emailAccount == null)
            {
                OnNewAccount(sender, e);
            }
            else
            {
                using (EmailAccountEditorDlg dlg = new EmailAccountEditorDlg(emailAccount))
                {
                    dlg.ShowDialog(this);

                    UpdateAccountUI();
                }
            }
        }

        /// <summary>
        /// Update the description text and UI stuff of the email account
        /// </summary>
        private void UpdateAccountUI()
        {
            if (emailAccount == null)
            {
                accountNew.Visible = false;
                accountEdit.Text = "Setup...";

                emailAccountDescription.Text = "";

                incomingFolderBrowse.Enabled = false;
                actionsControl.Enabled = false;
            }
            else
            {
                accountNew.Visible = true;
                accountEdit.Text = "Edit...";

                emailAccountDescription.Text = emailAccount.EmailAddress;

                incomingFolderBrowse.Enabled = true;
                actionsControl.Enabled = true;
            }
        }

        /// <summary>
        /// Browse for the incoming IMAP folder
        /// </summary>
        private void OnBrowseIncomingFolder(object sender, EventArgs e)
        {
            using (EmailImapFolderBrowserDlg dlg = new EmailImapFolderBrowserDlg(emailAccount))
            {
                dlg.SelectedFolder = incomingFolder.Text;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    incomingFolder.Text = dlg.SelectedFolder;
                }
            }
        }

        /// <summary>
        /// Browse for the folder to move to after succesful or error processing
        /// </summary>
        private void OnBrowseForActionFolder(object sender, GenericFileSourceFolderBrowseEventArgs e)
        {
            using (EmailImapFolderBrowserDlg dlg = new EmailImapFolderBrowserDlg(emailAccount))
            {
                dlg.SelectedFolder = e.Folder;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    e.Folder = dlg.SelectedFolder;
                }
            }
        }

        /// <summary>
        /// Adjust our own height to be big enough to hold the actions control size
        /// </summary>
        private void OnActionsSizeChanged(object sender, EventArgs e)
        {
            Height = actionsControl.Bottom;
        }
    }
}
