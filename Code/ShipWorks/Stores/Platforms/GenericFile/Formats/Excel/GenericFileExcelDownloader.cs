﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using log4net;
using ShipWorks.Data.Connection;
using Interapptive.Shared.IO.Text.Csv;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.Data.SqlClient;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet.Types.Excel;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Excel
{
    /// <summary>
    /// Download implementation for importing from Excel files
    /// </summary>
    public class GenericFileExcelDownloader : GenericFileSpreadsheetDownloaderBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileExcelDownloader));

        GenericExcelMap excelMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileExcelDownloader(GenericFileStoreEntity store)
            : base(store)
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
