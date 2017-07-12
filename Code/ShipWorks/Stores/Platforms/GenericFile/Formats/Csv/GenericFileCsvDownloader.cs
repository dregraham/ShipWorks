using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Interapptive.Shared.IO.Text.Csv;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Csv
{
    /// <summary>
    /// Download implementation for importing from CSV files
    /// </summary>
    public class GenericFileCsvDownloader : GenericFileSpreadsheetDownloaderBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileCsvDownloader));

        GenericCsvMap csvMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileCsvDownloader(GenericFileStoreEntity store)
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
                csvMap = new GenericCsvMap(new GenericSpreadsheetOrderSchema(), GenericStore.FlatImportMap);
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
