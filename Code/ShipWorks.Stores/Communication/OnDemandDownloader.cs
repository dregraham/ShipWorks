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
            IResult result = await downloadManager.Download(orderNumber);

            if (result.Failure)
            {
                messageHelper.ShowError(result.Message);
            }
        }
    }
}
