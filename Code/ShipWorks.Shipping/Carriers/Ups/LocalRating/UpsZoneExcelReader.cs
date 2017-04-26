using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads the ups zone excel work sheets and stores the zone info in to the UpsLocalRateTable
    /// </summary>
    public class UpsZoneExcelReader : IUpsZoneExcelReader
    {
        private readonly IAlaskaHawaiiZoneExcelReader alaskaHawaiiReader;
        private static readonly Regex fiveDigitZipRangeRegex = new Regex("^\\s*[0-9]{5}\\s*-\\s*[0-9]{5}\\s*$");
        private static readonly Regex threeDigitZipRangeRegex = new Regex("^\\s*[0-9]{3}\\s*-\\s*[0-9]{3}\\s*$");
        private static readonly Regex threeDigitNumberRegex = new Regex("^\\s*[0-9]{3}\\s*$");

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsZoneExcelReader(IAlaskaHawaiiZoneExcelReader alaskaHawaiiReader)
        {
            this.alaskaHawaiiReader = alaskaHawaiiReader;
        }

        /// <summary>
        ///  Reads the ups zone excel work sheets and stores the zone info in to the UpsLocalRateTable
        /// </summary>
        public void Read(IWorksheets zoneWorksheets, IUpsLocalRateTable upsLocalRateTable)
        {
            List<UpsLocalRatingZoneEntity> zones = new List<UpsLocalRatingZoneEntity>();

            foreach (IWorksheet worksheet in zoneWorksheets)
            {
                // looking for any workbook whos name is 5 digits dash 5 digits with optional white space xxxxx-xxxxx
                if (fiveDigitZipRangeRegex.IsMatch(worksheet.Name))
                {
                    ValidateWorksheet(worksheet);
                    ParseLower48Zones(worksheet, zones);
                } 
            }
            
            // If the zones list is empty there were no worksheets that match our naming convention
            if (!zones.Any())
            {
                throw new UpsLocalRatingException("The zone file contains no zone worksheets that zone naming convention of '#####-#####'");
            }

            zones.AddRange(alaskaHawaiiReader.GetAlaskaHawaiiZones(zoneWorksheets));

            upsLocalRateTable.ReplaceZones(zones);
        }

      
        /// <summary>
        /// Quickly lint the worksheet and make sure that the required rows are present and correct
        /// </summary>
        private static void ValidateWorksheet(IWorksheet worksheet)
        {
            foreach (IRange range in worksheet.Rows)
            {
                string cellText = range.Cells[0].Text ?? string.Empty;

                // Check for the header, Alaska and Hawaii rows
                if (string.IsNullOrWhiteSpace(cellText) || cellText == "Dest. ZIP")
                {
                    continue;
                }

                // Everything else has to be one of the following formats ###-###, ### or #####
                if (threeDigitZipRangeRegex.IsMatch(cellText) ||
                    threeDigitNumberRegex.IsMatch(cellText))
                {
                    continue;
                }

                // If we got this far the first column of the row is not valid
                throw new UpsLocalRatingException($"Worksheet {worksheet.Name} has an invalid value {cellText}.");
            }
        }

        /// <summary>
        /// parse zones from the worksheet and add them to the zone collection
        /// </summary>
        private static void ParseLower48Zones(IWorksheet worksheet, List<UpsLocalRatingZoneEntity> zones)
        {
            int originFloor = GetOriginZipFloor(worksheet.Name);
            int originCeiling = GetOriginZipCeiling(worksheet.Name);

            foreach (IRange row in worksheet.Rows)
            {
                // the cell 0 is xxx-xxx or xxx where x is a number
                if (threeDigitZipRangeRegex.IsMatch(row.Cells[0].Value) || 
                    threeDigitNumberRegex.IsMatch(row.Cells[0].Value))
                {
                    ParseRow(row, zones, originFloor, originCeiling);
                }
            }
        }

        /// <summary>
        /// Parse the given row and add it as a zone to the zones collection
        /// </summary>
        private static void ParseRow(IRange row, List<UpsLocalRatingZoneEntity> zones, int originFloor, int originCeiling)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (row.Cells[i].Value.Trim() == "-" || string.IsNullOrWhiteSpace(row.Cells[i].Value.Trim()))
                {
                    continue;
                }
                
                UpsLocalRatingZoneEntity zoneEntity =
                    new UpsLocalRatingZoneEntity
                    {
                        DestinationZipCeiling = GetDestinationZipCeiling(row.Cells[0].Value),
                        DestinationZipFloor = GetDestinationZipFloor(row.Cells[0].Value),
                        OriginZipCeiling = originCeiling,
                        OriginZipFloor = originFloor,
                        Service = (int) GetServiceType(i),
                        Zone = row.Cells[i].Value.Trim()
                    };
                zones.Add(zoneEntity);
            }
        }

        /// <summary>
        /// Get the Destination Zip ceiling from the worksheet
        /// </summary>
        private static int GetDestinationZipCeiling(string value)
        {
            int result;
            if (int.TryParse($"{value.Substring(value.IndexOf("-", StringComparison.Ordinal) + 1, 3)}99", out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }


        /// <summary>
        /// Get the Destination zip floor from the worksheet
        /// </summary>
        private static int GetDestinationZipFloor(string value)
        {
            int result;
            if (int.TryParse($"{value.Substring(0, 3)}00", out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }

        /// <summary>
        /// Get the origin zip ceiling from the worksheet
        /// </summary>
        private static int GetOriginZipCeiling(string value)
        {
            int result;
            if (int.TryParse(value.Substring(value.IndexOf("-", StringComparison.Ordinal) + 1, 5), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid origin zip {value}.");
        }


        /// <summary>
        /// Get the origin zip floor from the worksheet
        /// </summary>
        private static int GetOriginZipFloor(string value)
        {
            int result;
            if (int.TryParse(value.Substring(0, 5), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }

        /// <summary>
        /// Get the service type based on the column position
        /// </summary>
        private static UpsServiceType GetServiceType(int column)
        {
            switch (column)
            {
                case 1:
                    return UpsServiceType.UpsGround;
                case 2:
                    return UpsServiceType.Ups3DaySelect;
                case 3:
                    return UpsServiceType.Ups2DayAir;
                case 4:
                    return UpsServiceType.Ups2DayAirAM;
                case 5:
                    return UpsServiceType.UpsNextDayAirSaver;
                case 6:
                    return UpsServiceType.UpsNextDayAir;
                default:
                    throw new UpsLocalRatingException("Unknown service column");
            }
        }
    }
}