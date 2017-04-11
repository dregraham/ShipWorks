using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.ReadOnlyEntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Service to read rates from rate file.
    /// </summary>
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

            upsLocalRateTable.AddRates(readPackageRates, readLetterRates, readPricesPerPound);
        }

        /// <summary>
        /// Processes a rate sheet from the excel document
        /// </summary>
        /// <remarks>
        /// This method assumes the incoming sheet is a rate sheet and not a surcharge sheet
        /// </remarks>
        private void ProcessSheet(IWorksheet sheet, UpsServiceType upsServiceType)
        {
            if (sheet.Rows.Length == 0)
            {
                throw new UpsLocalRatingException($"Sheet {sheet.Name} has no rows.");
            }

            IRange[] headerCells = sheet.Rows[0].Cells;
            
            for (int rowIndex = 1; rowIndex < sheet.Rows.Length; rowIndex++)
            {
                IRange[] row = sheet.Rows[rowIndex].Cells;

                IRange weightCell = sheet.Rows[rowIndex].Cells[0];

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
            if (!ValidateRate(weightCell, headerCell, rateCell))
            {
                SaveRateToCollection(upsServiceType, weightCell, headerCell, rateCell);
            }
        }

        /// <summary>
        /// Validates the rate.
        /// </summary>
        private static bool ValidateRate(IRange weightCell, IRange headerCell, IRange rateCell)
        {
            // Validate Weight
            if (string.IsNullOrWhiteSpace(weightCell.Value))
            {
                if (rateCell.EntireRow.Cells.All(c => c.IsBlank))
                {
                    return true;
                }
                throw new UpsLocalRatingException($"A blank weight found in row {weightCell.Rows}");
            }

            if (!weightCell.HasNumber && weightCell.Text != PricePerPoundLabel && weightCell.Text != LetterLabel)
            {
                throw new UpsLocalRatingException(
                    $"Weight Value '{weightCell.Text}' must be a number or = \"Letter\" or \"Price Per Pound.\"");
            }

            if (headerCell.IsBlank || rateCell.IsBlank)
            {
                return true;
            }

            if (!headerCell.HasNumber || !headerCell.Number.IsInt())
            {
                throw new UpsLocalRatingException($"Header text '{headerCell.Text}' must be a whole number.");
            }

            if (!rateCell.HasNumber)
            {
                throw new UpsLocalRatingException($"Rate text '{rateCell.Text}' must be a number.");
            }
            return false;
        }

        /// <summary>
        /// Saves the rate to appropriate collection.
        /// </summary>
        private void SaveRateToCollection(UpsServiceType upsServiceType, IRange weightCell, IRange headerCell, IRange rateCell)
        {
            int zone = Convert.ToInt32(headerCell.Number);
            decimal rate = Convert.ToDecimal(rateCell.Number);
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
            if (sheetName=="NDA Early")
            {
                return UpsServiceType.UpsNextDayAirAM;
            }
            else if (sheetName=="NDA")
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