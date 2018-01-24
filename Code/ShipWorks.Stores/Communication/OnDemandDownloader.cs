using System.Threading.Tasks;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloader for downloading a specific order
    /// </summary>
    public class OnDemandDownloader : IOnDemandDownloader
    {
        private readonly IDownloadManager downloadManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloader(IDownloadManager downloadManager)
        {
            this.downloadManager = downloadManager;
        }

        /// <summary>
        /// Initiate download using given orderNumber
        /// </summary>
        public async Task Download(string orderNumber)
        {
            if (ShouldSearch(orderNumber))
            {
                await downloadManager.Download(orderNumber.Trim()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// If orderNumber is not a valid search term, return true.
        /// </summary>
        private static bool ShouldSearch(string orderNumber)
        {
            return !string.IsNullOrWhiteSpace(orderNumber) && orderNumber.Trim().Length <= 50;
        }
    }
}
