﻿using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Download from a store
    /// </summary>
    public interface IStoreDownloader
    {
        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        int QuantitySaved { get; }

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        int QuantityNew { get; }

        /// <summary>
        /// Downloads a specific order from the store
        /// </summary>
        Task Download(IProgressReporter progressItem, IDownloadEntity downloadLog, DbConnection con);

        /// <summary>
        /// Download the orderNumber from the store
        /// </summary>
        Task Download(string orderNumber, long downloadID, DbConnection con);

        /// <summary>
        /// Whether or not the downloader should download the given order number
        /// </summary>
        bool ShouldDownload(string orderNumber);

        /// <summary>
        /// The store the downloader downloads from
        /// </summary>
        StoreEntity Store { get; }
    }
}
