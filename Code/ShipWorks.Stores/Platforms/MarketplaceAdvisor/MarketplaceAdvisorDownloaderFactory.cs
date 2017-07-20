using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Factory for MarketplaceAdvisor downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.MarketplaceAdvisor)]
    public class MarketplaceAdvisorDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorDownloaderFactory(StoreEntity store,
            Func<MarketplaceAdvisorStoreEntity, IMarketplaceAdvisorLegacyDownloader> createLegacyDownloader,
            Func<MarketplaceAdvisorStoreEntity, IMarketplaceAdvisorOmsDownloader> createOmsDownloader)
        {
            MarketplaceAdvisorStoreEntity typedStore = store as MarketplaceAdvisorStoreEntity;
            downloader = typedStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS ?
                (IStoreDownloader) createOmsDownloader(typedStore) :
                (IStoreDownloader) createLegacyDownloader(typedStore);
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
