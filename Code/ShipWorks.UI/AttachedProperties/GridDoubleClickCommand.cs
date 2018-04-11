using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property to issue a command when an item in the grid is double clicked
    /// </summary>
    [Obfuscation(Exclude = true)]
    public static class GridDoubleClickCommand
    {
        /// <summary>
        /// Associate a command with a double click
        /// </summary>
        public static readonly DependencyProperty DataGridDoubleClickProperty =
            DependencyProperty.RegisterAttached("DataGridDoubleClickCommand", typeof(ICommand), typeof(GridDoubleClickCommand),
                new PropertyMetadata(AttachOrRemoveDataGridDoubleClickEvent));

        /// <summary>
        /// Get the double click command
        /// </summary>
        public static ICommand GetDataGridDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(DataGridDoubleClickProperty);
        }

        /// <summary>
        /// Set the double click command
        /// </summary>
        public static void SetDataGridDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DataGridDoubleClickProperty, value);
        }

        /// <summary>
        /// Attach or Remove double click event
        /// </summary>
        public static void AttachOrRemoveDataGridDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is DataGrid dataGrid)
            {
                if (args.OldValue == null && args.NewValue != null)
                {
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
                }
                else if (args.OldValue != null && args.NewValue == null)
                {
                    dataGrid.MouseDoubleClick -= ExecuteDataGridDoubleClick;
                }
            }
        }

        /// <summary>
        /// Run double click command
        /// </summary>
        private static void ExecuteDataGridDoubleClick(object sender, MouseButtonEventArgs args)
        {
            DependencyObject obj = sender as DependencyObject;
            ICommand cmd = (ICommand) obj?.GetValue(DataGridDoubleClickProperty);

            if (cmd?.CanExecute(obj) == true)
            {
                cmd.Execute(obj);
            }
        }
    }
}
