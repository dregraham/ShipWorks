using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.IO.Text.Csv;
using System.IO;
using System.Web.Script.Serialization;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv
{
    /// <summary>
    /// Utility class with useful stuff for the CSV\Text import stuff
    /// </summary>
    public static class GenericCsvUtility
    {
        /// <summary>
        /// Get the description to show for the given quote character type
        /// </summary>
        public static string GetCharacterDescription(char quotes)
        {
            switch (quotes)
            {
                case '"': return "Double (\")";
                case '\'': return "Single (')";
                case ',': return "Comma (,)";
                case ';': return "Semicolon (;)";
                case '\\': return "Backslash (\\)";
                case '\t': return "Tab";
                case '\0': return "(None)";
                default: return quotes.ToString();
            }
        }

        /// <summary>
        /// The csv reader we use 'CachedCsvReader' likes to sometimes put the unread buffer into the message. That looks horrible, so we strip it.
        /// </summary>
        public static string StripRawData(string message)
        {
            if (message == null)
            {
                return null;
            }

            int index = message.IndexOf("Current raw data");
            if (index != -1)
            {
                return message.Substring(0, index);
            }
            else
            {
                return message;
            }
        }

        /// <summary>
        /// Get the encoding represented by the given encoding name.  Returns null if the name was blank, but throws if the name is non-blank but invalid.
        /// </summary>
        public static Encoding GetEncoding(string encodingName)
        {
            if (string.IsNullOrWhiteSpace(encodingName))
            {
                return null;
            }

            try
            {
                return Encoding.GetEncoding(encodingName);
            }
            catch (ArgumentException ex)
            {
                throw new GenericSpreadsheetException(string.Format("'{0}' is not a supported encoding.", encodingName), ex);
            }
        }
    }
}
