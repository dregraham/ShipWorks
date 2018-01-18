namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Downloader for downloading a specific order
    /// </summary>
    public interface IOnDemandDownloader
    {
        /// <summary>
        /// Download using an order number
        /// </summary>
        void Download(string orderNumber);
    }
}
