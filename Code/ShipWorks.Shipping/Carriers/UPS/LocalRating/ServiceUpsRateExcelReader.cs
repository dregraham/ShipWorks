using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    public class ServiceUpsRateExcelReader : IUpsRateExcelReader
    {
        private List<UpsLocalRateEntity> readRates;

        public void Read(IWorksheets rateWorkSheets, IUpsLocalRateTable upsLocalRateTable)
        {
            readRates = new List<UpsLocalRateEntity>();

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

        private void ProcessSheet(IWorksheet sheet, UpsServiceType upsServiceType)
        {
            IRange[] headerCells = sheet.Rows[0].Cells;

            for (int rowIndex = 1; rowIndex < sheet.Rows.Length; rowIndex++)
            {
                IRange[] row = sheet.Rows[rowIndex].Cells;

                int weight = GetWeight(sheet, rowIndex);

                for (int i = 1; i < row.Length; i++)
                {
                    string headerText = headerCells[i].Value;
                    string rateText = row[i].Value;

                    UpsLocalRateEntity rateEntity = ProcessRate(upsServiceType, weight, headerText, rateText);
                    if (rateEntity != null)
                    {
                        readRates.Add(rateEntity);
                    }
                }
            }
        }

        private static UpsLocalRateEntity ProcessRate(UpsServiceType upsServiceType, int weight, string headerText, string rateText)
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
            
            return new UpsLocalRateEntity()
            {
                Zone = zone,
                Weight = weight,
                Service = (int) upsServiceType,
                Rate = rate
            };
        }

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
            else
            {
                return null;
            }
        }
    }
}