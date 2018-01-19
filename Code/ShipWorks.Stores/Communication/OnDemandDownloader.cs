using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloader for downloading a specific order
    /// </summary>
    [Component]
    public class OnDemandDownloader : IOnDemandDownloader
    {
        private readonly IMessageHelper messageHelper;
        private readonly IDownloadManager downloadManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloader(IMessageHelper messageHelper, IDownloadManager downloadManager)
        {
            this.messageHelper = messageHelper;
            this.downloadManager = downloadManager;
        }

        /// <summary>
        /// Initiate download using given orderNumber
        /// </summary>
        public async Task Download(string orderNumber)
        {
            if (ShouldNotSearch(orderNumber))
            {
                return;
            }

            IResult result = await downloadManager.Download(orderNumber);

            if (result.Failure)
            {
                messageHelper.ShowError(result.Message);
            }
        }

        /// <summary>
        /// If orderNumber is not a valid search term, return true.
        /// </summary>
        private static bool ShouldNotSearch(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return true;
            }

            if (orderNumber.Length > 50)
            {
                return true;
            }

            return false;
        }
    }
}
