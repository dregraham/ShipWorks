using System;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying dates
    /// </summary>
    public class GridDateDisplayType : GridColumnDisplayType
    {
        bool showDate = true;
        string dateFormat = "MM/dd/yyyy";
        bool descriptiveDates = false;

        // How time will be displayed
        TimeDisplayFormat timeFormat = TimeDisplayFormat.Standard;

        /// <summary>
        /// Create the editor used to edit the settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridDateDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the date portion will be shown.
        /// </summary>
        public bool ShowDate
        {
            get { return showDate; }
            set { showDate = value; }
        }

        /// <summary>
        /// Use "Today" and "Yesterday" for the display format
        /// </summary>
        public bool UseDescriptiveDates
        {
            get { return descriptiveDates; }
            set { descriptiveDates = value; }
        }

        /// <summary>
        /// The format string to use for the date
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        /// <summary>
        /// How to display time
        /// </summary>
        public TimeDisplayFormat TimeDisplayFormat
        {
            get { return timeFormat; }
            set { timeFormat = value; }
        }

        /// <summary>
        /// Pre-process the value to convert it to local time before display
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            if (value is DateTime)
            {
                DateTime utcDate = (DateTime) value;
                return utcDate.ToLocalTime();
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Get the text to display based on the formatting options
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            DateTime dateTime = (DateTime) value;

            string datePart = GetDateDisplay(dateTime);
            string timePart = GetTimeDisplay(dateTime);

            if (datePart.Length > 0 && timePart.Length > 0)
            {
                datePart += " ";
            }

            return datePart + timePart;
        }

        /// <summary>
        /// Get the date portion of the display
        /// </summary>
        private string GetDateDisplay(DateTime dateTime)
        {
            if (!showDate)
            {
                return "";
            }

            if (UseDescriptiveDates)
            {
                return dateTime.FormatFriendlyDate(dateFormat);
            }

            return dateTime.ToString(dateFormat);
        }

        /// <summary>
        /// Get the time portion of the display
        /// </summary>
        private string GetTimeDisplay(DateTime dateTime)
        {
            if (timeFormat == TimeDisplayFormat.Standard)
            {
                return dateTime.ToString("h:mm tt");
            }

            if (timeFormat == TimeDisplayFormat.Military)
            {
                return dateTime.ToString("HH:mm");
            }

            return "";
        }

        /// <summary>
        /// The default width for date's is bigger
        /// </summary>
        public override int DefaultWidth
        {
            get
            {
                // Default width for date column
                return 120;
            }
        }
    }
}
