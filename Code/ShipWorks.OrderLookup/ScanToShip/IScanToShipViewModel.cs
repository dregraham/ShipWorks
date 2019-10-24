using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;
using ShipWorks.OrderLookup.ScanPack;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// ScanToShip ViewModel
    /// </summary>
    public interface IScanToShipViewModel
    {
        /// <summary>
        /// IsPackTabActive
        /// </summary>
        bool IsPackTabActive { get; }

        /// <summary>
        /// OrderLookupSearch ViewModel
        /// </summary>
        OrderLookupSearchViewModel OrderLookupSearchViewModel { get; }
        
        /// <summary>
        /// OrderLookup ViewModel
        /// </summary>
        MainOrderLookupViewModel OrderLookupViewModel { get; }

        /// <summary>
        /// ScanPack ViewModel
        /// </summary>
        IScanPackViewModel ScanPackViewModel { get; }

        /// <summary>
        /// Returns the numeric value of the selected tab
        /// </summary>
        int SelectedTab { get; set; }
    }
}
