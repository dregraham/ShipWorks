using System.Windows.Forms;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Class to fetch missing order source IDs from the Hub
    /// </summary>
    public interface IAmazonOrderSourceIdFetcher
    {
        /// <summary>
        /// Fetch order source IDs from the Hub
        /// </summary>
        void FetchOrderSourceIds(IWin32Window owner);
    }
}
