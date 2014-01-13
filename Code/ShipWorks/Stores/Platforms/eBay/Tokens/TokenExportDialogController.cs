using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// Class for displaying an save file dialog and exporting a token to disk. 
    /// </summary>
    public class TokenExportDialogController : IDisposable
    {
        SaveFileDialog tokenDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenExportDialogController"/> class.
        /// </summary>
        public TokenExportDialogController()
        {
            tokenDialog = new SaveFileDialog();
            tokenDialog.Filter = "eBay Token File (*.tkn)|*.tkn";
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        public DialogResult Show(IWin32Window owner)
        {
            return tokenDialog.ShowDialog(owner);
        }

        /// <summary>
        /// Gets the name of the selected file.
        /// </summary>
        public string GetSelectedFileName()
        {
            if (!string.IsNullOrEmpty(tokenDialog.FileName))
            {
                return tokenDialog.FileName;
            }
            else
            {
                throw new InvalidOperationException("A file was not selected.");
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
