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
        IOrderLookupSearchViewModel SearchViewModel { get; }
        
        /// <summary>
        /// OrderLookup ViewModel
        /// </summary>
        MainOrderLookupViewModel OrderLookupViewModel { get; }

        /// <summary>
        /// ScanPack ViewModel
        /// </summary>
        IScanPackViewModel ScanPackViewModel { get; }

        /// <summary>
        /// Has the current order been verified
        /// </summary>
        bool IsOrderVerified { get; set; }

        /// <summary>
        /// Has the current order been processed
        /// </summary>
        bool IsOrderProcessed { get; set; }

        /// <summary>
        /// Whether or not to show the verification error
        /// </summary>
        bool ShowOrderVerificationError { get; set; }

        /// <summary>
        /// Returns the numeric value of the selected tab
        /// </summary>
        int SelectedTab { get; set; }

        /// <summary>
        /// Show the order verification error when appropriate
        /// </summary>
        void UpdateOrderVerificationError();

        /// <summary>
        /// Reset the state
        /// </summary>
        void Reset();
    }
}
