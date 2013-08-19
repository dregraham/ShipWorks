using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class FtpFileTaskEditor : TemplateBasedTaskEditor
    {
        private readonly FtpFileTask task;
        FtpAccountEntity ftpAccount = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileTaskEditor"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public FtpFileTaskEditor(FtpFileTask task) : base(task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            InitializeComponent();

            tokenizedFtpFolder.TextChanged += OnTokenizedFtpFolderTextChanged;
            tokenizedFtpFilename.TextChanged += OnTokenizedFtpFilenameTextChanged;

            this.task = task;

            LoadSettings();
        }

        /// <summary>
        /// Load the task settings
        /// </summary>
        public void LoadSettings()
        {
            if (task.FtpAccountID != null) 
            {
                ftpAccount = FtpAccountManager.GetAccount(task.FtpAccountID.Value);
            }

            UpdateAccountUI();

            tokenizedFtpFolder.Text = task.FtpFolder;
            tokenizedFtpFilename.Text = task.FtpFileName;
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
                        tokenizedFtpFolder.Text = wizard.InitialFolder;
                        
                        // Update the store to use this new account
                        task.FtpAccountID = ftpAccount.FtpAccountID;

                        // We own this account
                        ftpAccount.InternalOwnerID = null;
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
        /// Browse the FTP server
        /// </summary>
        private void OnBrowseFtp(object sender, EventArgs e)
        {
            if (ftpAccount != null)
            {
                using (FtpFolderBrowserDlg ftpFolderBrowserDlg = new FtpFolderBrowserDlg(ftpAccount, ""))
                {
                    if (ftpFolderBrowserDlg.ShowDialog(this) == DialogResult.OK)
                    {
                        tokenizedFtpFolder.Text = ftpFolderBrowserDlg.SelectedFolder;
                    }
                }
            }
        }

        /// <summary>
        /// Update the description text and UI stuff of the ftp account
        /// </summary>
        private void UpdateAccountUI()
        {
            if (ftpAccount == null)
            {
                ftpHost.Text = "";
            }
            else
            {
                ftpHost.Text = ftpAccount.Host;
            }
        }

        /// <summary>
        /// Called when tokenizedFtpFolder text changed to update the FtpFolder property of the task.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTokenizedFtpFolderTextChanged(object sender, EventArgs e)
        {
            task.FtpFolder = tokenizedFtpFolder.Text;
        }

        /// <summary>
        /// Called when tokenizedFtpFilename text changed to update the FtpFileName property of the task.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTokenizedFtpFilenameTextChanged(object sender, EventArgs e)
        {
            task.FtpFileName = tokenizedFtpFilename.Text;
        }
    }
}
