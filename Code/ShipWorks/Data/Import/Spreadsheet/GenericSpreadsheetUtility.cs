using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Utility class with useful stuff for the spreadsheet import stuff
    /// </summary>
    public static class GenericSpreadsheetUtility
    {
        /// <summary>
        /// Common date\time formats
        /// </summary>
        public static string[] CommonDateTimeFormats
        {
            get
            {
                return new string[]
                    {
                        "Automatic",
                        "M/d/y H:m",
                        "M/d/y h:m:s",
                        "M/d/y h:mm:ss tt",
                        "MMM d, y h:m:s",
                        "yyyy-M-d HH:mm:ss"
                    };
            }
        }

        /// <summary>
        /// Common date formats
        /// </summary>
        public static string[] CommonDateFormats
        {
            get
            {
                return new string[]
                    {
                        "Automatic",
                        "M/d/y",
                        "MMM d, y",
                        "yyyy-M-d"
                    };
            }
        }

        /// <summary>
        /// Common date\time formats
        /// </summary>
        public static string[] CommonTimeFormats
        {
            get
            {
                return new string[]
                    {
                        "Automatic",
                        "H:m",
                        "h:m:s",
                        "HH:mm:ss",
                        "h:mm:ss tt"
                    };
            }
        }

        /// <summary>
        /// Check to see if the new schema removes any of the given utilized columns
        /// </summary>
        public static bool CheckForRemovedColumns(IWin32Window owner, GenericSpreadsheetSourceSchema newSourceSchema, List<string> utilizedColumns)
        {
            // See which no longer exist
            List<string> removedColumns = new List<string>();
            foreach (string utilized in utilizedColumns)
            {
                if (!newSourceSchema.Columns.Any(c => c.Name == utilized))
                {
                    removedColumns.Add(utilized);
                }
            }

            if (removedColumns.Count > 0)
            {
                var warnResult = MessageHelper.ShowQuestion(owner, MessageBoxIcon.Warning,
                    "Some of the columns used by the map were not found in your updated source file.  If you continue any mappings using those columns will be broken.\n\n" +
                    "The missing columns are:\n    " + string.Join(", ", removedColumns) + "\n\n" +
                    "Continue?");

                if (warnResult != DialogResult.OK)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
