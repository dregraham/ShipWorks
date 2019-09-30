﻿using System;
using System.Text;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Warehouse;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Csv
{
    /// <summary>
    /// Download implementation for importing from CSV files
    /// </summary>
    [Component]
    public class GenericFileCsvDownloader : GenericFileSpreadsheetDownloaderBase, IGenericFileCsvDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileCsvDownloader));

        GenericCsvMap csvMap;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public GenericFileCsvDownloader(GenericFileStoreEntity store,
            Func<StoreEntity, GenericFileStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory, 
            IWarehouseOrderClient warehouseOrderClient,
            ILicenseService licenseService)
            : base(store, getStoreType, configurationData, sqlAdapterFactory, warehouseOrderClient, licenseService)
        {

        }

        /// <summary>
        /// Do initialization needed at the beginning of the download
        /// </summary>
        protected override void InitializeDownload()
        {
            try
            {
                csvMap = new GenericCsvMap(new GenericSpreadsheetOrderSchema(), GenericStoreEntity.FlatImportMap);
            }
            catch (GenericSpreadsheetException ex)
            {
                throw new GenericFileStoreException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create the spreadsheet reader for the given file
        /// </summary>
        protected override GenericSpreadsheetReader CreateReader(GenericFileInstance file)
        {
            Encoding encoding = GenericCsvUtility.GetEncoding(csvMap.SourceSchema.Encoding);

            return new GenericCsvReader(csvMap, file.ReadAllText(encoding ?? Encoding.UTF8, encoding == null));
        }
    }
}
