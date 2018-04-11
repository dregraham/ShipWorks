using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property to issue a command when the control is double clicked
    /// </summary>
    [Obfuscation(Exclude = true)]
    public static class DoubleClickCommand
    {
        /// <summary>
        /// Associate a command with a double click
        /// </summary>
        public static readonly DependencyProperty DoubleClickProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(DoubleClickCommand),
                new PropertyMetadata(AttachOrRemoveDoubleClickEvent));

        /// <summary>
        /// Get the double click command
        /// </summary>
        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(DoubleClickProperty);
        }

        /// <summary>
        /// Set the double click command
        /// </summary>
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickProperty, value);
        }

        /// <summary>
        /// Attach or Remove double click event
        /// </summary>
        public static void AttachOrRemoveDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is Control control)
            {
                if (args.OldValue == null && args.NewValue != null)
                {
                    control.MouseDoubleClick += ExecuteDoubleClick;
                }
                else if (args.OldValue != null && args.NewValue == null)
                {
                    control.MouseDoubleClick -= ExecuteDoubleClick;
                }
            }
        }

        /// <summary>
        /// Run double click command
        /// </summary>
        private static void ExecuteDoubleClick(object sender, MouseButtonEventArgs args)
        {
            DependencyObject obj = sender as DependencyObject;
            ICommand cmd = (ICommand) obj?.GetValue(DoubleClickProperty);
            cmd?.Execute(obj);
        }
    }
}
