using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI.Controls.Design;
using WinForms = System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Text box that allows template editing
    /// </summary>
    [TemplatePart(Name = "PART_EditorButton", Type = typeof(ButtonBase))]
    public class TokenTextBox : TextBox
    {
        private ButtonBase editorButton;
        private readonly ControlOwnerProvider ownerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        static TokenTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TokenTextBox), new FrameworkPropertyMetadata(typeof(TokenTextBox)));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenTextBox()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            ownerProvider = new ControlOwnerProvider(this);
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetupEditorButton();
        }

        /// <summary>
        /// Setup the editor button
        /// </summary>
        private void SetupEditorButton()
        {
            if (editorButton != null)
            {
                editorButton.Click -= OnEditorButtonClick;
            }

            editorButton = GetTemplateChild("PART_EditorButton") as ButtonBase;

            if (editorButton == null)
            {
                throw new InvalidOperationException("PART_Button is not available in the template");
            }

            editorButton.Click += OnEditorButtonClick;
        }

        /// <summary>
        /// Handle the editor button click
        /// </summary>
        private void OnEditorButtonClick(object sender, RoutedEventArgs e)
        {
            using (TemplateTokenEditorDlg dlg = new TemplateTokenEditorDlg())
            {
                dlg.TokenText = Text;

                if (dlg.ShowDialog(ownerProvider.Owner) == WinForms.DialogResult.OK)
                {
                    Text = dlg.TokenText;
                }
            }
        }
    }
}
