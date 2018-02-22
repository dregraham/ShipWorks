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
        /// Attaches header click handler
        /// </summary>
        protected override void OnAttached() => 
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));

        /// <summary>
        /// Detaches headerclick handler
        /// </summary>
        protected override void OnDetaching() => 
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClicked));

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
    }
}
