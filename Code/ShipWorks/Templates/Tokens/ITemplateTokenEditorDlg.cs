﻿using System.Windows.Forms;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Represents a token editor dialog
    /// </summary>
    public interface ITemplateTokenEditorDlg
    {
        /// <summary>
        /// The token text for the dialog
        /// </summary>
        string TokenText { get; set; }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        DialogResult ShowDialog(IWin32Window owner);
    }
}