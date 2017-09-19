using System;
using System.IO;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// Class for displaying an open file dialog and importing a token from disk.
    /// </summary>
    public class TokenImportDialogController : IDisposable
    {
        OpenFileDialog tokenDialog;
        DialogResult dialogResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenImportDialogController"/> class.
        /// </summary>
        public TokenImportDialogController()
        {
            tokenDialog = new OpenFileDialog();
            tokenDialog.Filter = "eBay Token File (*.tkn)|*.tkn";

            dialogResult = DialogResult.None;
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        public DialogResult Show(IWin32Window owner)
        {
            dialogResult = tokenDialog.ShowDialog(owner);
            return dialogResult;
        }

        /// <summary>
        /// Get's the token selected by the file browser
        /// </summary>
        public EbayToken GetToken()
        {
            if (dialogResult == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                EbayToken token = new EbayToken();
                token.Load(new FileInfo(tokenDialog.FileName));

                if (string.IsNullOrEmpty(token.UserId))
                {
                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        IEbayWebClient webClient = lifetimeScope.Resolve<IEbayWebClient>();

                        // There was not a user ID in the token file, so we need to explicitly look
                        // up the user ID with the ebay user info request. We didn't always used to load the UserID,
                        // so this is for backwards compatibility
                        UserType userInfo = webClient.GetUser(token);

                        // Apply the UserID
                        token.UserId = userInfo.UserID;
                    }
                }

                return token;
            }
            else
            {
                throw new InvalidOperationException("A token file was not selected to be imported.");
            }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            tokenDialog.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
