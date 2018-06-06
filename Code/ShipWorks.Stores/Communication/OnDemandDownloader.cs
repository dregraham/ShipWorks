using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Utility;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloader for downloading a specific order
    /// </summary>
    public class OnDemandDownloader : IOnDemandDownloader
    {
        private readonly IDownloadManager downloadManager;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloader(IDownloadManager downloadManager, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
        {
            this.downloadManager = downloadManager;
            this.messageHelper = messageHelper;
            this.log = logFactory(typeof(OnDemandDownloader));
        }

        /// <summary>
        /// Initiate download using given orderNumber
        /// </summary>
        public async Task Download(string orderNumber)
        {
            if (ShouldSearch(orderNumber))
            {
                try
                {
                    IEnumerable<Exception> exceptions = await downloadManager.Download(orderNumber.Trim()).ConfigureAwait(false);
                    ShowPopup(exceptions, orderNumber);
                }
                catch (SqlAppResourceLockException)
                {
                    log.Info("skipping download because another downloader is downloading that order number currently.");
                }
            }
        }

        /// <summary>
        /// Examines exception and shows popup if applicable
        /// </summary>
        private void ShowPopup(IEnumerable<Exception> exceptions, string orderNumber)
        {
            if (exceptions.Any())
            {
                bool shouldShowError = false;
                foreach (Exception exception in exceptions)
                {
                    OnDemandDownloadException onDemandDownloadException =
                        exception.GetAllExceptions().OfType<OnDemandDownloadException>().SingleOrDefault();

                    // If onDemandDownloadException not found, or if it is found and specifies the popup 
                    // should be displayed, we should show an error
                    if (onDemandDownloadException?.ShowPopup ?? true)
                    {
                        shouldShowError = true;
                    }
                }

                if (shouldShowError)
                {
                    messageHelper.ShowPopup(
                        $"There was an error downloading '{orderNumber}.' Please see the download log for additional information.");
                }
            }
        }

        /// <summary>
        /// If orderNumber is a valid search term, return true.
        /// </summary>
        private static bool ShouldSearch(string orderNumber)
        {
            return !string.IsNullOrWhiteSpace(orderNumber) && orderNumber.Trim().Length <= 50;
        }
    }
}
