using System.Threading.Tasks;

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
        Task Load(string orderNumber);
    }
}
