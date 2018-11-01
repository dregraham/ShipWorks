using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.Selectors
{
    /// <summary>
    /// Select a style based on the row index
    /// </summary>
    public class ListStyleSelector : StyleSelector
    {
        /// <summary>
        /// Style that will be used for the first row
        /// </summary>
        public Style First { get; set; }

        /// <summary>
        /// Style that will be used as the default for rows
        /// </summary>
        public Style Default { get; set; }

        /// <summary>
        /// Select the style that should be used
        /// </summary>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var listView = ItemsControl.ItemsControlFromItemContainer(container);
            int index = listView.ItemContainerGenerator.IndexFromContainer(container);

            return index == 0 ? First : Default;
        }
    }
}
