﻿using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Factory for Magento downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Magento)]
    public class MagentoDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoDownloaderFactory(StoreEntity store,
            Func<StoreEntity, IMagentoModuleDownloader> createModuleDownloader,
            Func<StoreEntity, IMagentoTwoRestDownloader> createRestDownloader)
        {
            Store = store;
            MagentoStoreEntity typedStore = store as MagentoStoreEntity;
            downloader = typedStore.MagentoVersion == (int) MagentoVersion.MagentoTwoREST ?
                (IStoreDownloader) createRestDownloader(store) :
                (IStoreDownloader) createModuleDownloader(store);
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
        /// The store the downloader downloads from
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        public Task Download(IProgressReporter progressItem, IDownloadEntity downloadLog, DbConnection con) =>
            downloader.Download(progressItem, downloadLog, con);

        /// <summary>
        /// Does not support downloading OrderNumbers
        /// </summary>
        public Task Download(string orderNumber, long downloadID, DbConnection con)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Should not download
        /// </summary>
        public bool ShouldDownload(string orderNumber) => false;
    }
}
