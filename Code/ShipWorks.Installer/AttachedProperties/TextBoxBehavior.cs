using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShipWorks.Installer.AttachedProperties
{
    /// <summary>
    /// Attached behavior for calling a command when a textbox loses focus
    /// </summary>
    public class TextBoxBehavior
    {
        public static DependencyProperty OnLostFocusProperty = DependencyProperty.RegisterAttached(
            "OnLostFocus",
            typeof(ICommand),
            typeof(TextBoxBehavior),
            new UIPropertyMetadata(OnLostFocus));

        /// <summary>
        /// Set the command
        /// </summary>
        public static void SetOnLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(OnLostFocusProperty, value);
        }

        /// <summary>
        /// Event handler
        /// </summary>
        private static void OnLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TextBox;
            if (element == null)
            {
                throw new InvalidOperationException("This behavior can be attached to a TextBox item only.");
            }

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                element.LostFocus += OnPreviewLostFocus;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                element.LostFocus -= OnPreviewLostFocus;
            }
        }

        /// <summary>
        /// Event handler
        /// </summary>
        private static void OnPreviewLostFocus(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement) sender;
            ICommand command = (ICommand) element.GetValue(OnLostFocusProperty);
            if (command != null)
            {
                command.Execute(e);
            }
        }
    }

}
