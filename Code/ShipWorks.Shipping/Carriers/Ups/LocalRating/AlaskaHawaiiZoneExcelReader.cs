using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads zone info for Alaska and Hawaii 
    /// </summary>
    [Component]
    public class AlaskaHawaiiZoneExcelReader
    {
        /// <summary>
        /// Read Alaska zones from the given worksheet
        /// </summary>
        public IEnumerable<UpsLocalRatingZoneEntity> GetAlaskaHawaiiZones(IWorksheets zoneWorksheets)
        {
            List<UpsLocalRatingZoneEntity> zones = new List<UpsLocalRatingZoneEntity>();

            foreach (IWorksheet worksheet in zoneWorksheets)
            {
                if (worksheet.Name == "AK" || worksheet.Name == "HI")
                {
                    AddToZones(worksheet, zones);
                }
            }
            
            return zones;
        }

        /// <summary>
        /// Read zone info from the worksheet and add it to the zones list
        /// </summary>
        private void AddToZones(IWorksheet worksheet, List<UpsLocalRatingZoneEntity> zones)
        {
            IRange secondPostalCodeRow = worksheet.Rows.FirstOrDefault(s => s.Cells[0].Text == "Postal Codes:" && s.Row > 5);
            if (secondPostalCodeRow == null)
            {
                throw new UpsLocalRatingException($"Missing postal codes from {worksheet.Name}.");
            }

            Dictionary<UpsServiceType, string> firstZoneDictionary = GetFirstZoneServiceDictinary(worksheet);
            AddToZones(firstZoneDictionary,
                GetPostalCodes(worksheet.Rows.Where(r => r.Row > 4 && r.Row < secondPostalCodeRow.Row)), zones);
            
            


            Dictionary<UpsServiceType, string> secondZoneDictionary = GetSecondZoneServiceDictinary(worksheet);
            AddToZones(secondZoneDictionary, GetPostalCodes(worksheet.Rows.Where(r => r.Row > secondPostalCodeRow.Row)), zones);
        }

        /// <summary>
        /// Build a dictionary of UpsServiceType/Zone from the worksheet
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetFirstZoneServiceDictinary(IWorksheet worksheet)
        {
            return new Dictionary<UpsServiceType, string>
            {
                {UpsServiceType.UpsGround, worksheet.Range["B1"].Text},
                {UpsServiceType.UpsNextDayAir, worksheet.Range["B2"].Text},
                {UpsServiceType.Ups2DayAir, worksheet.Range["B3"].Text}
            };
        }

        /// <summary>
        /// Build a dictionary of UpsServiceType/Zone from the worksheet
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetSecondZoneServiceDictinary(IWorksheet worksheet)
        {
            return new Dictionary<UpsServiceType, string>
            {
                { UpsServiceType.UpsGround, worksheet.Rows.FirstOrDefault(r => r.Cells[0].Text == "Ground" && r.Row > 4)?.Cells[1].Text},
                { UpsServiceType.UpsNextDayAir, worksheet.Rows.FirstOrDefault(r => r.Cells[0].Text == "Next Day Air" && r.Row > 4)?.Cells[1].Text},
                { UpsServiceType.Ups2DayAir, worksheet.Rows.FirstOrDefault(r => r.Cells[0].Text == "Second Day Air" && r.Row > 4)?.Cells[1].Text}
            };
        }

        /// <summary>
        /// Get a collection of 5 digit zip codes from the given collection or ranges
        /// </summary>
        private static List<int> GetPostalCodes(IEnumerable<IRange> ranges)
        {
            List<int> zipGroup = new List<int>();

            foreach (IRange row in ranges)
            {
                foreach (IRange cell in row.Cells)
                {
                    string zip = cell.Text ?? string.Empty;

                    if (Regex.IsMatch(zip, "^\\s*[0-9]{5}\\s*$"))
                    {
                        zipGroup.Add(int.Parse(zip));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return zipGroup;
        }

        /// <summary>
        /// Add a UpsLocalRatingZoneEntity to the zones collection for each Service/Zone and Zip combination
        /// </summary>
        private static void AddToZones(Dictionary<UpsServiceType, string> zoneDictionary, List<int> zipCodes,
            List<UpsLocalRatingZoneEntity> zones)
        {
            foreach (KeyValuePair<UpsServiceType, string> serviceZone in zoneDictionary)
            {
                foreach (int zipCode in zipCodes)
                {
                    UpsLocalRatingZoneEntity zoneEntity =
                        new UpsLocalRatingZoneEntity
                        {
                            DestinationZipCeiling = zipCode,
                            DestinationZipFloor = zipCode,
                            OriginZipCeiling = 100000,
                            OriginZipFloor = 1,
                            Service = (int) serviceZone.Key,
                            Zone = serviceZone.Value
                        };
                    zones.Add(zoneEntity);
                }
            }
        }
    }
}