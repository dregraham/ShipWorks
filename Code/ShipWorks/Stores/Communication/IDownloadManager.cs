using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Interface that represents the DownloadManager
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// Initiate download using given orderNumber
        /// </summary>
        Task<IEnumerable<Exception>> Download(string orderNumber);
    }
}
