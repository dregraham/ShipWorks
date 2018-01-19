using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Search;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloads an order from SingleScan
    /// </summary>
    [KeyedComponent(typeof(IOnDemandDownloader), OnDemandDownloaderType.SingleScanOnDemandDownloader)]
    public class SingleScanOnDemandDownloader : IOnDemandDownloader
    {
        private readonly ISingleScanOrderShortcut orderShortCut;
        private readonly IOnDemandDownloader onDemandDownloader;

        /// <summary>
        /// Download order from SingleScan
        /// </summary>
        public SingleScanOnDemandDownloader(IIndex<OnDemandDownloaderType, IOnDemandDownloader> onDemandDownloader,
            ISingleScanOrderShortcut orderShortCut)
        {
            this.orderShortCut = orderShortCut;
            this.onDemandDownloader = onDemandDownloader[OnDemandDownloaderType.OnDemandDownloader];
        }

        /// <summary>
        /// Download order
        /// </summary>
        public Task Download(string orderNumber)
        {
            if (orderShortCut.AppliesTo(orderNumber))
            {
                return Task.CompletedTask;
            }

            return onDemandDownloader.Download(orderNumber);
        }
    }
}