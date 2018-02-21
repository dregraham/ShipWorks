using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Manual
{
    /// <summary>
    /// ManualStore Downloader
    /// </summary>
    /// <remarks>
    /// This class was created to add a downloader shell to ShipWorks
    /// Should download is set to false which disables the functionality
    /// </remarks>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Manual)]
    public class ManualDownloader : IStoreDownloader
    {
        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        public int QuantitySaved { get; }

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        public int QuantityNew { get; }

        /// <summary>
        /// The store the downloader downloads from
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        public Task Download(IProgressReporter progressItem, long downloadID, DbConnection con)
        {
            progressItem.PercentComplete = 100;
            progressItem.Detail = "Manual Orders Do Not Download";

            return Task.CompletedTask;
        }

        /// <summary>
        /// Does not support downloading OrderNumbers
        /// </summary>
        public Task Download(string orderNumber, long downloadID, DbConnection con)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Whether or not the downloader should download
        /// </summary>
        public bool ShouldDownload(string orderNumber) => false;
    }
}