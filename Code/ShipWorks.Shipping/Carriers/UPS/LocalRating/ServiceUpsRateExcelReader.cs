using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Service to read rates from rate file.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsRateExcelReader" />
    public class ServiceUpsRateExcelReader : IUpsRateExcelReader
    {
        private List<UpsRateEntity> readRates;

        /// <summary>
        /// Reads the ups rates excel work sheets and store the rates in to the UpsLocalRateTable
        /// </summary>
        public void Read(IWorksheets rateWorkSheets, IUpsLocalRateTable upsLocalRateTable)
        {
            readRates = new List<UpsRateEntity>();

            foreach (IWorksheet sheet in rateWorkSheets)
            {
                UpsServiceType? upsServiceType = GetServiceType(sheet.Name);

                if (upsServiceType != null)
                {
                    ProcessSheet(sheet, upsServiceType.Value);
                }
            }

            if (readRates.Count>0)
            {
                upsLocalRateTable.AddRates(readRates);
            }
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

                int weight = GetWeight(sheet, rowIndex);

                for (int i = 1; i < row.Length; i++)
                {
                    string headerText = headerCells[i].Value;
                    string rateText = row[i].Value;

                    UpsRateEntity rateEntity = ProcessRate(upsServiceType, weight, headerText, rateText);
                    if (rateEntity != null)
                    {
                        readRates.Add(rateEntity);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the rate.
        /// </summary>
        private static UpsRateEntity ProcessRate(UpsServiceType upsServiceType, int weight, string headerText, string rateText)
        {
            if (string.IsNullOrWhiteSpace(headerText) || string.IsNullOrWhiteSpace(rateText))
            {
                return null;
            }

            int zone;
            if (!int.TryParse(headerText, out zone))
            {
                throw new UpsLocalRatingException($"Header text '{headerText}' must be a number.");
            }

            decimal rate;
            if (!decimal.TryParse(rateText, out rate))
            {
                throw new UpsLocalRatingException($"Rate text '{rateText}' must be a number.");
            }
            
            return new UpsRateEntity()
            {
                Zone = zone,
                WeightInPounds = weight,
                Service = (int) upsServiceType,
                Rate = rate
            };
        }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        private static int GetWeight(IWorksheet sheet, int rowIndex)
        {
            IRange[] row = sheet.Rows[rowIndex].Cells;

            int weight;
            if (row[0].Text == "Letter")
            {
                weight = 0;
            }
            else if (row[0].Text == "Price Per Pound")
            {
                weight = -1;
            }
            else if (!int.TryParse(row[0].Value, out weight))
            {
                throw new UpsLocalRatingException($"Invalid weight on sheet {sheet.Name}, Row {rowIndex}");
            }

            return weight;
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