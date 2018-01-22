using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Search;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloads an order from SingleScan
    /// </summary>
    public class SingleScanOnDemandDownloader : IOnDemandDownloader
    {
        private readonly ISingleScanOrderShortcut orderShortcut;
        private readonly IOnDemandDownloader onDemandDownloader;

        /// <summary>
        /// Download order from SingleScan
        /// </summary>
        public SingleScanOnDemandDownloader(IOnDemandDownloader onDemandDownloader,
            ISingleScanOrderShortcut orderShortcut)
        {
            this.orderShortcut = orderShortcut;
            this.onDemandDownloader = onDemandDownloader;
        }

        /// <summary>
        /// Download order
        /// </summary>
        public Task Download(string orderNumber)
        {
            if (orderShortcut.AppliesTo(orderNumber))
            {
                return Task.CompletedTask;
            }

            return onDemandDownloader.Download(orderNumber);
        }
    }
}