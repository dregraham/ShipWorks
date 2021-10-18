using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Utility;
using ActiproSoftware.SyntaxEditor;
using ShipWorks.UI;
using ActiproSoftware.SyntaxEditor.Addons.Xml;
using System.Xml.Schema;
using Interapptive.Shared.Utility;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;

namespace ShipWorks.Templates.Controls.XslEditing
{
    /// <summary>
    /// UserControl for editing the XSL code of a template
    /// </summary>
    public partial class TemplateXslEditorControl : UserControl
    {
        // Languages can be resused, so this is static
        static ShipWorksXslSyntaxLanguage syntaxLanguage;

        // For highlighting xsl tags
        static HighlightingStyle xslTagStyle = new HighlightingStyle("XslTag", "XslTag", Color.Teal, Color.White);

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXslEditorControl()
        {
            InitializeComponent();

            this.toolStrip.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (syntaxLanguage == null)
            {
                syntaxLanguage = new ShipWorksXslSyntaxLanguage();
            }

            // Set the language
            syntaxLanguage.ApplyTo(syntaxEditor);

            // Use spaces instead of tabs
            syntaxEditor.Document.AutoConvertTabsToSpaces = true;

            // Initialize find\replace
            findReplaceControl.Initialize(syntaxEditor);

            // Listen for text changes so we can syntax highlight
            syntaxEditor.Document.TextChanged += new DocumentModificationEventHandler(OnDocumentTextChanged);
        }

        /// <summary>
        /// Gets \ sets the XSL being edited
        /// </summary>
        public string TemplateXsl
        {
            get
            {
                return syntaxEditor.Document.Text;
            }
            set
            {
                syntaxEditor.Document.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the header XSL that gets logically appended to the beginning of TemplateXsl .
        /// </summary>
        public string HeaderXsl
        {
            get
            {
                return syntaxEditor.Document.HeaderText;
            }
            set
            {
                syntaxEditor.Document.HeaderText = value;
            }
        }

        /// <summary>
        /// Gets or sets the footer XSL that gets logically appended to the end of TemplateXsl.
        /// </summary>
        public string FooterXsl
        {
            get
            {
                return syntaxEditor.Document.FooterText;
            }
            set
            {
                syntaxEditor.Document.FooterText = value;
            }
        }

        /// <summary>
        /// The text of the document has changed
        /// </summary>
        void OnDocumentTextChanged(object sender, DocumentModificationEventArgs e)
        {
            foreach (IToken token in e.Document.Tokens)
            {
                DynamicToken dynamic = (DynamicToken) token;
                if (dynamic.TextRange.IntersectsWith(e.DirtyTextRange))
                {
                    if (dynamic.Key.StartsWith("StartTag", StringComparison.OrdinalIgnoreCase) || dynamic.Key.StartsWith("EndTag", StringComparison.OrdinalIgnoreCase))
                    {
                        if (e.Document.GetTokenText(dynamic).StartsWith("xsl:"))
                        {
                            dynamic.CustomHighlightingStyle = xslTagStyle;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ensure that the Find\Replace window is closed
        /// </summary>
        public void CloseFindReplace()
        {
            findReplaceControl.Reset();
            findReplaceControl.Visible = false;

            buttonSearch.Checked = false;
        }

        /// <summary>
        /// Updates the UI state of the toolbar.
        /// </summary>
        private void UpdateToolbarUI()
        {
            buttonUndo.Enabled = syntaxEditor.Document.UndoRedo.CanUndo;
            buttonRedo.Enabled = syntaxEditor.Document.UndoRedo.CanRedo;
        }

        /// <summary>
        /// Undo
        /// </summary>
        private void OnUndo(object sender, EventArgs e)
        {
            syntaxEditor.Document.UndoRedo.Undo();
        }

        /// <summary>
        /// Redo
        /// </summary>
        private void OnRedo(object sender, EventArgs e)
        {
            syntaxEditor.Document.UndoRedo.Redo();
        }

        /// <summary>
        /// Cut
        /// </summary>
        private void OnCut(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.CutToClipboard();
        }

        /// <summary>
        /// Copy
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.CopyToClipboard();
        }

        /// <summary>
        /// Paste
        /// </summary>
        private void OnPaste(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.PasteFromClipboard();
        }

        /// <summary>
        /// Select All
        /// </summary>
        private void OnSelectAll(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.Selection.SelectAll();
        }

        /// <summary>
        /// Open the find\replace window
        /// </summary>
        private void OnSearch(object sender, EventArgs e)
        {
            if (!findReplaceControl.Visible)
            {
                ActivateFindReplace();
            }
            else
            {
                CloseFindReplace();
            }
        }

        /// <summary>
        /// The undo\redo stack has changed.  Update the UI.
        /// </summary>
        private void OnUndoRedoStateChanged(object sender,UndoRedoStateChangedEventArgs e)
        {
            UpdateToolbarUI();
        }

        /// <summary>
        /// Context menu is being opened
        /// </summary>
        private void OnOpeningContextMenu(object sender, CancelEventArgs e)
        {
            menuItemUndo.Enabled = buttonUndo.Enabled;
            menuItemRedo.Enabled = buttonRedo.Enabled;
        }

        /// <summary>
        /// Clicking a key in the editor
        /// </summary>
        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F || e.KeyCode == Keys.H) && e.Control)
            {
                ActivateFindReplace();
            }
        }

        /// <summary>
        /// Show\activate the find\replace window
        /// </summary>
        private void ActivateFindReplace()
        {
            string findText = syntaxEditor.SelectedView.SelectedText;
            if (findText.Length == 0)
            {
                findText = syntaxEditor.SelectedView.GetCurrentWordText().Trim();
            }

            if (findText.Length > 30 || findText.Contains("\n"))
            {
                findText = string.Empty;
            }

            if (findText.Length > 0)
            {
                findReplaceControl.SetFindText(findText);
            }

            findReplaceControl.Visible = true;
            ActiveControl = findReplaceControl;

            buttonSearch.Checked = true;
        }

        /// <summary>
        /// Clicked the close-button on the Find\Replace panel
        /// </summary>
        private void OnCloseFindReplace(object sender, EventArgs e)
        {
            CloseFindReplace();
        }

        /// <summary>
        /// Process keys
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Toggle whitespace
            if (keyData == (Keys.Control | Keys.W))
            {
                // syntaxEditor.WhitespaceLineEndsVisible = !syntaxEditor.WhitespaceLineEndsVisible;
                syntaxEditor.WhitespaceSpacesVisible = !syntaxEditor.WhitespaceSpacesVisible;
                syntaxEditor.WhitespaceTabsVisible = !syntaxEditor.WhitespaceTabsVisible;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
