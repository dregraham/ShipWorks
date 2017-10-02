using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download
{
    /// <summary>
    /// An interface for downloading orders from Newegg.
    /// </summary>
    public interface IDownloadOrderRequest
    {
        /// <summary>
        /// Gets the maximum number of results that will appear on a single page.
        /// </summary>
        /// <value>The size of the max page.</value>
        int MaxPageSize { get; }

        /// <summary>
        /// Submits the request to download the orders.
        /// </summary>
        /// <param name="utcFromDate">The UTC from date.</param>
        /// <param name="utcToDate">The UTC to date.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns>
        /// A DownloadResult object.
        /// </returns>
        Task<DownloadResult> Download(DateTime utcFromDate, DateTime utcToDate, int pageNumber, NeweggOrderType orderType);

        /// <summary>
        /// Downloads the specified orders.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>A DownloadResult object.</returns>
        Task<DownloadResult> Download(IEnumerable<Order> orders, int pageNumber);

        /// <summary>
        /// Gets the download info for a particular date range.
        /// </summary>
        /// <param name="utcFromDate">The UTC from date.</param>
        /// <param name="utcToDate">The UTC to date.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns>
        /// A DownloadInfo object.
        /// </returns>
        Task<DownloadInfo> GetDownloadInfo(DateTime utcFromDate, DateTime utcToDate, NeweggOrderType orderType);

        /// <summary>
        /// Gets the download info for a set of orders.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns>A DownloadInfo object.</returns>
        DownloadInfo GetDownloadInfo(IEnumerable<Order> orders);
    }
}
