using System;
using System.Windows.Input;
using ShipWorks.OrderLookup.ScanToShip;

namespace ShipWorks.OrderLookup.Controls.OrderLookupSearchControl
{
    public interface IOrderLookupSearchViewModel : IDisposable
    {
        ICommand CreateLabelCommand { get; set; }
        ICommand GetOrderCommand { get; set; }
        string OrderNumber { get; set; }
        ICommand ResetCommand { get; set; }
        string SearchMessage { get; set; }
        ScanToShipTab SelectedTab { get; set; }
        IOrderLookupShipmentModel ShipmentModel { get; }
        bool ShowCreateLabel { get; set; }
        bool ShowSearchMessage { get; set; }

        void ClearOrderError(OrderClearReason reason);
    }
}