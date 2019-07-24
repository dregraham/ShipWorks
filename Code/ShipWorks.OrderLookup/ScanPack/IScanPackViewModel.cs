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
        Task Load(string scannedText);

        /// <summary>
        /// Load an order
        /// </summary>
        Task Load(OrderEntity order);

        /// <summary>
        /// Reset the control
        /// </summary>
        void Reset();
    }
}
