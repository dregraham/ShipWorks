using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using log4net;
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
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloaderFactory(IDownloadManager downloadManager,
            ISingleScanOrderShortcut orderShortcut,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory)
        {
            this.downloadManager = downloadManager;
            this.orderShortcut = orderShortcut;
            this.messageHelper = messageHelper;
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Create an OnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateOnDemandDownloader() => 
            new OnDemandDownloader(downloadManager, messageHelper, logFactory);

        /// <summary>
        /// Create a SingleScanOnDemandDownloader
        /// </summary>
        public IOnDemandDownloader CreateSingleScanOnDemandDownloader() =>
            new SingleScanOnDemandDownloader(CreateOnDemandDownloader(), orderShortcut);
    }
}