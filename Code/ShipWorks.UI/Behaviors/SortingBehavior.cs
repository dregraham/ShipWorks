using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using ShipWorks.UI.Behaviors.Sort;

namespace ShipWorks.UI.Behaviors
{
    /// <summary>
    /// Behavior to make a ListView sortable
    /// </summary>
    public class SortingBehavior : Behavior<ListView>
    {
        private readonly Sorting sorting;

        /// <summary>
        /// Constructor
        /// </summary>
        public SortingBehavior() => sorting = new Sorting();

        /// <summary>
        /// The index of the Default Sort Column
        /// </summary>
        public int DefaultSortColumnIndex { get; set; } = 0;

        /// <summary>
        /// Attaches header click handler
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));
            AssociatedObject.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnListViewLoaded));
        }

        /// <summary>
        /// Detaches headerclick handler
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));
            AssociatedObject.RemoveHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnListViewLoaded));
        }

        /// <summary>
        /// Sort when header is clicked
        /// </summary>
        private void OnColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView)
            {
                sorting.Sort(e.OriginalSource, listView.Items);
            }
        }

        /// <summary>
        /// Apply default sort
        /// </summary>
        private void OnListViewLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView)
            {
                object columnHeader = ((GridView) listView.View).Columns[DefaultSortColumnIndex].Header;
                if (columnHeader is GridViewColumnHeader gridViewColumnHeader)
                {
                    sorting.Sort(gridViewColumnHeader, AssociatedObject.Items);
                }
                else
                {
                    Debug.Fail($"The header for column {DefaultSortColumnIndex} must be defined as a " +
                               "GridViewColumnHeader and not in the header field of the column object.");
                }
            }
        }
    }
}
