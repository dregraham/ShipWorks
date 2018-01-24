using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Interface that represents the DownloadManager
    /// </summary>
    [Component]
    public class DownloadManagerWrapper : IDownloadManager
    {
        /// <summary>
        /// Initiate download using given orderNumber
        /// </summary>
        public Task<IEnumerable<Exception>> Download(string orderNumber)
        {
            return DownloadManager.Download(orderNumber);
        }
    }
}
