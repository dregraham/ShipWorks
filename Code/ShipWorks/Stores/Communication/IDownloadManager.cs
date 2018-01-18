﻿using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Interface that represents the DownloadManager
    /// </summary>
    public  interface IDownloadManager
    {
        /// <summary>
        /// Download the order number from all stores
        /// </summary>
        Task<IResult> Download(string orderNumber);
    }
}
