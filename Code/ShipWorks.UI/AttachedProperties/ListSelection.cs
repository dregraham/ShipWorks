using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Collections;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property for stopping a list from having no selected row
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ListSelection
    {
        /// <summary>
        /// The DependencyProperty
        /// </summary>
        public static readonly DependencyProperty AllowEmptySelectionProperty =
            DependencyProperty.RegisterAttached("AllowEmptySelection", typeof(bool), typeof(ListSelection),
                new PropertyMetadata(true, OnAllowEmptySelectionChanged));

        /// <summary>
        /// Wires up the event handler for when the selection changes
        /// </summary>
        private static void OnAllowEmptySelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox list = d as ListBox;

            if (list == null)
            {
                return;
            }

            list.SelectionChanged -= ListBoxOnSelectionChanged;

            if (!((bool) e.NewValue))
            {
                list.SelectionChanged += ListBoxOnSelectionChanged;
            }
        }

        /// <summary>
        /// Ensure that the selection cannot be empty
        /// </summary>
        private static void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list.SelectedItem == null && e.RemovedItems.OfType<object>().Any())
            {
                if (list.SelectionMode == SelectionMode.Single)
                {
                    list.SelectedItem = e.RemovedItems.OfType<object>().First();
                }
                else
                {
                    foreach (object item in e.RemovedItems)
                    {
                        list.SelectedItems.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static bool GetAllowEmptySelection(DependencyObject d) => (bool) d.GetValue(AllowEmptySelectionProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetAllowEmptySelection(DependencyObject d, bool value) => d.SetValue(AllowEmptySelectionProperty, value);
    }
}
