using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads Delivery Area Surcharges from an excel document and saves them via
    /// the UpsLocalRateTable.
    /// </summary>
    public class DeliveryAreaSurchargeUpsZoneExcelReader : IUpsZoneExcelReader
    {
        public const string MissingColumn = "DASzips missing '{0}' column.";
        public const string MissingDasTab = "Spreadsheet missing DASzips Tab";
        public const string TooManyColumns = "DASzips can only have 4 columns. There may be a cell with text outside of the expected 4 columns.";
        public const string DasZipsTabName = "DASzips";
        public const string InvalidZipCode = "DASzips has an invalid zip code for column {0} row {1}.";
        public const string DuplicateColumns = "DASzips can only have 1 '{0}' column.";

        /// <summary>
        /// Reads the worksheets in a UPS zone excel document, creates a list of  
        /// UpsLocalRatingDeliveryAreaSurchargeEntity objects, and passes the
        /// list to upsLocalRateTable.ReplaceDeliveryAreaSurcharges
        /// </summary>
        public void Read(IWorksheets zoneWorksheets, IUpsLocalRateTable upsLocalRateTable)
        {
            IWorksheet zoneWorksheet = zoneWorksheets[DasZipsTabName];
            if (zoneWorksheet == null)
            {
                throw new UpsLocalRatingException(MissingDasTab);
            }

            if (zoneWorksheet.Columns.Length>4)
            {
                throw new UpsLocalRatingException(TooManyColumns);
            }

            List<UpsLocalRatingDeliveryAreaSurchargeEntity> readSurcharges = new List<UpsLocalRatingDeliveryAreaSurchargeEntity>();

            foreach (EnumEntry<UpsDeliveryAreaSurchargeType> enumEntry in EnumHelper.GetEnumList<UpsDeliveryAreaSurchargeType>())
            {
                ProcessColumn(zoneWorksheet, enumEntry, readSurcharges);
            }

            upsLocalRateTable.ReplaceDeliveryAreaSurcharges(readSurcharges);
        }

        /// <summary>
        /// Processes the column.
        /// </summary>
        private void ProcessColumn(IWorksheet zoneWorksheet,
            EnumEntry<UpsDeliveryAreaSurchargeType> dasTypeEntry,
            List<UpsLocalRatingDeliveryAreaSurchargeEntity> readSurcharges)
        {
            List<IRange> matchingColumns = zoneWorksheet.Columns.Where(c => c.Cells[0].Text.Trim() == dasTypeEntry.ApiValue).ToList();
            if (matchingColumns.None())
            {
                throw new UpsLocalRatingException(string.Format(MissingColumn, dasTypeEntry.ApiValue));
            }
            if (matchingColumns.Count != 1)
            {
                throw new UpsLocalRatingException(string.Format(DuplicateColumns, dasTypeEntry.ApiValue));
            }

            IRange dasColumn = matchingColumns.Single();

            foreach (IRange cell in dasColumn.Cells.Skip(1).Where(c=>(c.Text?.Trim() ?? string.Empty) != string.Empty))
            {
                string zip = cell.Text.Trim();

                if (!Regex.IsMatch(zip, "^[0-9]{5}$"))
                {
                    string errorMessage = string.Format(InvalidZipCode, dasTypeEntry.ApiValue, cell.Row);
                    throw new UpsLocalRatingException(errorMessage);
                }

                readSurcharges.Add(new UpsLocalRatingDeliveryAreaSurchargeEntity()
                {
                    DestinationZip = int.Parse(zip),
                    DeliveryAreaType = (int) dasTypeEntry.Value
                });
            }
        }
    }
}