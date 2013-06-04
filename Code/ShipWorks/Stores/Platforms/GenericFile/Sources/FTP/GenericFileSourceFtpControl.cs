using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.FTP
{
    /// <summary>
    /// Control for editing settings of importing flat files from FTP
    /// </summary>
    public partial class GenericFileSourceFtpControl : GenericFileSourceSettingsControlBase
    {
        GenericFileStoreEntity store = null;
        FtpAccountEntity ftpAccount = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceFtpControl()
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
                     new { Value = GenericFileSuccessAction.Move, Display = "Move the file" },
                     new { Value = GenericFileSuccessAction.Delete, Display = "Delete the file" }
                },

                // Error options
                new object[]
                 {
                     new { Value = GenericFileErrorAction.Stop, Display = "Stop importing and display the error" },
                     new { Value = GenericFileErrorAction.Move, Display = "Move the file and continue importing" }
                 });

            this.store = store;

            if (store.FtpAccountID != null)
            {
                ftpAccount = FtpAccountManager.GetAccount(store.FtpAccountID.Value);
            }

            UpdateAccountUI();

            ftpFolder.Text = store.FtpFolder;

            // The following settings are shared in the database, but only make sense to load if we are the source they are for - otherwise
            // we'll just let them start off as the defauts.
            if (store.FileSource == (int) GenericFileSourceTypeCode.FTP)
            {
                importMustMatch.Checked = store.NamePatternMatch != null;
                importMustMatchPattern.Text = store.NamePatternMatch ?? "";

                // Can't match
                importCantMatch.Checked = store.NamePatternSkip != null;
                importCantMatchPattern.Text = store.NamePatternSkip ?? "";

                // Actions
                actionsControl.LoadStore(store);
            }
        }

        /// <summary>
        /// Save the settings from the control into the store
        /// </summary>
        public override bool SaveToEntity(GenericFileStoreEntity store)
        {
            if (store.FtpAccountID == null)
            {
                MessageHelper.ShowInformation(this, "Please configure the FTP account that will be monitored for files.");
                return false;
            }

            if (!actionsControl.CheckValidFolder(ftpFolder.Text, "import folder"))
            {
                return false;
            }

            store.FileSource = (int) GenericFileSourceTypeCode.FTP;

            store.FtpFolder = ftpFolder.Text;

            store.NamePatternMatch = importMustMatch.Checked ? importMustMatchPattern.Text : null;
            store.NamePatternSkip = importCantMatch.Checked ? importCantMatchPattern.Text : null;

            return actionsControl.SaveToEntity(store, store.FtpFolder);
        }

        /// <summary>
        /// Configure the FTP account connection
        /// </summary>
        private void OnConfigureFtp(object sender, EventArgs e)
        {
            // If there is no account, this button is acting as the "new" button
            if (ftpAccount == null)
            {
                using (AddFtpAccountWizard wizard = new AddFtpAccountWizard(true))
                {
                    if (wizard.ShowDialog(this) == DialogResult.OK)
                    {
                        FtpAccountManager.CheckForChangesNeeded();
                        ftpAccount = wizard.FtpAccount;
                        ftpFolder.Text = wizard.InitialFolder;

                        // Update the store to use this new account
                        store.FtpAccountID = ftpAccount.FtpAccountID;

                        // We own this account
                        ftpAccount.InternalOwnerID = store.StoreID;
                        SqlAdapter.Default.SaveAndRefetch(ftpAccount);

                        UpdateAccountUI();
                    }
                }
            }
            else
            {
                using (FtpAccountEditorDlg dlg = new FtpAccountEditorDlg(ftpAccount))
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
            if (ftpAccount == null)
            {
                ftpHost.Text = "";

                browseIncoming.Enabled = false;
                actionsControl.Enabled = false;
            }
            else
            {
                ftpHost.Text = ftpAccount.Host;

                browseIncoming.Enabled = true;
                actionsControl.Enabled = true;
            }
        }

        /// <summary>
        /// Changing the must-match check box state
        /// </summary>
        private void OnCheckedChangedMustMatch(object sender, EventArgs e)
        {
            importMustMatchPattern.Enabled = importMustMatch.Checked;
        }

        /// <summary>
        /// Changing the cant-match check box state
        /// </summary>
        private void OnCheckedChangedCantMatch(object sender, EventArgs e)
        {
            importCantMatchPattern.Enabled = importCantMatch.Checked;
        }

        /// <summary>
        /// Browse for the folder to import from
        /// </summary>
        private void OnBrowseImportFolder(object sender, EventArgs e)
        {
            using (FtpFolderBrowserDlg dlg = new FtpFolderBrowserDlg(ftpAccount, ftpFolder.Text))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ftpFolder.Text = dlg.SelectedFolder;
                }
            }
        }

        /// <summary>
        /// Browse for the folder to move to after succesful or error processing
        /// </summary>
        private void OnBrowseForActionFolder(object sender, GenericFileSourceFolderBrowseEventArgs e)
        {
            using (FtpFolderBrowserDlg dlg = new FtpFolderBrowserDlg(ftpAccount, e.Folder))
            {
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
