using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ShipWorks.UI.Behaviors
{
    /// <summary>
    /// Behavior to close a window when a button is clicked
    /// </summary>
    public class WindowCloseOnClickBehavior : Behavior<Button>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Click += OnClick;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnClick;
        }
        
        /// <summary>
        /// Close the window containing the button that was clicked 
        /// </summary>
        private static void OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }

            Window window = Window.GetWindow(button);
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}