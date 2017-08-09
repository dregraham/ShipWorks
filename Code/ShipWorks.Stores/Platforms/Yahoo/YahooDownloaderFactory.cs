using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// Factory for Yahoo downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Yahoo)]
    public class YahooDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooDownloaderFactory(StoreEntity store,
            Func<YahooStoreEntity, IYahooEmailDownloader> createEmailDownloader,
            Func<YahooStoreEntity, IYahooApiDownloader> createApiDownloader)
        {
            YahooStoreEntity typedStore = store as YahooStoreEntity;
            downloader = typedStore.YahooStoreID.IsNullOrWhiteSpace() ?
                (IStoreDownloader) createEmailDownloader(typedStore) :
                (IStoreDownloader) createApiDownloader(typedStore);
        }

        /// <summary>
        /// How many orders have been saved so far.  Utility function intended for progress calculation convenience.
        /// </summary>
        public int QuantitySaved => downloader.QuantitySaved;

        /// <summary>
        /// The number of orders that have been saved, that are the first time they have been downloaded.
        /// </summary>
        public int QuantityNew => downloader.QuantityNew;

        /// <summary>
        /// Download orders from the store
        /// </summary>
        public Task Download(IProgressReporter progressItem, long downloadID, DbConnection con) =>
            downloader.Download(progressItem, downloadID, con);
    }
}
