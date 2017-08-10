using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Factory for ChannelAdvisor downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorDownloaderFactory(StoreEntity store,
            Func<StoreEntity, IChannelAdvisorSoapDownloader> createSoapDownloader,
            Func<StoreEntity, IChannelAdvisorRestDownloader> createRestDownloader)
        {
            ChannelAdvisorStoreEntity typedStore = store as ChannelAdvisorStoreEntity;
            downloader = string.IsNullOrWhiteSpace(typedStore.RefreshToken) ?
                (IStoreDownloader) createSoapDownloader(store) :
                (IStoreDownloader) createRestDownloader(store);
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
