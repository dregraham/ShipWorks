using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;

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
        /// Download orders from the store
        /// </summary>
        Task Download(IProgressReporter progressItem, long downloadID, DbConnection con);
    }
}
