using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.IO;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// UserControl for managing and updating a NetworkSolutions token
    /// </summary>
    public partial class NetworkSolutionsManageTokenControl : UserControl
    {
        NetworkSolutionsStoreEntity store;

        /// <summary>
        /// Raised when a token has been succesfully created (or loaded from file) and imported into ShipWorks
        /// </summary>
        public event EventHandler TokenImported;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsManageTokenControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="store"></param>
        public void InitializeForStore(NetworkSolutionsStoreEntity store)
        {
            // setup the import token control
            createTokenControl.InitializeForStore(store);

            this.store = store;

            // change the UI
            UpdateTokenDisplay();
        }

        /// <summary>
        /// Update the display of the token
        /// </summary>
        private void UpdateTokenDisplay()
        {
            if (store.UserToken.Length == 0)
            {
                tokenTextBox.Text = "None";
            }
            else
            {
                tokenTextBox.Text = store.UserToken;
            }
        }

        /// <summary>
        /// A token has been created and imported succesfully
        /// </summary>
        private void OnUpdateTokenCompleted(object sender, EventArgs e)
        {
            UpdateTokenDisplay();

            if (TokenImported != null)
            {
                TokenImported(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Import a token from a token file
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (NetworkSolutionsWebClient.ImportTokenFile(store, this))
            {
                createTokenControl.CancelWaiting();

                OnUpdateTokenCompleted(null, EventArgs.Empty);

                MessageHelper.ShowInformation(this, "The token file has been imported.");
            }
        }

        /// <summary>
        /// Export a token to a file
        /// </summary>
        private void OnExportTokenFile(object sender, EventArgs e)
        {
            if (store.UserToken.Length == 0)
            {
                MessageHelper.ShowError(this, "You do not have a Network Solutions Login Token to export.");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Network Solutions Token File (*.nst)|*.nst";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // write the token
                    string saveToken =
                        "<NetworkSolutionsToken>" +
                        "<Token>" + store.UserToken + "</Token>" +
                        "</NetworkSolutionsToken>";

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        using (StreamWriter writer = new StreamWriter(dlg.FileName))
                        {
                            writer.Write(SecureText.Encrypt(saveToken, "token"));
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, String.Format("Unable to save token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageHelper.ShowError(this, String.Format("Unable to save token file: Access denied to '{0}'", dlg.FileName));
                    }
                }
            }
        }
    }
}
