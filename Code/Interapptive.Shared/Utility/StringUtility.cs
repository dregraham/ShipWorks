using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;
using log4net;
using System.Windows.Forms;
using System.Diagnostics;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with strings
    /// </summary>
    public static class StringUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(StringUtility));

        /// <summary>
        /// Intelligently format the byte count in bytes, kb, or mb
        /// </summary>
        public static string FormatByteCount(long bytes)
        {
            if (Math.Abs(bytes) < 1024)
            {
                return string.Format("{0:#,##0} Bytes", bytes);
            }

            if (Math.Abs(bytes) < 1024 * 1024 * 20)
            {
                return string.Format("{0:#,##0} KB", bytes / 1024);
            }

            if (Math.Abs(bytes) < 1024 * 1024 * 1024)
            {
                return string.Format("{0:#,##0.0} MB", bytes / (double) (1024 * 1024));
            }

            return string.Format("{0:#,##0.00} GB", bytes / (double) (1024 * 1024 * 1024));
        }

        /// <summary>
        /// Helper to get an encoding instance for ISO-8859-1
        /// </summary>
        public static Encoding Iso8859Encoding
        {
            get
            {
                return Encoding.GetEncoding("ISO-8859-1");
            }
        }

        /// <summary>
        /// Format a friendly date time (i.e. 'Today') using the system local for the time (or date if its not a describable date)
        /// </summary>
        public static string FormatFriendlyDateTime(DateTime utcTime)
        {
            DateTime local = utcTime.ToLocalTime();
            string timeText;

            if (local.Date == DateTime.Now.Date)
            {
                timeText = string.Format("Today {0:t}", local);
            }

            else if (local.Date == DateTime.Now.AddDays(-1).Date)
            {
                timeText = string.Format("Yesterday {0:t}", local);
            }

            else
            {
                timeText = local.ToString("g");
            }

            return timeText;
        }

        /// <summary>
        /// Truncate the given string to the specified length.  If it's already less than or equal to the given length, nothing is done.
        /// </summary>
        public static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength);
        }
    }
}
