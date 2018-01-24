using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Search;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Factory for creating OnDemandDownloaders
    /// </summary>
    [Component]
    public class OnDemandDownloaderFactory : IOnDemandDownloaderFactory
    {
        private readonly IDownloadManager downloadManager;
        private readonly ISingleScanOrderShortcut orderShortcut;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloaderFactory(IDownloadManager downloadManager,
            ISingleScanOrderShortcut orderShortcut)
        {
            this.downloadManager = downloadManager;
            this.orderShortcut = orderShortcut;
        }

        /// <summary>
        /// Create an OnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateOnDemandDownloader() => 
            new OnDemandDownloader(downloadManager);

        /// <summary>
        /// Create a SingleScanOnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateSingleScanOnDemandDownloader() =>
            new SingleScanOnDemandDownloader(CreateOnDemandDownloader(), orderShortcut);
    }
}