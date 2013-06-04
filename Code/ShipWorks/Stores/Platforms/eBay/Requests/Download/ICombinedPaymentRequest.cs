using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An interface for downloading combined payments.
    /// </summary>
    public interface ICombinedPaymentRequest
    {

        /// <summary>
        /// Gets high level, summary information if payments of a particular type were to be downloaded with the given date range.
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetPaymentSummary(OrderStatusCodeType orderStatus, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets high level, summary information all payment types were to be downloaded with the given date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetPaymentSummary(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A GetOrdersResponseType object.</returns>
        GetOrdersResponseType GetActivePayments(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the completed payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A GetOrdersResponseType object.</returns>
        GetOrdersResponseType GetCompletedPayments(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageToDownload">The page to download.</param>
        /// <returns>An GetOrdersResponseType object.</returns>
        GetOrdersResponseType GetAllPayments(DateTime startDate, DateTime endDate, int pageToDownload);
    }
}
