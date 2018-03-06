using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Set focus on a control on load
    /// </summary>
    public class Focus
    {
        public static readonly DependencyProperty StartupProperty =
            DependencyProperty.RegisterAttached("Startup", typeof(FrameworkElement), typeof(Focus), new PropertyMetadata(StartupSetCallback));

        /// <summary>
        /// Set the startup property
        /// </summary>
        public static void SetStartup(UIElement element, FrameworkElement value) =>
            element.SetValue(StartupProperty, value);

        /// <summary>
        /// Get the startup property
        /// </summary>
        public static FrameworkElement GetStartup(UIElement element) =>
            (FrameworkElement) element.GetValue(StartupProperty);

        /// <summary>
        /// Handle when the startup property changes
        /// </summary>
        private static void StartupSetCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var frameworkElement = (FrameworkElement) dependencyObject;
            var target = GetStartup(frameworkElement);

            if (target == null)
            {
                return;
            }

            frameworkElement.Loaded += (s, e) => HandleFrameworkElementLoaded(target);
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
