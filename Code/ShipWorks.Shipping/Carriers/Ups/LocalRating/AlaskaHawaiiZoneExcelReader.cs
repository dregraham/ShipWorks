﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Collections;
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
        private readonly Regex fiveDigitZipRegex = new Regex("^\\s*[0-9]{5}\\s*$");
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
        /// <exception cref="UpsLocalRatingException"> Throws exception if 1 sheet isn't found
        /// </exception>
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
            List<IRange> sections = GetSections(worksheet).ToList();
            if (sections.None())
            {
                throw new UpsLocalRatingException($"Error reading worksheet '{worksheet.Name}.'\r\r" +
                                  $"{worksheet.Name} should have at least one section starting with ground" +
                                  "in the first column.");
            }

            foreach (IRange section in sections)
            {
                AddSectionToZones(section);
            }
        }

        /// <summary>
        /// Gets the sections from a given worksheet.
        /// </summary>
        private IEnumerable<IRange> GetSections(IWorksheet worksheet)
        {
            List<int> groundRowNumbers =
                worksheet.Columns[0].Cells.Where(s => s.Value == "Ground").Select(cell => cell.Row).ToList();

            for (int groundRowNumberIndex = 0; groundRowNumberIndex < groundRowNumbers.Count; groundRowNumberIndex++)
            {
                int lastRowOfSection = groundRowNumberIndex == groundRowNumbers.Count - 1 ?
                    worksheet.UsedRange.LastRow :
                    groundRowNumbers[groundRowNumberIndex + 1] - 1;

                yield return worksheet.Range[groundRowNumbers[groundRowNumberIndex], 1, lastRowOfSection, worksheet.UsedRange.LastColumn];
            }
        }

        /// <summary>
        /// Adds the section to zones.
        /// </summary>
        /// <remarks>
        /// Each section will start with service labels followed by zone number in columns a and b.
        /// This will be followed by Postal Codes: in column A followed by a block of zip codes that
        /// can span any number of columns.
        /// </remarks>
        private void AddSectionToZones(IRange section)
        {
            List<int> zipCodes = GetPostalCodes(section).ToList();
            Dictionary<UpsServiceType, string> zoneDictionary = GetZoneServiceDictionary(section);

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
        /// Get a collection of 5 digit zip codes from the given collection or ranges
        /// </summary>
        /// <remarks>
        /// Each section will end with a label in column A with the value of "Postal Codes:"
        /// followed by a block of zip codes that can span any number of columns.
        /// </remarks>
        private IEnumerable<int> GetPostalCodes(IRange section)
        {
            // Find "Postal Code:" label and throw if it cannot be found.
            IEnumerable<int> postalCodeLabelRowNumbers =
                section.Rows.Where(s => s.Cells[0].Value == "Postal Codes:").Select(r=>r.Row).ToList();
            if (postalCodeLabelRowNumbers.Count() != 1)
            {
                throw new UpsLocalRatingException($"Error reading worksheet '{section.Worksheet.Name}.'\r\r" +
                                                  $"section starting at row {section.Row} should have zip codes with a header" +
                                                  "in the first column labeled 'Postal Codes:'");
            }

            // Get the row number for the found postal code row.
            int postalCodeLabelRowNumber = postalCodeLabelRowNumbers.Single();

            // Get all the non-blank cells in the section under "Postal Codes:"
            List<IRange> postalCodeCells = section.Rows
                .Where(r => r.Row > postalCodeLabelRowNumber)
                .SelectMany(r => r.Cells)
                .Where(r=> !r.IsBlank)
                .ToList();

            // Loop through each cell. If we encounter a value that cannot be parsed, throw an exception
            foreach (IRange cell in postalCodeCells)
            {
                string zip = cell.Value ?? string.Empty;

                if (fiveDigitZipRegex.IsMatch(zip))
                {
                    yield return int.Parse(zip);
                }
                else
                {
                    throw new UpsLocalRatingException($"Invalid zip code found in sheet {section.Worksheet.Name}, cell {cell.AddressLocal}");
                }
            }
        }

        /// <summary>
        /// Build a dictionary of UpsServiceType/Zone from the worksheet
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetZoneServiceDictionary(IRange section)
        {
            Dictionary<UpsServiceType, string> zoneTypes = new Dictionary<UpsServiceType, string>();

            AddZoneDictionaryEntry(zoneTypes, section, UpsServiceType.UpsGround, "Ground");
            AddZoneDictionaryEntry(zoneTypes, section, UpsServiceType.UpsNextDayAir, "Next Day Air");
            AddZoneDictionaryEntry(zoneTypes, section, UpsServiceType.Ups2DayAir, "Second Day Air");

            return zoneTypes;
        }

        /// <summary>
        /// Adds the UpsServiceType Zone defined in section.
        /// </summary>
        private static void AddZoneDictionaryEntry(Dictionary<UpsServiceType, string> zoneTypes,
            IRange section,
            UpsServiceType upsServiceType,
            string serviceName)
        {
            string zoneValue = section.Rows.FirstOrDefault(r => r.Cells[0].Value == serviceName)?.Cells[1].Value;
            if (string.IsNullOrWhiteSpace(zoneValue))
            {
                throw new UpsLocalRatingException($"Error reading worksheet {section.Worksheet.Name} \n\n" +
                                                  $"Missing {serviceName} zone for section starting at row {section.Row}.");
            }

            zoneTypes.Add(upsServiceType, zoneValue);
        }
    }
}