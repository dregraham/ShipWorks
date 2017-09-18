using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// FTP Client for Buy.com
    /// </summary>
    public interface IBuyDotComFtpClient : IDisposable
    {
        /// <summary>
        /// Gets list of order files queued for process
        /// </summary>
        Task<List<string>> GetOrderFileNames();

        /// <summary>
        /// Given a name of the order file, return a stream that is ready to read
        /// </summary>
        Task<string> GetOrderFileContent(string fileName);

        /// <summary>
        /// Move order file to archive directory.
        /// </summary>
        Task ArchiveOrder(string fileName);

        /// <summary>
        /// Upload ship confirmation information.
        /// </summary>
        Task UploadShipConfirmation(List<BuyDotComShipConfirmation> confirmations);
    }
}