﻿using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Csv;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Excel;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Xml;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Factory for GenericFile downloaders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.GenericFile)]
    public class GenericFileDownloaderFactory : IStoreDownloader
    {
        private readonly IStoreDownloader downloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileDownloaderFactory(StoreEntity store,
            Func<GenericFileStoreEntity, IGenericFileXmlDownloader> createXmlDownloader,
            Func<GenericFileStoreEntity, IGenericFileCsvDownloader> createCsvDownloader,
            Func<GenericFileStoreEntity, IGenericFileExcelDownloader> createExcelDownloader)
        {
            Store = store;
            GenericFileStoreEntity generic = (GenericFileStoreEntity) store;

            switch ((GenericFileFormat) generic.FileFormat)
            {
                case GenericFileFormat.Xml:
                    downloader = createXmlDownloader(generic);
                    break;
                case GenericFileFormat.Csv:
                    downloader = createCsvDownloader(generic);
                    break;
                case GenericFileFormat.Excel:
                    downloader = createExcelDownloader(generic);
                    break;
                default:
                    throw new InvalidOperationException("Unknown FileFormat: " + generic.FileFormat);
            }
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
