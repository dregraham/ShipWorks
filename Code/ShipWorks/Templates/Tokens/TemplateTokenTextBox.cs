using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ComponentFactory.Krypton.Toolkit;
using ShipWorks.UI.Controls.Design;
using Interapptive.Shared;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Win32;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Control for editing ShipWorks tokens
    /// </summary>
    public partial class TemplateTokenTextBox : UserControl
    {
        bool isMultiValued = false;

        TokenUsage usage = TokenUsage.Generic;
        TokenSelectionMode selectionMode = TokenSelectionMode.Replace;
        private ITokenSuggestionFactory tokenSuggestionFactory;

        /// <summary>
        /// Raised when the Text property changes
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        // Hook for displaying the multi-value text over the text box
        InnerTextBoxHook textBoxHook = null;

        /// <summary>
        /// This is used to draw the prompt when the ComboBox is a DropDown, and not a DropDownList.  When its a 
        /// DropDown, it uses an enter Edit control, which we subclass to do the drawing.
        /// </summary>
        class InnerTextBoxHook : NativeWindow
        {
            TemplateTokenTextBox parent;

            /// <summary>
            /// Constructor
            /// </summary>
            public InnerTextBoxHook(TemplateTokenTextBox parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// WndPrc of subclassed child edit control
            /// </summary>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == NativeMethods.WM_PAINT && !parent.tokenTextBox.TextBox.Focused && parent.isMultiValued)
                {
                    using (Graphics g = Graphics.FromHwnd(m.HWnd))
                    {
                        DrawTextPrompt(g, parent.Font, new Rectangle(0, 0, parent.Width - 10, parent.Height - 4));
                    }
                }
            }

            /// <summary>
            /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
            /// </summary>
            protected static void DrawTextPrompt(Graphics g, Font font, Rectangle rect)
            {
                TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.Left;

                // Draw the prompt text using TextRenderer
                TextRenderer.DrawText(g, MultiValueExtensions.MultiText, font, rect, MultiValueExtensions.MultiColor, Color.Transparent, flags);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateTokenTextBox" /> class using the
        /// TokenSuggestionGroups class for generating token suggestions.
        /// </summary>
        public TemplateTokenTextBox()
        {
            InitializeComponent();
            tokenSuggestionFactory = new CommonTokenSuggestionsFactory();

            UpdateTokenSuggestionsMenu();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            tokenTextBox.StateCommon.Border.Color1 = VisualStyleInformation.TextControlBorder;
            tokenTextBox.TextChanged += new EventHandler(OnInnerTextChanged);

            textBoxHook = new InnerTextBoxHook(this);
            textBoxHook.AssignHandle(tokenTextBox.TextBox.Handle);

            tokenTextBox.TextBox.HandleDestroyed += new EventHandler(OnInnerTextBoxHandleDestroyed);
            tokenTextBox.TextBox.HandleCreated += new EventHandler(OnInnerTextBoxHandleCreated);
        }

        /// <summary>
        /// Inner text box handle has been created
        /// </summary>
        void OnInnerTextBoxHandleCreated(object sender, EventArgs e)
        {
            OnInnerTextBoxHandleDestroyed(sender, e);

            textBoxHook = new InnerTextBoxHook(this);
            textBoxHook.AssignHandle(tokenTextBox.TextBox.Handle);
        }

        /// <summary>
        /// Inner text box handle has been destroyed
        /// </summary>
        void OnInnerTextBoxHandleDestroyed(object sender, EventArgs e)
        {
            if (textBoxHook != null)
            {
                textBoxHook.ReleaseHandle();
                textBoxHook = null;
            }
        }

        /// <summary>
        /// Gets or sets the token suggestion factory to use for generation a list of token suggestions 
        /// (allowing the behavior of the control to be modified via property injection)
        /// </summary>
        /// <value>The token suggestion factory.</value>
        public ITokenSuggestionFactory TokenSuggestionFactory
        {
            get { return tokenSuggestionFactory; }
            set 
            { 
                tokenSuggestionFactory = value;
                UpdateTokenSuggestionsMenu();
            }
        }

        /// <summary>
        /// Get \ set whether the box represents a null value.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool MultiValued
        {
            get
            {
                return isMultiValued;
            }
            set
            {
                if (value == isMultiValued)
                {
                    return;
                }

                if (value)
                {
                    SetInnerText("");
                }

                isMultiValued = value;
                tokenTextBox.TextBox.Invalidate();
            }
        }

        /// <summary>
        /// The token text
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return tokenTextBox.Text;
            }
            set
            {
                SetInnerText(value);

                if (isMultiValued)
                {
                    isMultiValued = false;
                    tokenTextBox.TextBox.Invalidate();
                }
            }
        }

        [DefaultValue(TokenUsage.Generic)]
        public TokenUsage TokenUsage
        {
            get
            {
                return usage;
            }
            set
            {
                if (usage == value)
                {
                    return;
                }

                usage = value;

                UpdateTokenSuggestionsMenu();
            }
        }

        /// <summary>
        /// The replacement mode when the user selects a token.
        /// </summary>
        [DefaultValue(TokenSelectionMode.Replace)]
        public TokenSelectionMode TokenSelectionMode
        {
            get { return selectionMode; }
            set { selectionMode = value; }
        }

        /// <summary>
        /// Indicates if the TextBox supports multiple lines.
        /// </summary>
        [DefaultValue(false)]
        public bool Multiline
        {
            get
            {
                return tokenTextBox.Multiline;
            }
            set
            {
                tokenTextBox.Multiline = value;
                tokenTextBox.ScrollBars = value ? ScrollBars.Vertical : ScrollBars.None;
                tokenTextBox.AcceptsReturn = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the token
        /// </summary>
        [DefaultValue(0)]
        public int MaxLength
        {
            get { return tokenTextBox.MaxLength; }
            set { tokenTextBox.MaxLength = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show token options].
        /// </summary>
        [DefaultValue(true)]
        public bool ShowTokenOptions
        {
            get { return tokenOptionsDropdown.Visible; }
            set { tokenOptionsDropdown.Visible = value; }
        }

        /// <summary>
        /// Set the text of our inner text box, advancing the selection caret to the end of the string
        /// </summary>
        private void SetInnerText(string value)
        {
            tokenTextBox.Text = value;
            tokenTextBox.SelectionStart = tokenTextBox.Text.Length;
            tokenTextBox.SelectionLength = 0;
        }

        /// <summary>
        /// Update the token suggestions menu based on the selected usage
        /// </summary>
        private void UpdateTokenSuggestionsMenu()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            contextMenuStrip.Items.Clear();

            // Add each suggestion to the menu
            foreach (TokenSuggestion suggestion in TokenSuggestionFactory.GetSuggestions(usage))
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(suggestion.Xsl, null, OnInsertToken);

                if (!string.IsNullOrWhiteSpace(suggestion.Description))
                {
                    contextMenuStrip.Items.Add(new ToolStripLabel(suggestion.Description) { Font = new Font(Font, FontStyle.Bold)  });
                }

                contextMenuStrip.Items.Add(menuItem);

                if (!string.IsNullOrWhiteSpace(suggestion.Description))
                {
                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                }
            }

            // Remove the last separator
            if (contextMenuStrip.Items.Count > 0 && contextMenuStrip.Items[contextMenuStrip.Items.Count - 1] is ToolStripSeparator)
            {
                contextMenuStrip.Items.RemoveAt(contextMenuStrip.Items.Count - 1);
            }
        }

        /// <summary>
        /// Open the token editor
        /// </summary>
        private void OnEditToken(object sender, EventArgs e)
        {
            using (TemplateTokenEditorDlg dlg = new TemplateTokenEditorDlg())
            {
                dlg.TokenText = tokenTextBox.Text;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    SetInnerText(dlg.TokenText);
                }
            }
        }

        /// <summary>
        /// Insert a token from the quick token menu
        /// </summary>
        private void OnInsertToken(object sender, EventArgs e)
        {
            string text = ((ToolStripMenuItem) sender).Text;

            if (TokenSelectionMode == Tokens.TokenSelectionMode.Paste)
            {
                tokenTextBox.TextBox.Paste(text);
            }
            else
            {
                SetInnerText(text);
            }
        }

        /// <summary>
        /// Raised by the inner textbox to notify that text has changed
        /// </summary>
        void OnInnerTextChanged(object sender, EventArgs e)
        {
            if (isMultiValued)
            {
                isMultiValued = false;
                tokenTextBox.TextBox.Invalidate();
            }

            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The token options box is dropping down
        /// </summary>
        private void OnClickTokenOptionsDropdown(object sender, EventArgs e)
        {
            if (TokenSelectionMode == Tokens.TokenSelectionMode.Paste)
            {
                tokenTextBox.Focus();
            }
        }
    }
}
