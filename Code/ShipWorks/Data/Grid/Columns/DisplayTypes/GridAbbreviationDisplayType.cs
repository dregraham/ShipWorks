using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying state\country codes\names
    /// </summary>
    public abstract class GridAbbreviationDisplayType : GridColumnDisplayType
    {
        AbbreviationFormat format = AbbreviationFormat.Full;

        /// <summary>
        /// How to display, or not to display, the state abbreviation
        /// </summary>
        public AbbreviationFormat AbbreviationFormat
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Get the display text to use
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            string code = value.ToString();
            string text = FormatAbbreviation(code, null);

            return text;
        }

        /// <summary>
        /// Format the given code based on the given code
        /// </summary>
        protected string FormatAbbreviation(string code, object customData)
        {
            string full = GetFullName(code, customData);

            if (full == code)
            {
                return code;
            }

            switch (format)
            {
                case AbbreviationFormat.Abbreviated:
                    return code;

                case AbbreviationFormat.AbbreviatedFull:
                    return string.Format("{0} ({1})", code, full);

                case AbbreviationFormat.Full:
                    return full;

                case AbbreviationFormat.FullAbbreviated:
                    return string.Format("{0} ({1})", full, code);
            }

            Debug.Fail("Invalid AbbreviationFormat");
            return code;
        }

        /// <summary>
        /// Get the Full display text for the given abbreviation
        /// </summary>
        protected abstract string GetFullName(string abbreviation, object customData);
    }
}
