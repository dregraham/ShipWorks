using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.PayPal;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Downloader for eBay
    /// </summary>
    public class EbayDownloader : StoreDownloader
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EbayDownloader));

        // The current time according to eBay
        DateTime eBayOfficialTime;

        // WebClient to use for connetivity
        EbayWebClient webClient;

        // Time range we are downloading from\to
        DateTime rangeStart;
        DateTime rangeEnd;

        // Total number of ordres expected during this download
        int expectedCount;

        /// <summary>
        /// Create the new eBay downloader
        /// </summary>
        public EbayDownloader(StoreEntity store)
            : base(store)
        {
            webClient = new EbayWebClient(EbayToken.FromStore((EbayStoreEntity) store));
        }

        /// <summary>
        /// Begin the order download process
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Connecting to eBay...";

                // Get the official eBay time in UTC
                eBayOfficialTime = webClient.GetOfficialTime();

                // Get the date\time to start downloading from
                rangeStart = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-7);
                rangeEnd = eBayOfficialTime.AddMinutes(-5);

                if (!DownloadOrders())
                {
                    return;
                }

                // Done
                Progress.Detail = "Done";
                Progress.PercentComplete = 100;
            }
            catch (EbayException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (PayPalException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download all orders from eBay
        /// </summary>
        private bool DownloadOrders()
        {
            int page = 1;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                GetOrdersResponseType response = webClient.GetOrders(rangeStart, rangeEnd, page);

                // Grab the total expected account from the first page
                if (page == 1)
                {
                    expectedCount = response.PaginationResult.TotalNumberOfEntries;
                }

                // Process all of the downloaded orders
                ProcessOrders(response.OrderArray);

                // Quit if eBay says there aren't any more
                if (!response.HasMoreOrders)
                {
                    return true;
                }

                // Next page
                page++;
            }
        }

        // Process all of the ordres in the array
        private void ProcessOrders(OrderType[] orders)
        {
            foreach (OrderType orderType in orders)
            {
                SaveDownloadedOrder((OrderEntity) DataProvider.GetEntity(3336006));

                Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved, expectedCount);
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);
            }
        }
    }
}
