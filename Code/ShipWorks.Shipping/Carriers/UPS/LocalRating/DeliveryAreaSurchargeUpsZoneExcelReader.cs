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
        private const string DasZipsTabName = "DASzips";
        private const string MissingColumn = DasZipsTabName + " missing '{0}' column.";
        private const string MissingDasTab = "Spreadsheet missing " + DasZipsTabName + " Tab";
        private const string TooManyColumns = DasZipsTabName + " can only have 4 columns. There may be a cell with text outside of the expected 4 columns.";
        private const string InvalidZipCode = DasZipsTabName + " has an invalid zip code for column '{0}' row {1}.";
        private const string DuplicateColumns = DasZipsTabName + " can only have 1 '{0}' column.";

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
        /// Processes the column with a header equal to the ApiValue of the dasTypeEntry
        /// </summary>
        /// <remarks>
        /// It is assumed the top cell in each column will be a header that matches the ApiValue of dasTypeEntry
        /// followed by zip codes for that surcharge type.
        /// </remarks>
        private void ProcessColumn(IWorksheet zoneWorksheet,
            EnumEntry<UpsDeliveryAreaSurchargeType> dasTypeEntry,
            List<UpsLocalRatingDeliveryAreaSurchargeEntity> readSurcharges)
        {
            List<IRange> matchingColumns = zoneWorksheet.Columns.Where(c => c.Cells[0].Text?.Trim() == dasTypeEntry.ApiValue).ToList();
            if (matchingColumns.None())
            {
                throw new UpsLocalRatingException(string.Format(MissingColumn, dasTypeEntry.ApiValue));
            }
            if (matchingColumns.Count != 1)
            {
                throw new UpsLocalRatingException(string.Format(DuplicateColumns, dasTypeEntry.ApiValue));
            }

            IRange dasColumn = matchingColumns.Single();

            // Loop through the column cells. We skip the first cell as it is the header. 
            // We also ignore empty cells. There will be empty cells because some columns are longer
            // than other columns and excel will return equal number of cells for each column.
            foreach (IRange cell in dasColumn.Cells.Skip(1).Where(c=>!string.IsNullOrWhiteSpace(c.Text)))
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