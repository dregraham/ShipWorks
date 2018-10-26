using System.Collections.ObjectModel;
using System.Windows;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Represents the main order lookup view model
    /// </summary>
    public interface IMainOrderLookupViewModel
    {
        /// <summary>
        /// The left column of panels
        /// </summary>
        ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> LeftColumn { get; set; }

        /// <summary>
        /// The middle column of panels
        /// </summary>
        ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> MiddleColumn { get; set; }

        /// <summary>
        /// The rights column of panels
        /// </summary>
        ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> RightColumn { get; set; }

        /// <summary>
        /// Width of the left column
        /// </summary>
        GridLength LeftColumnWidth { get; set; }

        /// <summary>
        /// Width of the middle column
        /// </summary>
        GridLength MiddleColumnWidth { get; set; }
    }
}