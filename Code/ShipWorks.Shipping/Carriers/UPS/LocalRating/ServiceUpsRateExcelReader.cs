using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Service to read rates from rate file.
    /// </summary>
    /// <remarks>
    /// This class reads the values an excel document.  The format of the document can be seen at
    /// ShipWorks.Shipping.UI.UpsLocalRatesSample.xlsx.
    /// The reader reads the sheets whose names correlate to a service. 
    /// We expect that row 1 contains the Ups Zone and Column A represents the weight.
    /// The inner cells represent the rate of a package for the corresponding weight and zone.
    /// These rates are stored in the UpsPackageRate table
    /// Column A may also read "Letter." In this case, we store the rate  in the
    /// UpsLetterRate table.
    /// Finally, Column A may read "PricePerPound." In this case, we store the rate in the 
    /// UpsPricePerPound table.
    /// </remarks>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsRateExcelReader" />
    public class ServiceUpsRateExcelReader : IUpsRateExcelReader
    {
        private List<UpsPackageRateEntity> readPackageRates;
        private List<UpsLetterRateEntity> readLetterRates;
        private List<UpsPricePerPoundEntity> readPricesPerPound;

        public const string LetterLabel = "Letter";
        public const string PricePerPoundLabel = "Price Per Pound";

        /// <summary>
        /// Reads the ups rates excel work sheets and store the rates in to the UpsLocalRateTable
        /// </summary>
        public void Read(IWorksheets rateWorkSheets, IUpsLocalRateTable upsLocalRateTable)
        {
            readPackageRates = new List<UpsPackageRateEntity>();
            readLetterRates = new List<UpsLetterRateEntity>();
            readPricesPerPound = new List<UpsPricePerPoundEntity>();

            foreach (IWorksheet sheet in rateWorkSheets)
            {
                UpsServiceType? upsServiceType = GetServiceType(sheet.Name);

                if (upsServiceType != null)
                {
                    ProcessSheet(sheet, upsServiceType.Value);
                }
            }

            upsLocalRateTable.ReplaceRates(readPackageRates, readLetterRates, readPricesPerPound);
        }

        /// <summary>
        /// Processes a rate sheet from the excel document
        /// </summary>
        /// <remarks>
        /// This method assumes the incoming sheet is a rate sheet and not a surcharge sheet
        /// </remarks>
        private void ProcessSheet(IWorksheet sheet, UpsServiceType upsServiceType)
        {
            // Column A and Row 1 describe the what the rates are for. The actual rates are then stored battle ship
            // style within Column A and Row 1.  We loop through the rows and then the cells in the row and call 
            // ProcessRate, passing in the service, weightCell from Column 1, zone cell from the header, and the rate cell.
            if (sheet.Rows.Length == 0)
            {
                throw new UpsLocalRatingException($"Sheet '{sheet.Name}' has no rows.");
            }

            IRange[] headerCells = sheet.Rows[0].Cells;

            IEnumerable<string> zoneHeaders = headerCells.Where(c => !string.IsNullOrWhiteSpace(c.Value))
                .Select(c => c.Value).ToList();

            // Ensure there are not duplicate zone columns
            if (zoneHeaders.Distinct().Count() != zoneHeaders.Count())
            {
                throw new UpsLocalRatingException($"Duplicate zone detected in sheet '{sheet.Name}'.");
            }

            // Loop through each row after the header row
            for (int rowIndex = 1; rowIndex < sheet.Rows.Length; rowIndex++)
            {
                IRange[] row = sheet.Rows[rowIndex].Cells;
                if (row.All(c=>c.IsBlank))
                {
                    continue;
                }

                // The first cell contains the weight value (or "Letter" or "Price Per Pound")
                IRange weightCell = sheet.Rows[rowIndex].Cells[0];

                // Loop through each cell in the row, starting with second cell.
                for (int i = 1; i < row.Length; i++)
                {
                    IRange headerCell = headerCells[i];
                    IRange rateCell = row[i];

                    ProcessRate(upsServiceType, weightCell, headerCell, rateCell);
                }
            }
        }

        /// <summary>
        /// Processes the rate.
        /// </summary>
        /// <param name="upsServiceType">Type of the ups service.</param>
        /// <param name="weightCell">The weight cell.</param>
        /// <param name="headerCell">The header cell.</param>
        /// <param name="rateCell">The rate cell.</param>
        /// <exception cref="UpsLocalRatingException">
        /// </exception>
        private void ProcessRate(UpsServiceType upsServiceType, IRange weightCell, IRange headerCell, IRange rateCell)
        {
            // Assuming this isn't a rate, skip.
            if (headerCell.IsBlank)
            {
                return;
            }

            // Validate Weight
            if (string.IsNullOrWhiteSpace(weightCell.Value))
            {
                throw new UpsLocalRatingException($"A blank weight found in row {weightCell.Row}");
            }

            if (!weightCell.HasNumber && weightCell.Text != PricePerPoundLabel && weightCell.Text != LetterLabel)
            {
                throw new UpsLocalRatingException(
                    $"Weight Value '{weightCell.Text}' must be a number or = \"Letter\" or \"Price Per Pound.\"");
            }

            if (!headerCell.HasNumber || !headerCell.Number.IsInt())
            {
                throw new UpsLocalRatingException($"Header text '{headerCell.Text}' must be a whole number.");
            }
            
            if (!rateCell.HasNumber && rateCell.Value != "-" && !rateCell.IsBlank)
            {
                throw new UpsLocalRatingException($"Rate text '{rateCell.Text}' must be a number, '-', or empty.");
            }

            SaveRateToCollection(upsServiceType, weightCell, headerCell, rateCell);
        }

        /// <summary>
        /// Saves the rate to appropriate collection.
        /// </summary>
        private void SaveRateToCollection(UpsServiceType upsServiceType, IRange weightCell, IRange headerCell, IRange rateCell)
        {
            int zone = Convert.ToInt32(headerCell.Number);
            decimal rate = rateCell.HasNumber ? Convert.ToDecimal(rateCell.Number) : 0;
            
            // If weight has number, it represents a package weight. We store this in the UpsPackageRate table
            if (weightCell.HasNumber)
            {
                UpsPackageRateEntity packageRateEntity = new UpsPackageRateEntity()
                {
                    Zone = zone,
                    WeightInPounds = Convert.ToInt32(weightCell.Number),
                    Service = (int) upsServiceType,
                    Rate = rate
                };
                readPackageRates.Add(packageRateEntity);
            }
            // If the cell reads "Price Per Pound" this is a price per pound value and is stored in the
            // UpsPricePerPound table
            else if (weightCell.Text == PricePerPoundLabel)
            {
                UpsPricePerPoundEntity pricePerPoundEntity = new UpsPricePerPoundEntity()
                {
                    Zone = zone,
                    Service = (int) upsServiceType,
                    Rate = rate
                };
                readPricesPerPound.Add(pricePerPoundEntity);
            }
            // If the cell reads "Letter" this is the price for sending a letter and is stored in the
            // UpsLetterRate table.
            else if (weightCell.Text == LetterLabel)
            {
                UpsLetterRateEntity letterRateEntity = new UpsLetterRateEntity()
                {
                    Zone = zone,
                    Service = (int) upsServiceType,
                    Rate = rate
                };
                readLetterRates.Add(letterRateEntity);
            }
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        private UpsServiceType? GetServiceType(string sheetName)
        {
            if (sheetName == "NDA Early")
            {
                return UpsServiceType.UpsNextDayAirAM;
            }
            else if (sheetName == "NDA")
            {
                return UpsServiceType.UpsNextDayAir;
            }
            else if (sheetName == "NDA Saver")
            {
                return UpsServiceType.UpsNextDayAirSaver;
            }
            else if (sheetName == "2DA AM")
            {
                return UpsServiceType.Ups2DayAirAM;
            }
            else if (sheetName == "2DA")
            {
                return UpsServiceType.Ups2DayAir;
            }
            else if (sheetName == "3DA Select")
            {
                return UpsServiceType.Ups3DaySelect;
            }
            else if (sheetName == "Ground")
            {
                return UpsServiceType.UpsGround;
            }
            else
            {
                return null;
            }
        }
    }
}