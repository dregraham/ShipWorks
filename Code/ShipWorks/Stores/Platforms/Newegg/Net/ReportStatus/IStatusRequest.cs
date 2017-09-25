using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus
{
    /// <summary>
    /// An interface for getting the status of a report from Newegg.
    /// </summary>
    public interface IStatusRequest
    {
        /// <summary>
        /// Gets or sets the ID of the request being inquired about.
        /// </summary>
        /// <value>The request ID.</value>
        string RequestId { get; set; }

        /// <summary>
        /// Gets the status of the report.
        /// </summary>
        /// <returns>A StatusResult object.</returns>
        Task<StatusResult> GetStatus();
    }
}
