namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Factory for creating OnDemandDownloaders
    /// </summary>
    public interface IOnDemandDownloaderFactory
    {
        /// <summary>
        /// Create an OnDemandDownloader
        /// </summary>
        IOnDemandDownloader CreateOnDemandDownloader();

        /// <summary>
        /// Create a SingleScanOnDemandDownloader
        /// </summary>
        IOnDemandDownloader CreateSingleScanOnDemandDownloader();
    }
}
