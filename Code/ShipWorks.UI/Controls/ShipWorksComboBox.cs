using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SD.LLBLGen.Pro.ORMSupportClasses;

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

        /// <summary>
        /// Override OnIsKeyboardFocusWithinChanged so that we can handle the issue where the user
        /// clicks a letter inside the ComboBox and then hits TAB.  MS knows about this:
        /// https://connect.microsoft.com/VisualStudio/feedback/details/1660886/system-windows-controls-combobox-coerceisselectionboxhighlighted-bug
        /// </summary>
        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            // This is the nasty hack.  See the link above as to why.
            System.Reflection.FieldInfo fieldInfo = typeof(ComboBox)
                .GetField("_highlightedInfo",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            fieldInfo?.SetValue(this, null);

            base.OnIsKeyboardFocusWithinChanged(e);
        }
    }
}
