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
        public static readonly DependencyProperty Command =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickCommand),
                new PropertyMetadata(AttachOrRemoveDoubleClickEvent));

        /// <summary>
        /// Get the double click command
        /// </summary>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(Command);
        }

        /// <summary>
        /// Set the double click command
        /// </summary>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(Command, value);
        }

        /// <summary>
        /// Attach or Remove double click event
        /// </summary>
        private static void AttachOrRemoveDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is Control control)
            {
                if (args.OldValue == null && args.NewValue != null)
                {
                    control.MouseDoubleClick += ExecuteCommand;
                }
                else if (args.OldValue != null && args.NewValue == null)
                {
                    control.MouseDoubleClick -= ExecuteCommand;
                }
            }
        }

        /// <summary>
        /// Run double click command
        /// </summary>
        private static void ExecuteCommand(object sender, MouseButtonEventArgs args)
        {
            ICommand command = GetCommand(sender as DependencyObject);
            
            command?.Execute(null);
        }
    }
}
