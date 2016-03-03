using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Combo box that has ShipWorks customizations
    /// </summary>
    public class ShipWorksComboBox : ComboBox
    {
        static readonly PropertyMetadata existingMetadata = SelectedValueProperty.GetMetadata(typeof(Selector));

        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedIndexWhenNullProperty =
            DependencyProperty.RegisterAttached("SelectedIndexWhenNull", typeof(RelativeIndex),
                typeof(ShipWorksComboBox), new PropertyMetadata(RelativeIndex.None));

        /// <summary>
        /// Static constructor
        /// </summary>
        static ShipWorksComboBox()
        {
            SelectedValueProperty.OverrideMetadata(typeof(ShipWorksComboBox),
                new FrameworkPropertyMetadata(null, new CoerceValueCallback(HandleValueCoercion)));
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static RelativeIndex GetSelectedIndexWhenNull(DependencyObject d) =>
            (RelativeIndex) d.GetValue(SelectedIndexWhenNullProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetSelectedIndexWhenNull(DependencyObject d, RelativeIndex value) =>
            d.SetValue(SelectedIndexWhenNullProperty, value);

        /// <summary>
        /// Handle value coercion when the selected value of a combo box is changed
        /// </summary>
        private static object HandleValueCoercion(DependencyObject d, object value)
        {
            object newValue = existingMetadata.CoerceValueCallback(d, value);
            ComboBox comboBox = (ComboBox) d;

            if (WasValueCoercedToNull(value, newValue) || comboBox.SelectedItem == null)
            {
                SelectNewValue(comboBox);
            }

            return newValue;
        }

        /// <summary>
        /// Was the value coerced to null instead of starting as null
        /// </summary>
        private static bool WasValueCoercedToNull(object value, object newValue)
        {
            return value != null && newValue == null;
        }

        /// <summary>
        /// Select a new value if necessary
        /// </summary>
        private static void SelectNewValue(ComboBox combo)
        {
            RelativeIndex relativeIndex = GetSelectedIndexWhenNull(combo);
            if (relativeIndex == RelativeIndex.None || !combo.HasItems)
            {
                return;
            }

            // Using BeginInvoke will put selection of the new item on the event loop, which will let this
            // coercion complete before changing the value to something valid
            combo.Dispatcher.BeginInvoke((Action) (() =>
            {
                combo.SelectedItem = relativeIndex == RelativeIndex.Last ?
                    combo.Items.OfType<object>().Last() :
                    combo.Items.OfType<object>().First();
            }), null);
        }
    }
}
