using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ShipWorks.UI.Behaviors
{
    /// <summary>
    /// Behavior to allow binding on a command to run after a ComboBoxSelection changes.
    /// </summary>
    [Obfuscation(Exclude=true)]
    public class ComboBoxSelectionChangedBehavior : Behavior<ComboBox>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(ComboBoxSelectionChangedBehavior), new PropertyMetadata());

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            ComboBox listBox = AssociatedObject;
            listBox.SelectionChanged += OnSelectionChanged;
        }

        /// <summary>
        /// Fires when ComboBox Selection Changes
        /// </summary>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            Command.Execute(null);
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            ComboBox listBox = AssociatedObject;
            listBox.SelectionChanged -= OnSelectionChanged;
        }
    }
}
