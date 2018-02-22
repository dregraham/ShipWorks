using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace ShipWorks.UI.Behaviors.Sort
{
    /// <summary>
    /// Logic for sorting a ListView
    /// </summary>
    public class Sorting
    {
        private ListSortDirection sortDirection;
        private GridViewColumnHeader sortColumn;

        /// <summary>
        /// Field used for sorting
        /// </summary>
        public static readonly DependencyProperty FieldProperty =
            DependencyProperty.RegisterAttached("Field", typeof(string), typeof(Sorting));

        /// <summary>
        /// Perform sort
        /// </summary>
        public void Sort(object columnHeader, CollectionView list)
        {
            string column = SetAdorner(columnHeader);

            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(new SortDescription(column, sortDirection));
        }

        /// <summary>
        /// Set the Adorner
        /// </summary>
        private string SetAdorner(object columnHeader)
        {
            if (!(columnHeader is GridViewColumnHeader column))
            {
                return null;
            }

            // Remove arrow from previously sorted header
            if (sortColumn != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(sortColumn);
                Adorner adorner = adornerLayer.GetAdorners(sortColumn)?.FirstOrDefault();
                if (adorner!=null)
                {
                    adornerLayer.Remove(adorner);
                }
            }

            if (ReferenceEquals(sortColumn, column))
            {
                // Toggle sorting direction
                sortDirection = sortDirection == ListSortDirection.Ascending ?
                    ListSortDirection.Descending :
                    ListSortDirection.Ascending;
            }
            else
            {
                sortColumn = column;
                sortDirection = ListSortDirection.Ascending;
            }

            SortingAdorner sortingAdorner = new SortingAdorner(column, sortDirection);
            AdornerLayer.GetAdornerLayer(column).Add(sortingAdorner);

            string columnToSortBy = string.Empty;

            // if binding is used and property name doesn't match header content
            if (sortColumn.Column.DisplayMemberBinding is Binding b)
            {
                columnToSortBy = b.Path.Path;
            }

            if (string.IsNullOrWhiteSpace(columnToSortBy))
            {
                columnToSortBy = GetField(sortColumn.Column);
            }

            return columnToSortBy;
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static string GetField(DependencyObject d) => (string) d.GetValue(FieldProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetField(DependencyObject d, string value) => d.SetValue(FieldProperty, value);
    }
}