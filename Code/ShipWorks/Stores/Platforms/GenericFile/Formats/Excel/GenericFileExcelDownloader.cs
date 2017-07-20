using System;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet.Types.Excel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Excel
{
    /// <summary>
    /// Download implementation for importing from Excel files
    /// </summary>
    [Component]
    public class GenericFileExcelDownloader : GenericFileSpreadsheetDownloaderBase, IGenericFileExcelDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileExcelDownloader));

        GenericExcelMap excelMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileExcelDownloader(GenericFileStoreEntity store,
            Func<StoreEntity, GenericFileStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, getStoreType, configurationData, sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Do initialization needed at the beginning of the download
        /// </summary>
        protected override void InitializeDownload()
        {
            try
            {
                excelMap = new GenericExcelMap(new GenericSpreadsheetOrderSchema(), GenericStore.FlatImportMap);
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
            return new GenericExcelReader(excelMap, file.ReadAllBytes());
        }
    }
}
