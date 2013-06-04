using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Date and time settings for a map
    /// </summary>
    public class GenericSpreadsheetMapDateSettings
    {
        string dateTimeFormat = "Automatic";
        string dateFormat = "Automatic";
        string timeformat = "Automatic";

        GenericSpreadsheetTimeZoneAssumption timezoneAssumption = GenericSpreadsheetTimeZoneAssumption.Local;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetMapDateSettings()
        {

        }

        /// <summary>
        /// The format used when date and time are in the same column
        /// </summary>
        public string DateTimeFormat
        {
            get { return dateTimeFormat; }
            set { dateTimeFormat = value; }
        }

        /// <summary>
        /// Format used when a date is in it's own column
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        /// <summary>
        /// Format used when a time is in it's own column
        /// </summary>
        public string TimeFormat
        {
            get { return timeformat; }
            set { timeformat = value; }
        }

        /// <summary>
        /// The TimeZone date\times are assumed to be in if not specified in the source value
        /// </summary>
        public GenericSpreadsheetTimeZoneAssumption TimeZoneAssumption
        {
            get { return timezoneAssumption; }
            set { timezoneAssumption = value; }
        }
    }
}
