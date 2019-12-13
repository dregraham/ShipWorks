using System;
using System.Windows.Input;
using ShipWorks.OrderLookup.ScanToShip;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    /// <summary>
    /// View model for the OrderLookupSearchControl
    /// </summary>
    public interface IOrderLookupSearchViewModel : IDisposable
    {
        /// <summary>
        /// Command to create a label
        /// </summary>
        ICommand CreateLabelCommand { get; set; }

        /// <summary>
        /// Command for getting orders
        /// </summary>
        ICommand GetOrderCommand { get; set; }

        /// <summary>
        /// Order Number to search for
        /// </summary>
        string OrderNumber { get; set; }

        /// <summary>
        /// Command for resetting the order
        /// </summary>
        ICommand ResetCommand { get; set; }

        /// <summary>
        /// Error message to display when a error occurs while searching
        /// </summary>
        string SearchMessage { get; set; }

        /// <summary>
        /// Currently selected Scan to Ship tab
        /// </summary>
        ScanToShipTab SelectedTab { get; set; }

        /// <summary>
        /// The order lookup Shipment Model
        /// </summary>
        IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Show the create label button?
        /// </summary>
        bool ShowCreateLabel { get; set; }

        /// <summary>
        /// Indicates whether or not an error has occurred while searching.
        /// </summary>
        bool ShowSearchMessage { get; set; }

        /// <summary>
        /// Clears the order error
        /// </summary>
        void ClearSearchMessage(OrderClearReason reason);
    }
}
