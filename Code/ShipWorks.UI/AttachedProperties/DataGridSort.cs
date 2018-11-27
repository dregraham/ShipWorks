using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Collections;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Handle custom sorting on a data grid
    /// </summary>
    public class DataGridSort
    {
        /// <summary>
        /// CurrentSort dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentSortProperty =
            DependencyProperty.RegisterAttached("CurrentSort", typeof(IBasicSortDefinition), typeof(DataGridSort),
                new PropertyMetadata(null, OnCurrentSortChanged));

        /// <summary>
        /// Wires up the event handler for when the url is clicked
        /// </summary>
        private static void OnCurrentSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = d as DataGrid;

            if (dataGrid == null)
            {
                return;
            }

            var dataGridItemsSourceProperty = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            dataGridItemsSourceProperty?.RemoveValueChanged(dataGrid, OnDataGridItemsSourceChanged);

            dataGrid.Sorting -= OnDataGridSorting;

            if (e.NewValue is IBasicSortDefinition sortDefinition)
            {
                SetSortDirection(dataGrid, sortDefinition);

                dataGrid.Sorting += OnDataGridSorting;
                dataGridItemsSourceProperty?.AddValueChanged(dataGrid, OnDataGridItemsSourceChanged);
            }
        }

        /// <summary>
        /// Handle when the items source has changed so we can show the sort correctly
        /// </summary>
        private static void OnDataGridItemsSourceChanged(object sender, EventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                SetSortDirection(dataGrid, GetCurrentSort(dataGrid));
            }
        }

        /// <summary>
        /// Set the sort direction in the grid columns
        /// </summary>
        private static void SetSortDirection(DataGrid dataGrid, IBasicSortDefinition sortDefinition) =>
            dataGrid.Columns.ForEach(x => x.SortDirection = x.SortMemberPath.EndsWith(sortDefinition.Name) ? sortDefinition.Direction : (ListSortDirection?) null);

        /// <summary>
        /// Handle when the grid is sorting
        /// </summary>
        private static void OnDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                SetCurrentSort(dataGrid, new BasicSortDefinition(e.Column.SortMemberPath, e.Column.SortDirection));
            }

            e.Handled = true;
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static IBasicSortDefinition GetCurrentSort(DependencyObject d) => (IBasicSortDefinition) d.GetValue(CurrentSortProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetCurrentSort(DependencyObject d, IBasicSortDefinition value) => d.SetValue(CurrentSortProperty, value);
    }
}
