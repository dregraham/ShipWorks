using System.Reflection;
using System.Windows;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property allowing a control to be used to drag a window
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class Drag
    {
        /// <summary>
        /// The DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ParentWindowProperty =
            DependencyProperty.RegisterAttached("ParentWindow", typeof(bool), typeof(FrameworkElement),
                new PropertyMetadata(false, WindowChanged));

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static bool GetWindow(DependencyObject d) => (bool) d.GetValue(ParentWindowProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetWindow(DependencyObject d, bool value) => d.SetValue(ParentWindowProperty, value);

        /// <summary>
        /// Wires up the event handler for when the element is clicked
        /// </summary>
        private static void WindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;

            if (element == null)
            {
                return;
            }

            element.MouseLeftButtonDown -= OnMouseLeftButtonDown;

            if ((bool) e.NewValue)
            {
                element.MouseLeftButtonDown += OnMouseLeftButtonDown;
            }
        }

        /// <summary>
        /// Handle when the title border is clicked so that we can allow the window to be dragged
        /// </summary>
        private static void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            Window.GetWindow(sender as DependencyObject)?.DragMove();
    }
}
