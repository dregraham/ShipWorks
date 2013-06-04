using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// Class for displaying an save file dialog and exporting a token to disk. 
    /// </summary>
    public class TokenExportDialog : IDisposable
    {
        private SaveFileDialog tokenDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenExportDialog"/> class.
        /// </summary>
        public TokenExportDialog()
            : this ("eBay Token File (*.tkn)|*.tkn")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenExportDialog"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public TokenExportDialog(string filter)
        {
            tokenDialog = new SaveFileDialog();
            tokenDialog.Filter = filter;
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public DialogResult Show(IWin32Window owner)
        {
            return tokenDialog.ShowDialog(owner);
        }

        /// <summary>
        /// Gets the name of the selected file.
        /// </summary>
        /// <returns>The selected file name.</returns>
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
