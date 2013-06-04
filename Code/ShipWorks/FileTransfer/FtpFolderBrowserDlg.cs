using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.Threading.Tasks;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Window for browsing for FTP folders
    /// </summary>
    public partial class FtpFolderBrowserDlg : Form
    {
        FtpAccountEntity ftpAccount;
        string initialFolder;

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpFolderBrowserDlg(FtpAccountEntity ftpAccount, string initialFolder)
        {
            InitializeComponent();

            this.ftpAccount = ftpAccount;
            this.initialFolder = initialFolder;
        }

        /// <summary>
        /// The selected folder. Only valid if DialogResult is OK
        /// </summary>
        public string SelectedFolder
        {
            get
            {
                return folderBrowser.SelectedFolder;
            }
        }

        /// <summary>
        /// Window is being shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            // Prepare for async work
            this.Cursor = Cursors.WaitCursor;
            cancel.Enabled = false;
            ok.Enabled = false;

            // Initialize the folder list
            folderBrowser.BeginInitialize(ftpAccount, initialFolder, (Task task) =>
            {
                // Async work complete
                this.Cursor = Cursors.Default;
                cancel.Enabled = true;

                if (task.Exception != null)
                {
                    Exception actualEx = task.Exception.Flatten().InnerExceptions[0];

                    MessageHelper.ShowError(this, "There was an error loading the folder list:\n\n" + actualEx.Message);
                }
                else
                {
                    ok.Enabled = true;
                }
            });
        }

        /// <summary>
        /// OK'ing the final selection
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(folderBrowser.SelectedFolder))
            {
                MessageHelper.ShowInformation(this, "Please select a folder.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is trying to be closed
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !cancel.Enabled;
        }
    }
}
