using Interapptive.Shared.Messaging;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.AttachedProperties
{
    public enum RelativeIndex
    {
        First,
        Last
    }

    /// <summary>
    /// Update a binding when a specific message is received
    /// </summary>
    public class SelectDefaultWhenValueIsNull : DependencyObject
    {
        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty RelativeIndexProperty = DependencyProperty.RegisterAttached("RelativeIndex", typeof(RelativeIndex),
                typeof(SelectDefaultWhenValueIsNull), new PropertyMetadata(OnRelativeIndexChanged));

        /// <summary>
        /// Handle when the specific message type changes
        /// </summary>
        private static void OnRelativeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox control = d as ComboBox;
            if (control == null)
            {
                return;
            }

            if (e.NewValue != null && e.OldValue == null)
            {
                control.SelectionChanged += OnControlSelectionChanged;
            }
            else if (e.NewValue == null && e.OldValue != null)
            {
                control.SelectionChanged -= OnControlSelectionChanged;
            }
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static RelativeIndex GetRelativeIndex(DependencyObject d)
        {
            return (RelativeIndex)d.GetValue(RelativeIndexProperty);
        }

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetRelativeIndex(DependencyObject d, RelativeIndex value)
        {
            d.SetValue(RelativeIndexProperty, value);
        }

        /// <summary>
        /// Select a relative position in the list when the current value is null
        /// </summary>
        private static void OnControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            if (e.RemovedItems.OfType<object>().Any() && !e.AddedItems.OfType<object>().Any())
            {
                control.SelectedIndex = GetRelativeIndex(control) == RelativeIndex.Last ? control.Items.Count - 1 : 0;
            }
        }
    }
}
