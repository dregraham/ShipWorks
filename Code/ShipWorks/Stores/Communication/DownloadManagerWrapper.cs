using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Interface that represents the DownloadManager
    /// </summary>
    public class DownloadManagerWrapper : IDownloadManager
    {
        /// <summary>
        /// Download the order number from all stores
        /// </summary>
        public Task<IResult> Download(string orderNumber)
        {
            return DownloadManager.Download(orderNumber);
        }
    }
}
