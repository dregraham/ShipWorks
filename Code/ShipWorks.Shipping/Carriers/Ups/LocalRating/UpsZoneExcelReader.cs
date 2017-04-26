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

        private const string DestinationZipHeader = "Dest. ZIP";
        private const string GroundHeader = "Ground";
        private const string ThreeDaySelectHeader = "3 Day Select";
        private const string SecondDayAirHeader = "2nd Day Air";
        private const string SecondDayAirAMHeader = "2nd Day Air A.M.";
        private const string NextDayAirSaverHeader = "Next Day Air Saver";
        private const string NextDayAirHeader = "Next Day Air";

        private const string InvalidOriginZipErrorMessage = "Invalid origin zip {0}.";
        private const string InvalidDestinationZipErrorMessage = "Worksheet {0} has an invalid destination zip value {1}.";
        private const string MissingServiceHeaderErrorMessage = "{0} is missing the required {1} header.";
        private const string InvalidZoneErrorMessage = "Invalid zone in Worksheet {0}, cell {1}. Zones must be 3 digit numbers.";

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
            
            // looking for any workbook whose name is 5 digits dash 5 digits with optional white space xxxxx-xxxxx
            List<IWorksheet> zipWorkSheets = zoneWorksheets.Cast<IWorksheet>()
                .Where(worksheet => fiveDigitZipRangeRegex.IsMatch(worksheet.Name)).ToList();

            if (!zipWorkSheets.Any())
            {
                throw new UpsLocalRatingException("The zone file contains no worksheets that follow the zone worksheet naming convention of '#####-#####'");
            }

            foreach (IWorksheet worksheet in zipWorkSheets)
            {
                ValidateWorksheet(worksheet);
                ParseLower48Zones(worksheet, zones);
            }

            zones.AddRange(alaskaHawaiiReader.GetAlaskaHawaiiZones(zoneWorksheets));

            upsLocalRateTable.ReplaceZones(zones);
        }


        /// <summary>
        /// Quickly lint the worksheet and make sure that the required rows are present and correct
        /// </summary>
        private static void ValidateWorksheet(IWorksheet worksheet)
        {
            ValidateWorksheetHeaders(worksheet);

            foreach (IRange cell in worksheet.Columns[0].Cells)
            {
                string cellText = cell.Value;
                
                if (cell.EntireRow.IsBlank || cellText == DestinationZipHeader)
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
                throw new UpsLocalRatingException(string.Format(InvalidDestinationZipErrorMessage, worksheet.Name, cell.Value));
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
                    ParseRow(row, zones, originFloor, originCeiling, worksheet.Name);
                }
            }
        }

        /// <summary>
        /// Parse the given row and add it as a zone to the zones collection
        /// </summary>
        private static void ParseRow(IRange row, List<UpsLocalRatingZoneEntity> zones, int originFloor, int originCeiling, string worksheetName)
        {
            int destinationZipCeiling = GetDestinationZipCeiling(row.Cells[0]);
            int destinationZipFloor = GetDestinationZipFloor(row.Cells[0]);

            for (int i = 1; i <= 6; i++)
            {
                string cellValue = row.Cells[i].Value.Trim();

                if (cellValue == "-" || string.IsNullOrWhiteSpace(cellValue))
                {
                    continue;
                }

                if (!threeDigitNumberRegex.IsMatch(cellValue))
                {
                    throw new UpsLocalRatingException(string.Format(InvalidZoneErrorMessage, worksheetName, row.Cells[i].AddressLocal));
                }

                UpsLocalRatingZoneEntity zoneEntity =
                    new UpsLocalRatingZoneEntity
                    {
                        DestinationZipCeiling = destinationZipCeiling,
                        DestinationZipFloor = destinationZipFloor,
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
        private static int GetDestinationZipCeiling(IRange cell)
        {
            string value = cell.Value;
            int result;
            if (int.TryParse($"{value.Substring(value.IndexOf("-", StringComparison.Ordinal) + 1, 3)}99", out result))
            {
                return result;
            }

            throw new UpsLocalRatingException(string.Format(InvalidDestinationZipErrorMessage, value, cell.Worksheet.Name));
        }


        /// <summary>
        /// Get the Destination zip floor from the worksheet
        /// </summary>
        private static int GetDestinationZipFloor(IRange cell)
        {
            string value = cell.Value;
            int result;
            if (int.TryParse($"{value.Substring(0, 3)}00", out result))
            {
                return result;
            }

            throw new UpsLocalRatingException(string.Format(InvalidDestinationZipErrorMessage, value, cell.Worksheet.Name));
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

            throw new UpsLocalRatingException(string.Format(InvalidOriginZipErrorMessage, value));
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

            throw new UpsLocalRatingException(string.Format(InvalidOriginZipErrorMessage, value));
        }

        /// <summary>
        /// Validate that the worksheet contains the required headers.
        /// </summary>
        private static void ValidateWorksheetHeaders(IWorksheet worksheet)
        {
            string worksheetName = worksheet.Name;
            IRange headerRow = worksheet.Rows[0];
            
            ValidateColumnHeader(DestinationZipHeader, 0, headerRow, worksheetName);
            ValidateColumnHeader(GroundHeader, 1, headerRow, worksheetName);
            ValidateColumnHeader(ThreeDaySelectHeader, 2, headerRow, worksheetName);
            ValidateColumnHeader(SecondDayAirHeader, 3, headerRow, worksheetName);
            ValidateColumnHeader(SecondDayAirAMHeader, 4, headerRow, worksheetName);
            ValidateColumnHeader(NextDayAirSaverHeader, 5, headerRow, worksheetName);
            ValidateColumnHeader(NextDayAirHeader, 6, headerRow, worksheetName);
        }

        /// <summary>
        /// Validate that the header string exists in the header row at the given postion 
        /// </summary>
        private static void ValidateColumnHeader(string header, int position, IRange headerRow, string worksheetName)
        {
            try
            {
                if (headerRow.Cells[position].Text != header)
                {
                    throw new UpsLocalRatingException(string.Format(MissingServiceHeaderErrorMessage, worksheetName, NextDayAirHeader));
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new UpsLocalRatingException(string.Format(MissingServiceHeaderErrorMessage, worksheetName, NextDayAirHeader));
            }
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