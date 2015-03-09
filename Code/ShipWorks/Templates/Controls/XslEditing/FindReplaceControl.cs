using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using Interapptive.Shared.UI;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Templates.Controls.XslEditing
{
    /// <summary>
    /// UserControl that contains all find\replace UI and logic for SyntaxEditor
    /// </summary>
    public partial class FindReplaceControl : UserControl
    {
        SyntaxEditor syntaxEditor;

        // The find\replace options to use
        FindReplaceOptions options = new FindReplaceOptions();

        // We swap it out - this is the actual real one that should be used.
        Button formAcceptButton;

        /// <summary>
        /// Indicates if span indicator marks need cleared
        /// </summary>
        bool needClearFindAllMarks = false;

        // Raised when the Close button is clicked - up to the owning control to do something about it.
        public event EventHandler CloseClicked;

        /// <summary>
        /// Constructor
        /// </summary>
        public FindReplaceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control to do find\replace on the given syntax editor
        /// </summary>
        public void Initialize(SyntaxEditor syntaxEditor)
        {
            this.syntaxEditor = syntaxEditor;
            this.syntaxEditor.TextChanged += new EventHandler(OnSyntaxEditorTextChanged);

            if (!DesignModeDetector.IsDesignerHosted())
            {
                formAcceptButton = (Button) ((Form) TopLevelControl).AcceptButton;
            }
        }

        /// <summary>
        /// Reset the find marks and sazve the find options for next time
        /// </summary>
        public void Reset()
        {
            ClearFindAllMarks();
            SaveFindReplaceOptions();
        }

        [Category("Appearance")]
        [DefaultValue("Close")]
        public string CloseButtonText
        {
            get { return close.Text; }
            set { close.Text = value; }
        }

        /// <summary>
        /// The text content of the control is changing
        /// </summary>
        void OnSyntaxEditorTextChanged(object sender, EventArgs e)
        {
            ClearFindAllMarks();
        }

        /// <summary>
        /// The text to find has changed
        /// </summary>
        private void OnFindTextChanged(object sender, EventArgs e)
        {
            find.Enabled = (findText.Text.Length > 0);
            findAll.Enabled = find.Enabled;
            replace.Enabled = find.Enabled;
            replaceAll.Enabled = find.Enabled;
        }

        /// <summary>
        /// Set the text loaded in the find box of the control
        /// </summary>
        public void SetFindText(string findText)
        {
            this.findText.Text = findText;
        }

        /// <summary>
        /// Control is getting focus
        /// </summary>
        private void OnFocusEnter(object sender, EventArgs e)
        {
            findText.SelectAll();
            findText.Focus();
        }

        /// <summary>
        /// Find
        /// </summary>
        private void OnFind(object sender, EventArgs e)
        {
            ClearFindAllMarks();

            // Update find/replace options
            SaveFindReplaceOptions();

            // Perform a find operation
            FindReplaceResultSet resultSet = syntaxEditor.SelectedView.FindReplace.Find(options);

            if (resultSet.Count == 0 && resultSet.PastSearchStartOffset)
            {
                resultSet = syntaxEditor.SelectedView.FindReplace.Find(options);
            }

            // If no matches were found...			
            if (resultSet.Count == 0)
            {
                MessageHelper.ShowInformation(this, "The specified text was not found.");
                return;
            }
        }

        /// <summary>
        /// Find every match in the document
        /// </summary>
        private void OnFindAll(object sender, EventArgs e)
        {
            // Update find/replace options
            SaveFindReplaceOptions();

            FindReplaceResultSet resultSet = syntaxEditor.Document.FindReplace.MarkAll(options, typeof(FindAllIndicator));

            // If no matches were found...			
            if (resultSet.Count == 0)
            {
                MessageHelper.ShowInformation(this, "The specified text was not found.");
                return;
            }
            else
            {
                needClearFindAllMarks = true;
            }
        }

        /// <summary>
        /// Replace
        /// </summary>
        private void OnReplace(object sender, EventArgs e)
        {
            ClearFindAllMarks();

            // Update find/replace options
            SaveFindReplaceOptions();

            // Perform a find operation
            FindReplaceResultSet resultSet = syntaxEditor.SelectedView.FindReplace.Replace(options);

            // If no matches were found...			
            if (resultSet.Count == 0)
            {
                if (resultSet.PastSearchStartOffset || resultSet.ReplaceOccurred)
                {
                    MessageHelper.ShowInformation(this, "There are no more occurrences to replace.");
                }
                else
                {
                    MessageHelper.ShowInformation(this, "The specified text was not found.");
                }
            }
        }

        /// <summary>
        /// Replace All
        /// </summary>
        private void OnReplaceAll(object sender, EventArgs e)
        {
            ClearFindAllMarks();

            // Update find/replace options
            SaveFindReplaceOptions();

            // Perform a mark all operation
            FindReplaceResultSet resultSet = syntaxEditor.SelectedView.FindReplace.ReplaceAll(options);

            // If no matches were found...
            if (resultSet.Count == 0)
            {
                MessageHelper.ShowInformation(this, "The specified text was not found.");
            }
            else
            {
                // Display the number of replacements
                MessageHelper.ShowInformation(this, string.Format("{0} occurrence(s) replaced.", resultSet.Count));
            }
        }

        /// <summary>
        /// Updates the find/replace options.
        /// </summary>
        private void SaveFindReplaceOptions()
        {
            options.FindText = findText.Text;
            options.ReplaceText = replaceText.Text;
            options.MatchCase = matchCaseCheckBox.Checked;
            options.MatchWholeWord = matchWholeWordCheckBox.Checked;
            options.SearchUp = searchUpCheckBox.Checked;
            options.SearchType = (searchTypeCheckBox.Checked ? FindReplaceSearchType.RegularExpression : FindReplaceSearchType.Normal);
        }

        /// <summary>
        /// Clear any marks made due to a Find All operation
        /// </summary>
        private void ClearFindAllMarks()
        {
            if (needClearFindAllMarks)
            {
                syntaxEditor.Document.FindReplace.ClearSpanIndicatorMarks();

                needClearFindAllMarks = false;
            }
        }

        /// <summary>
        /// Find box is getting focus
        /// </summary>
        private void OnEnterFind(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).AcceptButton = find;
        }

        /// <summary>
        /// Find box is losing focus
        /// </summary>
        private void OnLeaveFind(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).AcceptButton = formAcceptButton;
        }

        /// <summary>
        /// Replace box is getting focus
        /// </summary>
        private void OnEnterReplace(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).AcceptButton = replace;
        }

        /// <summary>
        /// Replace box is losing focus
        /// </summary>
        private void OnLeaveReplace(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).AcceptButton = formAcceptButton;
        }

        /// <summary>
        /// Close button was clicked
        /// </summary>
        private void OnClose(object sender, EventArgs e)
        {
            // Pass it on...
            if (CloseClicked != null)
            {
                CloseClicked(this, EventArgs.Empty);
            }
        }
    }
}
