using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An interface for downloading selling data.
    /// </summary>
    public interface ISellingRequest
    {
        /// <summary>
        /// Gets the maximum duration in days.
        /// </summary>
        int MaximumDurationInDays { get; }

        /// <summary>
        /// Gets the sold items summary.
        /// </summary>
        /// <param name="durationInDays">The duration in days.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetSoldItemsSummary(int durationInDays);

        /// <summary>
        /// Gets the sold items.
        /// </summary>
        /// <param name="durationInDays">The duration in days.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetMyeBaySellingResponseType object.</returns>
        GetMyeBaySellingResponseType GetSoldItems(int durationInDays, int pageNumber);
    }
}
