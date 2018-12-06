using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property for highlighting all of a textboxes text on focus
    /// </summary>
    public class SelectTextOnFocus : DependencyObject
    {
        /// <summary>
        /// Whether or not this attached property is enabled
        /// </summary>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
            "Enabled",
            typeof(bool),
            typeof(SelectTextOnFocus),
            new PropertyMetadata(false, EnabledPropertyChanged));

        
        /// <summary>
        /// Get the enabled property
        /// </summary>
        public static bool GetEnabled(DependencyObject d)
        {
            return (bool) d.GetValue(EnabledProperty);
        }

        /// <summary>
        /// Set the enabled property
        /// </summary>
        public static void SetEnabled(DependencyObject d, bool value)
        {
            d.SetValue(EnabledProperty, value);
        }

        /// <summary>
        /// Hook up or unhook events when the enabled property changes
        /// </summary>
        private static void EnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotKeyboardFocus -= OnKeyboardFocusSelectText;
                textBox.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                
                if (e.NewValue as bool? == true)
                {
                    textBox.GotKeyboardFocus += OnKeyboardFocusSelectText;
                    textBox.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                }
            }
        }

        /// <summary>
        /// Focus textbox when left clicked
        /// </summary>
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = GetParentFromVisualTree(e.OriginalSource);

            if (dependencyObject is TextBox textBox && !textBox.IsKeyboardFocusWithin)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Highlight all text when textbox receives focus 
        /// </summary> 
        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        /// <summary>
        /// Get the given dependency objects parent from the visual tree
        /// </summary>
        private static DependencyObject GetParentFromVisualTree(object source)
        {
            DependencyObject parent = source as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }
    }
}