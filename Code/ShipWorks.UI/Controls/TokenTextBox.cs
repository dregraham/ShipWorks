using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Autofac;
using ShipWorks.ApplicationCore;
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
        private Selector suggestionSelector;
        private ToggleButton popupButton;
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

            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            SetupEditorButton();
            SetupSuggestions();
            SetupPopupButton();
        }

        /// <summary>
        /// Setup the popup button
        /// </summary>
        private void SetupPopupButton()
        {
            popupButton = GetTemplateChild("PART_TogglePopupButton") as ToggleButton;
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
                throw new InvalidOperationException("PART_EditorButton is not available in the template");
            }

            editorButton.Click += OnEditorButtonClick;
        }

        /// <summary>
        /// Setup suggestions
        /// </summary>
        private void SetupSuggestions()
        {
            if (suggestionSelector != null)
            {
                suggestionSelector.SelectionChanged -= OnSuggestionSelectorSelectionChanged;
            }

            suggestionSelector = GetTemplateChild("PART_SuggestionSelector") as Selector;

            if (suggestionSelector == null)
            {
                throw new InvalidOperationException("PART_SuggestionSelector is not available in the template");
            }

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var tokenSuggestions = lifetimeScope.Resolve<ITokenSuggestionFactory>();
                suggestionSelector.ItemsSource = tokenSuggestions.GetSuggestions(TokenUsage.Generic);
                suggestionSelector.SelectionChanged += OnSuggestionSelectorSelectionChanged;
            }
        }

        /// <summary>
        /// Handle suggestion selection
        /// </summary>
        private void OnSuggestionSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedToken = e.AddedItems.OfType<TokenSuggestion>().FirstOrDefault();
            if (selectedToken != null)
            {
                Text = selectedToken.Xsl;
                suggestionSelector.SelectedItem = null;
            }

            // Close the popup regardless of whether there is a selection
            popupButton.IsChecked = false;
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
