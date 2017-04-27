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
        private static readonly Regex fiveDigitZipRegex = new Regex("^\\s*[0-9]{5}\\s*$");
        private List<UpsLocalRatingZoneEntity> zones;

        /// <summary>
        /// Read Alaska zones from the given worksheet
        /// </summary>
        public IEnumerable<UpsLocalRatingZoneEntity> GetAlaskaHawaiiZones(IWorksheets zoneWorksheets)
        {
            zones = new List<UpsLocalRatingZoneEntity>();

            AddToZones(GetStateSheet(zoneWorksheets, "AK"));
            AddToZones(GetStateSheet(zoneWorksheets, "HI"));
            
            return zones;
        }

        /// <summary>
        /// Gets the state sheet.
        /// </summary>
        private static IWorksheet GetStateSheet(IWorksheets zoneWorksheets, string sheetName)
        {
            List<IWorksheet> stateSheets = zoneWorksheets.Cast<IWorksheet>().Where(sheet => sheet.Name == sheetName).ToList();

            if (stateSheets.Count > 1)
            {
                throw new UpsLocalRatingException($"Zone file should not have two '{sheetName}' worksheets");
            }

            if (stateSheets.Count < 1)
            {
                throw new UpsLocalRatingException($"Zone file must have a '{sheetName}' worksheet.");
            }

            return stateSheets.Single();
        }

        /// <summary>
        /// Read zone info from the worksheet and add it to the zones list
        /// </summary>
        private void AddToZones(IWorksheet worksheet)
        {
            // The worksheet has two sections of postal codes, find the second header "Postal Codes:" because it divides the two sections
            // to use as an anchor 
            int rowStartSecondSection = GetSecondSectionStartRow(worksheet);
            IRange firstSection = worksheet.Range[1, 1, rowStartSecondSection - 1, worksheet.UsedRange.LastColumn];
            IRange secondSection = worksheet.Range[rowStartSecondSection, 1, worksheet.UsedRange.LastRow, worksheet.UsedRange.LastColumn];
            AddSectionToZones(firstSection, "first");
            AddSectionToZones(secondSection, "second");
        }

        /// <summary>
        /// Adds the section to zones.
        /// </summary>
        private void AddSectionToZones(IRange section, string sectionName)
        {
            // Get all of the zone/service combos from the section
            Dictionary<UpsServiceType, string> zoneDictionary = GetZoneServiceDictionary(section, sectionName);

            // Create and add zone entities for all of the zip codes in the first section
            AddToZones(zoneDictionary, GetPostalCodes(section));
        }

        /// <summary>
        /// Gets the second section start row number.
        /// </summary>
        private int GetSecondSectionStartRow(IWorksheet worksheet)
        {
            // Make sure there are two postal codes labels
            List<int> postalCodeLabelRowNumbers = worksheet.Columns[0].Cells
                .Where(c => c.Value == "Postal Codes:")
                .Select(c=>c.Row).ToList();

            if (postalCodeLabelRowNumbers.Count != 2)
            {
                    throw new UpsLocalRatingException($"Error reading worksheet '{worksheet.Name}.'\r\r" +
                                                      $"{worksheet.Name} should have two sections of zip codes with a header" +
                                                      "in the first column labeled 'Postal Codes:'");
            }

            // The second section starts with Ground and is between the first and second "Postal Codes:" label
            List<int> groundLabelRow = worksheet.Columns[0].Cells
                .Where(
                    c =>
                        c.Value == "Ground" && c.Row > postalCodeLabelRowNumbers.First() &&
                        c.Row < postalCodeLabelRowNumbers.Last())
                .Select(c => c.Row).ToList();

            if (groundLabelRow.Count != 1)
            {
                HandleMissingZone("Ground", "second", worksheet.Name);
            }

            return groundLabelRow.Single();
        }

        /// <summary>
        /// Get a collection of 5 digit zip codes from the given collection or ranges
        /// </summary>
        private static List<int> GetPostalCodes(IRange section)
        {
            IEnumerable<int> postalCodeLabelRowNumbers =
                section.Rows.Where(s => s.Cells[0].Value == "Postal Codes:").Select(r=>r.Row).ToList();
            if (postalCodeLabelRowNumbers.Count() != 1)
            {
                throw new UpsLocalRatingException($"Error reading worksheet '{section.Worksheet.Name}.'\r\r" +
                                                  $"{section.Worksheet.Name} should have two sections of zip codes with a header" +
                                                  "in the first column labeled 'Postal Codes:'");
            }
            int postalCodeLabelRowNumber = postalCodeLabelRowNumbers.Single();

            return section.Rows
                .Where(r => r.Row > postalCodeLabelRowNumber)
                .SelectMany(r => r.Cells)
                .Select(cell => cell.Value ?? string.Empty)
                .Where(zip => fiveDigitZipRegex.IsMatch(zip))
                .Select(int.Parse).ToList();
        }

        /// <summary>
        /// Add a UpsLocalRatingZoneEntity to the zones collection for each Service/Zone and Zip combination
        /// </summary>
        private void AddToZones(Dictionary<UpsServiceType, string> zoneDictionary, List<int> zipCodes)
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
        private static Dictionary<UpsServiceType, string> GetZoneServiceDictionary(IRange section, string sectionName)
        {
            string groundZone = section.Rows.FirstOrDefault(r => r.Cells[0].Value == "Ground")?.Cells[1].Value;
            string nextDayAirZone = section.Rows.FirstOrDefault(r => r.Cells[0].Value == "Next Day Air")?.Cells[1].Value;
            string twoDayAirZone = section.Rows.FirstOrDefault(r => r.Cells[0].Value == "Second Day Air")?.Cells[1].Value;
            
            ValidateZones(groundZone, nextDayAirZone, twoDayAirZone, sectionName, section.Worksheet.Name);

            return GetServiceDictionary(groundZone, nextDayAirZone, twoDayAirZone);
        }

        /// <summary>
        /// Build a dictionary of UpsServiceType and passed in strings.
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetServiceDictionary(string ground,
            string nextDayAir,
            string twoDayAir)
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