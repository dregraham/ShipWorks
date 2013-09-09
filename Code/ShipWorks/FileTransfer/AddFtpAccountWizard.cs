using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using System.Threading.Tasks;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Wizard for adding FTP accounts 
    /// </summary>
    public partial class AddFtpAccountWizard : WizardForm
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AddFtpAccountWizard));

        bool selectFolder;
        string initialFolder;

        // The FTP account settings pending creation
        FtpAccountEntity ftpAccount;

        bool needLoadFolders = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddFtpAccountWizard(bool selectFolder)
        {
            InitializeComponent();

            this.selectFolder = selectFolder;
        }

        /// <summary>
        /// The FtpAccount that was creatd.  Only valid if the DialogResult is OK.
        /// </summary>
        public FtpAccountEntity FtpAccount
        {
            get
            {
                if (DialogResult != DialogResult.OK)
                {
                    return null;
                }

                return ftpAccount;
            }
        }

        /// <summary>
        /// The folder the user selected.  Only valid if constructed with 'selectFolder = true' and DialogResult is OK.
        /// </summary>
        public string InitialFolder
        {
            get { return initialFolder; }
        }

        /// <summary>
        /// Stepping next from the account setup page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            if (string.IsNullOrEmpty(host.Text))
            {
                MessageHelper.ShowInformation(this, "Please specify the host FTP server.");
                e.NextPage = CurrentPage;
                return;
            }

            if (string.IsNullOrEmpty(username.Text))
            {
                MessageHelper.ShowInformation(this, "Please specify your FTP username.");
                e.NextPage = CurrentPage;
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ftpAccount = FtpUtility.CreateMostSecureAccount(host.Text.Trim(), username.Text.Trim(), password.Text);
                needLoadFolders = true;
            }
            catch (UriFormatException)
            {
                MessageHelper.ShowError(this, "Invalid FTP host.");
                e.NextPage = CurrentPage;
                return;
            }
            catch (FileTransferException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the folder browser page
        /// </summary>
        private void OnSteppingIntoFolderBrowse(object sender, WizardSteppingIntoEventArgs e)
        {
            e.Skip = !selectFolder;
        }

        /// <summary>
        /// Folder browse page has been shown
        /// </summary>
        private void OnShownFolderBrowse(object sender, WizardPageShownEventArgs e)
        {
            if (needLoadFolders)
            {
                needLoadFolders = false;

                // Prepare for async work
                this.Cursor = Cursors.WaitCursor;
                NextEnabled = false;
                BackEnabled = false;
                CanCancel = false;

                // Initialize the folder list
                folderBrowser.BeginInitialize(ftpAccount, null, (Task task) =>
                {
                    // Async work complete
                    this.Cursor = Cursors.Default;
                    BackEnabled = true;
                    CanCancel = true;

                    if (task.Exception != null)
                    {
                        Exception actualEx = task.Exception.Flatten().InnerExceptions[0];

                        MessageHelper.ShowError(this, "There was an error loading the folder list:\n\n" + actualEx.Message);
                    }
                    else
                    {
                        NextEnabled = true;
                    }
                });
            }
        }

        /// <summary>
        /// Stepping next from the folder browse page
        /// </summary>
        private void OnStepNextFolderBrowse(object sender, WizardStepEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(folderBrowser.SelectedFolder))
            {
                MessageHelper.ShowInformation(this, "Please select a folder.");
                e.NextPage = CurrentPage;
                return;
            }

            initialFolder = folderBrowser.SelectedFolder;
        }

        /// <summary>
        /// Stepping into the success page
        /// </summary>
        private void OnSteppingIntoSuccess(object sender, WizardSteppingIntoEventArgs e)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(ftpAccount);
            }
        }
    }
}
