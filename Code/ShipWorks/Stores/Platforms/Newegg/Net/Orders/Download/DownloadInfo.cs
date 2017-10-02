using System;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download
{
    /// <summary>
    /// A data transport object containing summary information about downloading order
    /// between a given start and end date.
    /// </summary>
    public class DownloadInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadInfo"/> class.
        /// </summary>
        public DownloadInfo()
        { }

        /// <summary>
        /// Gets or sets the total orders.
        /// </summary>
        /// <value>
        /// The total orders.
        /// </value>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }
    }
}
