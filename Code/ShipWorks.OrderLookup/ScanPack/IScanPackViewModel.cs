using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Viewmodel for ScanPackView
    /// </summary>
    public interface IScanPackViewModel
    {
        /// <summary>
        /// The Scan Header
        /// </summary>
        string ScanHeader { get; set; }

        /// <summary>
        /// The Scan Footer
        /// </summary>
        string ScanFooter { get; set; }

        /// <summary>
        /// Is the view enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Can the view accept focus
        /// </summary>
        Func<bool> CanAcceptFocus { get; set; }

        /// <summary>
        /// Process a line item
        /// </summary>
        void ProcessItemScan(string scannedText);

        /// <summary>
        /// Load an order
        /// </summary>
        Task LoadOrder(OrderEntity order);

        /// <summary>
        /// Reset the control
        /// </summary>
        void Reset();

        /// <summary>
        /// Current state of the view model
        /// </summary>
        ScanPackState State { get; set; }

        /// <summary>
        /// Event handler for when state changes
        /// </summary>
        event EventHandler OrderVerified;
    }
}
