using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Set focus on a control
    /// </summary>
    public partial class Focus
    {
        public static readonly DependencyProperty StartupProperty =
            DependencyProperty.RegisterAttached("Startup", typeof(bool), typeof(Focus), new PropertyMetadata(StartupSetCallback));

        /// <summary>
        /// Set the startup property
        /// </summary>
        public static void SetStartup(UIElement element, bool value) =>
            element.SetValue(StartupProperty, value);

        /// <summary>
        /// Get the startup property
        /// </summary>
        public static bool GetStartup(UIElement element) =>
            (bool) element.GetValue(StartupProperty);

        /// <summary>
        /// Handle when the startup property changes
        /// </summary>
        private static void StartupSetCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var frameworkElement = (FrameworkElement) dependencyObject;
           

            if (frameworkElement == null || !(args.NewValue is bool newValue))
            {
                return;
            }

            if(newValue)
            {
                frameworkElement.Loaded += (s, e) => HandleFrameworkElementLoaded(frameworkElement);
            }
        }

        /// <summary>
        /// Handle when the framework element is loaded
        /// </summary>
        private static void HandleFrameworkElementLoaded(FrameworkElement target)
        {
            Keyboard.Focus(target);

            if (target is TextBox textBox)
            {
                textBox.Select(textBox.Text.Length, 0);
            }
        }
    }
}
