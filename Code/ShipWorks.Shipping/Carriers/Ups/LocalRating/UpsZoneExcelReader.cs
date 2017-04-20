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
        /// <summary>
        ///  Reads the ups zone excel work sheets and stores the zone info in to the UpsLocalRateTable
        /// </summary>
        public void Read(IWorksheets zoneWorksheets, IUpsLocalRateTable upsLocalRateTable)
        {
            List<UpsLocalRatingZoneEntity> zones = new List<UpsLocalRatingZoneEntity>();

            foreach (IWorksheet worksheet in zoneWorksheets)
            {
                // looking for any workbook whos name is 5 digits dash 5 digits with optional white space xxxxx-xxxxx
                if (Regex.IsMatch(worksheet.Name, "^\\s*[0-9]{5}\\s*-\\s*[0-9]{5}\\s*$"))
                {
                    //ValidateWorksheet(worksheet);
                    ParseHawaiiZones(worksheet, zones);
                } 
            }

            // If the zones list is empty there were no worksheets that match our naming convention
            if (!zones.Any())
            {
                throw new UpsLocalRatingException("The zone file contains no zone worksheets that zone naming convention of '#####-#####'");
            }

            upsLocalRateTable.ReplaceZones(zones);
        }

        /// <summary>
        /// parse Hawaii zones from the worksheet and add them to the zone collection
        /// </summary>
        private static void ParseHawaiiZones(IWorksheet worksheet, List<UpsLocalRatingZoneEntity> zones)
        {
            foreach (IRange row in worksheet.Rows)
            {
                // Find the Hawaii row
                if (row.Cells[0].Text == "HI")
                {
                    // loop each cell
                    for (int i = 1; i <= 6; i++)
                    {
                        // if the cell is not empty it has a zone
                        if (!string.IsNullOrWhiteSpace(row.Cells[i].Text))
                        {
                            // Grab all of the rows after the HI row and before any other HI or AK row
                            foreach (IRange foo in worksheet.Rows.Where(r=> r.Row > row.Row))
                            {
                                // If we hit a new state row break out of this foreach
                                if (foo.Cells[0].Text == "HI" || row.Cells[0].Text == "AK")
                                {
                                    break;
                                }

                                string cellText = foo.Cells[0].Text ?? string.Empty;
                                // If its a zip add the zone
                                if (Regex.IsMatch(cellText, "^\\s*[0-9]{5}\\s*$"))
                                {
                                    foreach (IRange zipCell in foo.Cells)
                                    {
                                        UpsLocalRatingZoneEntity zoneEntity =
                                            new UpsLocalRatingZoneEntity
                                            {
                                                DestinationZipCeiling = Get5DigitZip(zipCell.Value),
                                                DestinationZipFloor = Get5DigitZip(zipCell.Value),
                                                OriginZipCeiling = GetOriginZipCeiling(worksheet.Name),
                                                OriginZipFloor = GetOriginZipFloor(worksheet.Name),
                                                Service = (int) GetServiceType(i),
                                                Zone = row.Cells[i].Value.Trim()
                                            };
                                        zones.Add(zoneEntity);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Quickly lint the worksheet and make sure that the required rows are present and correct
        /// </summary>
        private static void ValidateWorksheet(IWorksheet worksheet)
        {
            foreach (IRange range in worksheet.Rows)
            {
                string cellText = range.Cells[0].Text;

                // Check for the header, Alaska and Hawaii rows
                if (cellText == null || cellText == "Dest. ZIP" || cellText == "HI" || cellText == "AK")
                {
                    continue;
                }

                // Everything else has to be one of the following formats ###-###, ### or #####
                if (Regex.IsMatch(cellText, "^\\s*[0-9]{3}\\s*-\\s*[0-9]{3}\\s*$") || 
                    Regex.IsMatch(cellText, "^\\s*[0-9]{3}\\s*$") || 
                    Regex.IsMatch(worksheet.Name, "^\\s*[0-9]{5}\\s*$"))
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
                if (Regex.IsMatch(row.Cells[0].Value, "^\\s*[0-9]{3}\\s*-\\s*[0-9]{3}\\s*$") || Regex.IsMatch(row.Cells[0].Value, "^\\s*[0-9]{3}\\s*$"))
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
                if (row.Cells[i].Value.Trim() == "-")
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
        /// Get the origin zip floor from the worksheet
        /// </summary>
        private static int GetDestinationZipCeiling(string value)
        {
            int result;
            if (int.TryParse(value.Substring(value.IndexOf("-", StringComparison.Ordinal) + 1, 3), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }


        /// <summary>
        /// Get the origin zip ceiling from the worksheet
        /// </summary>
        private static int GetDestinationZipFloor(string value)
        {
            int result;
            if (int.TryParse(value.Substring(0, 3), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }

        /// <summary>
        /// Get the origin zip floor from the worksheet
        /// </summary>
        private static int GetOriginZipFloor(string value)
        {
            int result;
            if (int.TryParse(value.Substring(value.IndexOf("-", StringComparison.Ordinal) + 1, 5), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid origin zip {value}.");
        }


        /// <summary>
        /// Get the origin zip ceiling from the worksheet
        /// </summary>
        private static int GetOriginZipCeiling(string value)
        {
            int result;
            if (int.TryParse(value.Substring(0, 5), out result))
            {
                return result;
            }

            throw new UpsLocalRatingException($"Invalid destination zip {value}.");
        }

        /// <summary>
        /// Get the origin zip ceiling from the worksheet
        /// </summary>
        private static int Get5DigitZip(string value)
        {
            if (Regex.IsMatch(value, "^\\s*[0-9]{5}\\s*$"))
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    return result;
                }
            }

            throw new UpsLocalRatingException($"Invalid zip {value}.");
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