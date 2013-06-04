using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.IO.Text.Csv;
using System.IO;
using log4net;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv
{
    /// <summary>
    /// Class for reading csv files with mappings
    /// </summary>
    public sealed class GenericCsvReader : GenericSpreadsheetReader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericCsvReader));

        CachedCsvReader reader;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvReader(GenericCsvMap map, string csvContents)
            : base(map)
        {
            if (csvContents == null)
            {
                throw new ArgumentNullException("csvContents");
            }

            this.reader = OpenCsv(new StringReader(csvContents));
        }

        /// <summary>
        /// Open a CSV reader 
        /// </summary>
        private CachedCsvReader OpenCsv(TextReader textReader)
        {
            try
            {
                GenericCsvMap map = (GenericCsvMap) Map;

                return new CachedCsvReader(textReader, true, map.SourceSchema.Delimiter, map.SourceSchema.Quotes, map.SourceSchema.QuotesEscape, '#', true);
            }
            catch (MalformedCsvException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks could not read the input file:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Disposable
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Advance the reader to the next record.  Returns false if there are no more records
        /// </summary>
        public override bool NextRecord()
        {
            try
            {
                return reader.ReadNextRecord();
            }
            catch (MissingFieldCsvException ex)
            {
                throw new GenericSpreadsheetException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                log.Error("Error reading record.", ex);

                throw new GenericSpreadsheetException("The file does not contain header columns, or contains header columns with duplicate names.", ex);
            }
        }

        /// <summary>
        /// Rewind the reader to the previous record.  Returns false if already at the beginning.
        /// </summary>
        public override bool PreviousRecord()
        {
            if (reader.CurrentRecordIndex == 0)
            {
                return false;
            }
            else
            {
                reader.MoveTo(reader.CurrentRecordIndex - 1);
                return true;
            }
        }

        /// <summary>
        /// Create the value of the current for for the given column name
        /// </summary>
        public override string ReadColumnText(string column)
        {
            if (reader.GetFieldIndex(column) < 0)
            {
                throw new GenericSpreadsheetException(string.Format("The column '{0}' does not exist in the input file.", column));
            }

            return reader[column];
        }
    }
}
