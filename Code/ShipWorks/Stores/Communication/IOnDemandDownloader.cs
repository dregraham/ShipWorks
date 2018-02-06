using System.Threading.Tasks;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloader for downloading a specific order
    /// </summary>
    public interface IOnDemandDownloader
    {
        /// <summary>
        /// Download order
        /// </summary>
        Task Download(string orderNumber);
    }
}
