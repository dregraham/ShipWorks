using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// interface that represents the DownloadManager
    /// </summary>
    public  interface IDownloadManager
    {
        /// <summary>
        /// Download the order number from all stores
        /// </summary>
        IResult Download(string orderNumber);
    }
}
