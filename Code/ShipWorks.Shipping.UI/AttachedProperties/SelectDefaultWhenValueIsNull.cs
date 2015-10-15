using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ShipWorks.Shipping.UI.AttachedProperties
{
    public enum RelativeIndex
    {
        None,
        First,
        Last
    }

    /// <summary>
    /// Update a binding when a specific message is received
    /// </summary>
    public class SelectDefaultWhenValueIsNull : DependencyObject
    {
        static readonly PropertyMetadata existingMetadata = Selector.SelectedValueProperty.GetMetadata(typeof(Selector));

        /// <summary>
        /// Static constructor
        /// </summary>
        static SelectDefaultWhenValueIsNull()
        {
            Selector.SelectedValueProperty.OverrideMetadata(typeof(ComboBox),
                new FrameworkPropertyMetadata(null, new CoerceValueCallback(HandleValueCoersion)));
        }

        /// <summary>
        /// Handle value coersion when the selected value of a combobox is changed
        /// </summary>
        private static object HandleValueCoersion(DependencyObject d, object value)
        {
            object newValue = existingMetadata.CoerceValueCallback(d, value);

            if (value != null && newValue == null)
            {
                SelectNewValue(d as ComboBox);
            }

            return newValue;
        }

        /// <summary>
        /// Select a new value if necessary
        /// </summary>
        private static void SelectNewValue(ComboBox combo)
        {
            if (combo == null)
            {
                return;
            }

            RelativeIndex relativeIndex = GetRelativeIndex(combo);
            if (relativeIndex == RelativeIndex.None || !combo.HasItems)
            {
                return;
            }

            // Using BeginInvoke will put selection of the new item on the event loop, which will let this
            // coersion complete before changing the value to something valid
            combo.Dispatcher.BeginInvoke((Action)(() =>
            {
                combo.SelectedItem = relativeIndex == RelativeIndex.Last ?
                    combo.Items.OfType<object>().Last() :
                    combo.Items.OfType<object>().First();
            }), null);
        }

        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty RelativeIndexProperty = DependencyProperty.RegisterAttached("RelativeIndex", typeof(RelativeIndex),
                typeof(SelectDefaultWhenValueIsNull), new PropertyMetadata(RelativeIndex.None));

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static RelativeIndex GetRelativeIndex(DependencyObject d) => (RelativeIndex)d.GetValue(RelativeIndexProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetRelativeIndex(DependencyObject d, RelativeIndex value) => d.SetValue(RelativeIndexProperty, value);
    }
}
