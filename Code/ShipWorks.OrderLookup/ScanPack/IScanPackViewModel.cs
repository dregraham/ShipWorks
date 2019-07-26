using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Viewmodel for ScanPackView
    /// </summary>
    public interface IScanPackViewModel
    {
        /// <summary>
        /// Load an order
        /// </summary>
        Task ProcessScan(string scannedText);

        /// <summary>
        /// Load an order
        /// </summary>
        Task LoadOrder(OrderEntity order);

        /// <summary>
        /// Reset the control
        /// </summary>
        void Reset();
    }
}
