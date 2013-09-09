using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Task editor for Upload using FTP task
    /// </summary>
    public partial class FtpFileTaskEditor : TemplateBasedTaskEditor
    {
        private readonly FtpFileTask task;
        FtpAccountEntity ftpAccount;

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
                using (FtpFolderBrowserDlg ftpFolderBrowserDlg = new FtpFolderBrowserDlg(ftpAccount, tokenizedFtpFolder.Text))
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
                browseFtpFolder.Enabled = false;
            }
            else
            {
                ftpHost.Text = ftpAccount.Host;
                browseFtpFolder.Enabled = true;
            }
        }

        /// <summary>
        /// Called when tokenizedFtpFolder text changed to update the FtpFolder property of the task.
        /// </summary>
        private void OnTokenizedFtpFolderTextChanged(object sender, EventArgs e)
        {
            task.FtpFolder = tokenizedFtpFolder.Text;
        }

        /// <summary>
        /// Called when tokenizedFtpFilename text changed to update the FtpFileName property of the task.
        /// </summary>
        private void OnTokenizedFtpFilenameTextChanged(object sender, EventArgs e)
        {
            task.FtpFileName = tokenizedFtpFilename.Text;
        }

        /// <summary>
        /// Performs validation outside of the Windows Forms flow to make dealing with navigation easier
        /// </summary>
        /// <param name="errors">Collection of errors to which new errors will be added</param>
        public override void ValidateTask(ICollection<TaskValidationError> errors)
        {
            TaskValidationError error = new TaskValidationError("The 'Upload with FTP' task is missing some information.");

            if (templateCombo.SelectedTemplate == null)
            {
                error.Details.Add("A template must be selected.");
            }

            if (string.IsNullOrWhiteSpace(ftpHost.Text))
            {
                error.Details.Add("An FTP server was not provided.");
            }

            if (string.IsNullOrWhiteSpace(tokenizedFtpFolder.Text))
            {
                error.Details.Add("The FTP folder you want the file uploaded to was not provided.");
            }

            if (string.IsNullOrWhiteSpace(tokenizedFtpFilename.Text))
            {
                error.Details.Add("The file name that gets saved on the FTP server was not provided.");
            }

            // Add the error to the main errors collection if there are any validation errors
            if (error.Details.Any())
            {
                errors.Add(error);
            }
        }
    }
}
