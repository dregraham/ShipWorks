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
        /// Constructor
        /// </summary>
        public SingleScanOnDemandDownloader(IOnDemandDownloader onDemandDownloader,
            ISingleScanOrderShortcut orderShortcut)
        {
            this.orderShortcut = orderShortcut;
            this.onDemandDownloader = onDemandDownloader;
        }

        /// <summary>
        /// If order number is not a shortcut, we delegate order downloading to the onDemandDownloader
        /// If it is an order shortcut, simply return CompletedTask
        /// </summary>
        public Task Download(string orderNumber)
        {
            // Is the orderNumber a Singel Scan Order Shortcut?
            if (orderShortcut.AppliesTo(orderNumber))
            {
                return Task.CompletedTask;
            }

            // We know the order Number is not a shortcut, so attempt to download by
            // delegating to the onDemandDownloader
            return onDemandDownloader.Download(orderNumber);
        }
    }
}