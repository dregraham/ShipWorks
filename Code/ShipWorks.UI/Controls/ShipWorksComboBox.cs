using System;
using System.ComponentModel;
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
        private bool _suppressSelectionChangedUpdatesRebind = false;

        /// <summary>
        /// Dependency property for what value to use when selected value is null
        /// </summary>
        public static readonly DependencyProperty SelectedIndexWhenNullProperty =
            DependencyProperty.RegisterAttached("SelectedIndexWhenNull", typeof(RelativeIndex),
                typeof(ShipWorksComboBox), new PropertyMetadata(RelativeIndex.None));

        /// <summary>
        /// Proper selected value dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperProperty =
           DependencyProperty.RegisterAttached(
               "SelectedValueProper",
               typeof(object),
               typeof(ShipWorksComboBox),
               new PropertyMetadata((o, dp) =>
               {
                   var comboBox = o as ShipWorksComboBox;
                   if (comboBox == null)
                   {
                       return;
                   }
                      
                   comboBox.SetSelectedValueSuppressingChangeEventProcessing(dp.NewValue);
               }));

        public ShipWorksComboBox()
        {
            SelectionChanged += ComboBoxEx_SelectionChanged;
        }

        /// <summary>
        /// The correct value of selected property
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public object SelectedValueProper
        {
            get { return (object) GetValue(SelectedValueProperProperty); }
            set { SetValue(SelectedValueProperProperty, value); }
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
                // If someone else has successfully changed the selected item before we had a chance to update it
                // (because we delayed execution by dispatching), don't change the value. We got into a situation
                // where the value would be null first, then set correctly. The null would schedule this action,
                // then the correct value would be set. Then this action would be run and wipe out the correct value.
                if (combo.SelectedItem == null && combo.Items.OfType<object>().Any())
                {
                    combo.SelectedItem = relativeIndex == RelativeIndex.Last ?
                        combo.Items.OfType<object>().Last() :
                        combo.Items.OfType<object>().First();
                }
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

        /// <summary>
        /// Updates the current selected item when the collection has changed.
        /// </summary>
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Must re-apply value here because the combobox has a bug that 
            // despite the fact that the binding still exists, it doesn't 
            // re-evaluate and subsequently drops the binding on the change event
            SetSelectedValueSuppressingChangeEventProcessing(SelectedValueProper);
        }

        /// <summary>
        /// Handle the selection changed event.
        /// </summary>
        private void ComboBoxEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Avoid recursive stack overflow
            if (_suppressSelectionChangedUpdatesRebind)
                return;

            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                if (SelectedValue == null)
                {
                    SelectNewValue(this);
                }
                SelectedValueProper = SelectedValue;
            }
            // Do not apply the value if no items are selected (ie. the else)
            // because that just passes on the null-value bug from the combobox
        }

        /// <summary>
        /// Sets the selected value suppressing change event processing.
        /// </summary>
        private void SetSelectedValueSuppressingChangeEventProcessing(object newSelectedValue)
        {
            try
            {
                _suppressSelectionChangedUpdatesRebind = true;
                SelectedValue = newSelectedValue;
            }
            finally
            {
                _suppressSelectionChangedUpdatesRebind = false;
            }
        }
    }
}
