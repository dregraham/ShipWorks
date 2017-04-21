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
    public class AlaskaHawaiiZoneExcelReader : IAlaskaHawaiiZoneExcelReader
    {
        private const int SecondPostalCodesRow = 4;
        private static readonly Regex fiveDigitZipRegex = new Regex("^\\s*[0-9]{5}\\s*$");

        /// <summary>
        /// Read Alaska zones from the given worksheet
        /// </summary>
        public IEnumerable<UpsLocalRatingZoneEntity> GetAlaskaHawaiiZones(IWorksheets zoneWorksheets)
        {
            ValidateWorksheets(zoneWorksheets);

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
        /// Validate the worksheets collection
        /// </summary>
        private static void ValidateWorksheets(IWorksheets zoneWorksheets)
        {
            bool hasAlaska = false;
            bool hasHawaii = false;
            foreach (IWorksheet worksheet in zoneWorksheets)
            {
                if (worksheet.Name == "AK")
                {
                    hasAlaska = true;
                }

                if (worksheet.Name == "HI")
                {
                    hasHawaii = true;
                }
            }

            if (!hasAlaska || !hasHawaii)
            {
                throw new UpsLocalRatingException("Zone file must have 'AK' and 'HI' worksheets.");
            }
        }

        /// <summary>
        /// Read zone info from the worksheet and add it to the zones list
        /// </summary>
        private void AddToZones(IWorksheet worksheet, List<UpsLocalRatingZoneEntity> zones)
        {
            // The worksheet has two sections of postal codes, find the second header "Postal Codes:" because it divides the two sections
            // to use as an anchor 
            IRange secondPostalCodeRow = worksheet.Rows.FirstOrDefault(s => s.Cells[0].Value == "Postal Codes:" && s.Row > SecondPostalCodesRow);
            if (secondPostalCodeRow == null)
            {
                throw new UpsLocalRatingException($"Missing postal codes from {worksheet.Name}.");
            }

            // Get all of the zone/service combos from the first section
            Dictionary<UpsServiceType, string> firstZoneDictionary = GetFirstZoneServiceDictinary(worksheet);

            // Create and add zone entities for all of the zip codes in the first section
            AddToZones(firstZoneDictionary, GetPostalCodes(worksheet.Rows.Where(r => r.Row > SecondPostalCodesRow && r.Row < secondPostalCodeRow.Row)), zones);
            
            // Get all of the zone/service combos for the second section
            Dictionary<UpsServiceType, string> secondZoneDictionary = GetSecondZoneServiceDictinary(worksheet);

            // Create and add zone entities for all of the zip codes in the second section
            AddToZones(secondZoneDictionary, GetPostalCodes(worksheet.Rows.Where(r => r.Row > secondPostalCodeRow.Row)), zones);
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
                    string zip = cell.Value ?? string.Empty;

                    if (fiveDigitZipRegex.IsMatch(zip))
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
        
        /// <summary>
        /// Build a dictionary of UpsServiceType/Zone from the worksheet
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetFirstZoneServiceDictinary(IWorksheet worksheet)
        {
            string groundZone = worksheet.Range["B1"].Value;
            string nextDayAirZone = worksheet.Range["B2"].Value;
            string twoDayAirZone = worksheet.Range["B3"].Value;

            ValidateZones(groundZone, nextDayAirZone, twoDayAirZone, "first", worksheet.Name);

            return GetServiceDictinary(groundZone, nextDayAirZone, twoDayAirZone);
        }

        /// <summary>
        /// Build a dictionary of UpsServiceType/Zone from the worksheet
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetSecondZoneServiceDictinary(IWorksheet worksheet)
        {
            string groundZone = worksheet.Rows.FirstOrDefault(r => r.Cells[0].Value == "Ground" && r.Row > SecondPostalCodesRow)?.Cells[1].Value;
            string nextDayAirZone = worksheet.Rows.FirstOrDefault(r => r.Cells[0].Value == "Next Day Air" && r.Row > SecondPostalCodesRow)?.Cells[1].Value;
            string twoDayAirZone = worksheet.Rows.FirstOrDefault(r => r.Cells[0].Value == "Second Day Air" && r.Row > SecondPostalCodesRow)?.Cells[1].Value;

            ValidateZones(groundZone, nextDayAirZone, twoDayAirZone, "second", worksheet.Name);

            return GetServiceDictinary(groundZone, nextDayAirZone, twoDayAirZone);
        }

        private static Dictionary<UpsServiceType, string> GetServiceDictinary(string ground, string nextDayAir, string twoDayAir)
        {
            return new Dictionary<UpsServiceType, string>
            {
                {UpsServiceType.UpsGround, ground},
                {UpsServiceType.UpsNextDayAir, nextDayAir},
                {UpsServiceType.Ups2DayAir, twoDayAir}
            };
        }

        /// <summary>
        /// Ensure each zone string has a value
        /// </summary>
        private static void ValidateZones(string ground, string nextDayAir, string twoDayAir, string section, string worksheetName)
        {
            if (string.IsNullOrWhiteSpace(ground))
            {
                HandleMissingZone("Ground", section, worksheetName);
            }

            if (string.IsNullOrWhiteSpace(nextDayAir))
            {
                HandleMissingZone("Next Day Air", section, worksheetName);
            }

            if (string.IsNullOrWhiteSpace(twoDayAir))
            {
                HandleMissingZone("Second Day Air", section, worksheetName);
            }
        }

        /// <summary>
        /// Throw an exception with the missing zone service for a section/worksheet 
        /// </summary>
        private static void HandleMissingZone(string service, string section, string worksheetName)
        {
            throw new UpsLocalRatingException($"Missing {service} zone for {section} section of {worksheetName} worksheet.");
        }
    }
}