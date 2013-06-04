using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Controls.XslEditing
{
    /// <summary>
    /// The Find\Replace window to use with the SyntaxEditor control
    /// </summary>
    public partial class FindReplaceDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FindReplaceDlg(SyntaxEditor syntaxEditor)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

            findReplaceControl.Initialize(syntaxEditor);
        }

        /// <summary>
        /// Set the text loaded in the find box of the control
        /// </summary>
        public void SetFindText(string findText)
        {
            findReplaceControl.SetFindText(findText);
        }

        /// <summary>
        /// Window is being activated
        /// </summary>
        private void OnActivated(object sender, EventArgs e)
        {
            ActiveControl = findReplaceControl;
        }

        /// <summary>
        /// Close the window
        /// </summary>
        private void OnClose(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Don't really close (which could dispose).  Just hide, so we can be reused.
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            findReplaceControl.Reset();

            Owner.Activate();
            Visible = false;
        }
    }
}
