using System.Runtime.Remoting.Messaging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloaderFactory(IDownloadManager downloadManager,
            ISingleScanOrderShortcut orderShortcut,
            IMessageHelper messageHelper)
        {
            this.downloadManager = downloadManager;
            this.orderShortcut = orderShortcut;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Create an OnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateOnDemandDownloader() => 
            new OnDemandDownloader(downloadManager, messageHelper);

        /// <summary>
        /// Create a SingleScanOnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateSingleScanOnDemandDownloader() =>
            new SingleScanOnDemandDownloader(CreateOnDemandDownloader(), orderShortcut);
    }
}