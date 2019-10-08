using System;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// UserControl for managing and renewing an ebay token
    /// </summary>
    public partial class EbayTokenManageControl : UserControl
    {
        EbayStoreEntity store;

        /// <summary>
        /// Raised when a token has been succesfully created (or loaded from file) and imported into shipworks
        /// </summary>
        public event EventHandler TokenImported;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayTokenManageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control for the given store
        /// </summary>
        public void InitializeForStore(EbayStoreEntity store)
        {
            createTokenControl.InitializeForStore(store);

            this.store = store;

            UpdateTokenDisplay();
        }

        /// <summary>
        /// Update the display of the token
        /// </summary>
        private void UpdateTokenDisplay()
        {
            if (store.EBayToken.Length == 0)
            {
                tokenBox.Text = "None";
            }
            else
            {
                tokenBox.Text = string.Format("For '{0}', {1} on {2}", store.EBayUserID, store.EBayTokenExpire <= DateTime.UtcNow ? "expired" : "expires", store.EBayTokenExpire.ToShortDateString());
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
            using (TokenImportDialogController importDialog = new TokenImportDialogController())
            {
                try
                {
                    if (importDialog.Show(this) == DialogResult.OK)
                    {
                        // A token file has been selected, so we'll import the token from file 
                        // and use the imported token to configure the eBay store
                        EbayToken token = importDialog.GetToken();

                        store.EBayToken = token.Token;
                        store.EBayTokenExpire = token.ExpirationDate;
                        store.EBayUserID = token.UserId;

                        createTokenControl.CancelWaiting();
                        OnUpdateTokenCompleted(null, EventArgs.Empty);

                        MessageHelper.ShowInformation(this, "The token file has been imported.");
                    }
                }
                catch (EbayException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Export a token to a file
        /// </summary>
        private void OnExportTokenFile(object sender, EventArgs e)
        {
            if (store.EBayToken.Length > 0)
            {
                // We have an eBay token to export, so use the TokenExportDialog for the user to select
                // where to save the token to
                using (TokenExportDialogController exportDialog = new TokenExportDialogController())
                {
                    if (exportDialog.Show(this) == DialogResult.OK)
                    {
                        try
                        {
                            // We need to construct the token data to save a new token
                            EbayToken token = EbayToken.FromStore(store);

                            // Grab the file that was selected and save the token
                            FileInfo file = new FileInfo(exportDialog.GetSelectedFileName());
                            token.Save(file);

                            MessageHelper.ShowInformation(this, "The token was successfully exported.");
                        }
                        catch (IOException ex)
                        {
                            // This is to handle IO exceptions from the creation of the FileInfo object
                            MessageHelper.ShowError(this, String.Format("Unable to save token file: '{0}'", ex.Message));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // This is to handle access exceptions from the creation of the FileInfo object
                            MessageHelper.ShowError(this, String.Format("Unable to save token file: Access denied to '{0}'.", exportDialog.GetSelectedFileName()));
                        }
                        catch (EbayException ex)
                        {
                            MessageHelper.ShowError(this, ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageHelper.ShowError(this, "You do not have an eBay Login Token to export.");
            }
        }
    }
}
