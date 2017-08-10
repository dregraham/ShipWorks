using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Factory for Amazon downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Amazon)]
    public class AmazonDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonDownloaderFactory(StoreEntity store,
            Func<StoreEntity, IAmazonLegacyDownloader> createLegacyDownloader,
            Func<StoreEntity, IAmazonMwsDownloader> createMwsDownloader)
        {
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            downloader = amazonStore.AmazonApi == (int) AmazonApi.MarketplaceWebService ?
                (IStoreDownloader) createMwsDownloader(store) :
                (IStoreDownloader) createLegacyDownloader(store);
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
