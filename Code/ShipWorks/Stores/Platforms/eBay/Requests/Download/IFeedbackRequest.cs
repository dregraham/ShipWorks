using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An interface for downloading feedback.
    /// </summary>
    public interface IFeedbackRequest
    {
        /// <summary>
        /// Gets the feedback download summary.
        /// </summary>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetFeedbackDownloadSummary();

        /// <summary>
        /// Gets the feedback details.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetFeedbackResponseType object.</returns>
        GetFeedbackResponseType GetFeedbackDetails(int pageNumber);
    }
}
