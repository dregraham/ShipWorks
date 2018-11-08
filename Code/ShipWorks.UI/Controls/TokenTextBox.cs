using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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
    [TemplatePart(Name = "PART_TogglePopupButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_SuggestionSelector", Type = typeof(Selector))]
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public class TokenTextBox : TextBox
    {
        private ButtonBase editorButton;
        private Selector suggestionSelector;
        private ToggleButton popupButton;
        private Popup popup;
        private readonly ControlOwnerProvider ownerProvider;
        
        public static readonly DependencyProperty ButtonBackgroundProperty = 
            DependencyProperty.Register("ButtonBackground", typeof(Brush), 
                                        typeof(TokenTextBox), new UIPropertyMetadata(SystemColors.ControlBrush));
        
        public static readonly DependencyProperty ButtonIconColorProperty = 
            DependencyProperty.Register("ButtonIconColor", typeof(Brush), 
                                        typeof(TokenTextBox), new UIPropertyMetadata(Brushes.Black));

        
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
        /// Background color for buttons
        /// </summary>
        public Brush ButtonBackground
        {
            get => (Brush) GetValue(ButtonBackgroundProperty);
            set => SetValue(ButtonBackgroundProperty, value);
        }

        /// <summary>
        /// Icon color for buttons
        /// </summary>
        public Brush ButtonIconColor
        {
            get => (Brush) GetValue(ButtonIconColorProperty);
            set => SetValue(ButtonIconColorProperty, value);
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

            if (popup != null)
            {
                popup.PreviewMouseWheel -= OnPopupPreviewMouseWheel;
            }

            popup = GetTemplateChild("PART_Popup") as Popup;

            if (popup == null)
            {
                throw new InvalidOperationException("PART_Popup is not available in the template");
            }

            popup.PreviewMouseWheel += OnPopupPreviewMouseWheel;
        }

        /// <summary>
        /// Handle scrolling when the popup is open
        /// </summary>
        /// <remarks>
        /// We want to suppress scrolling of the screen when the popup is open so that the button doesn't
        /// scroll away from the popup. This mimics the behavior of the built-in combobox.
        /// </remarks>
        private void OnPopupPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) =>
            e.Handled = sender is Popup senderPopup && senderPopup.IsOpen;

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
