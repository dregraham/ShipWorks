namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Download
    /// </summary>
    public interface IOnDemandDownloader
    {
        /// <summary>
        /// Download using an order number
        /// </summary>
        void Download(string orderNumber);
    }
}
