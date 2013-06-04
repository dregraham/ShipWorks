using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// Class for displaying an open file dialog and importing a token from disk. 
    /// </summary>
    public class TokenImportDialog : IDisposable
    {
        private OpenFileDialog tokenDialog;
        private DialogResult dialogResult;
        private IEbayWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenImportDialog"/> class.
        /// </summary>
        public TokenImportDialog()
            : this("eBay Token File (*.tkn)|*.tkn")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenImportDialog"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public TokenImportDialog(string filter)
            : this(filter, new EbayWebClient(new EbayRequestFactory()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenImportDialog"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public TokenImportDialog(string filter, IEbayWebClient webClient)
        {
            this.webClient = webClient;

            tokenDialog = new OpenFileDialog();
            tokenDialog.Filter = filter;

            dialogResult = DialogResult.None;
        }

        
        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public DialogResult Show(IWin32Window owner)
        {
            dialogResult = tokenDialog.ShowDialog(owner);
            return dialogResult;
        }

        /// <summary>
        /// Imports the token for the specified eBay store.
        /// </summary>
        /// <param name="ebayStore">The eBay store.</param>
        /// <returns>A Boolean value indicating whether the token was imported successfully.</returns>
        public Token GetToken()
        {
            if (dialogResult == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                
                // Load the token based on the file from disk
                string license = ShipWorksSession.InstanceID.ToString();
                Token authorizationToken = new Authorization.Token(license);
                authorizationToken.Load(new FileInfo(tokenDialog.FileName));

                if (string.IsNullOrEmpty(authorizationToken.UserId))
                {
                    // There was not a user ID in the token file, so we need to explicitly look 
                    // up the user ID with the ebay user info request. A missing user ID would
                    // be the result of importing a token prior to the Authorization.Token class
                    // was implemented, so this is just a stop-gap solution for backwards compatibility.                    
                    GetUserResponseType userInfo = webClient.GetUserInfo(authorizationToken.Key);

                    // Create a new token that includes the user ID value
                    authorizationToken = new Token(license, new TokenData { UserId = userInfo.User.UserID, Key = authorizationToken.Key, ExpirationDate = authorizationToken.ExpirationDate });
                }

                return authorizationToken;
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
