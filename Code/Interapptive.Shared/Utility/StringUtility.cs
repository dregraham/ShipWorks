using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using log4net;
using System.Security;
using System.Runtime.InteropServices;
using System.Linq;

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
                return string.Format("{0:#,##0.0} MB", bytes / (double)(1024 * 1024));
            }

            return string.Format("{0:#,##0.00} GB", bytes / (double)(1024 * 1024 * 1024));
        }

        /// <summary>
        /// Intelligently format the byte count in bytes, kb, or mb
        /// </summary>
        /// <param name="bytes">Number to format</param>
        /// <param name="decimalFormat">Decimal format, i.e. {0:#,##0}</param>
        public static string FormatByteCount(long bytes, string decimalFormat)
        {
            string format = string.Empty;
            double value = 0;

            if (Math.Abs(bytes) < 1024)
            {
                format = $"{decimalFormat} Bytes";
                value = bytes;
            }
            else if (Math.Abs(bytes) < 1024 * 1024 * 20)
            {
                format = $"{decimalFormat} KB";
                value = (double) bytes / 1024;
            }
            else if (Math.Abs(bytes) < 1024 * 1024 * 1024)
            {
                format = $"{decimalFormat} MB";
                value = (double) bytes/(1024 * 1024);
            }
            else
            {
                format = $"{decimalFormat} GB";
                value = (double) bytes/(1024 * 1024 * 1024);
            }

            return string.Format(format, value);
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
        /// Return a friendly formatted date
        /// </summary>
        public static string FormatFriendlyDate(this DateTime dateTime) =>
            FormatFriendlyDate(dateTime, "d");

        /// <summary>
        /// Return a friendly formatted date
        /// </summary>
        public static string FormatFriendlyDate(this DateTime dateTime, string defaultFormat)
        {
            if (dateTime.Date == DateTime.Now.Date)
            {
                return "Today";
            }

            if (dateTime.Date == DateTime.Now.AddDays(-1).Date)
            {
                return "Yesterday";
            }

            if (dateTime.Date == DateTime.Now.AddDays(1).Date)
            {
                return "Tomorrow";
            }

            return dateTime.ToString(defaultFormat);
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
        /// Formats amount to a currency amount with support for a half penny.
        /// </summary>
        public static string FormatFriendlyCurrency(this decimal amount)
        {
            string formattedAmount = amount.ToString("c");
            decimal truncatedAmount = decimal.Parse(formattedAmount, NumberStyles.Currency);
            if (truncatedAmount - amount == .005M)
            {
                formattedAmount = string.Format("{0}\u00bd", (amount - .005M).ToString("c"));
            }

            return formattedAmount;
        }

        /// <summary>
        /// Truncate the given string to the specified length.  If it's already less than or equal to the given length, nothing is done.
        /// </summary>
        public static string Truncate(this string text, int maxLength)
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

        /// <summary>
        /// Split the given text into as many lines of ideal length of idealLineLength.  If the text is too long to fit in maxLines with lines of length idealLineLength,
        /// then each line will be longer.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static string[] SplitLines(string text, int idealLineLength, int maxLines = Int32.MaxValue)
        {
            if (idealLineLength <= 0)
            {
                throw new ArgumentException("lineLength must be greater than 0", "idealLineLength");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            List<string> lines = new List<string>();

            // Split into words
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder line = new StringBuilder();

            // Create the lines, word by word
            foreach (string word in words)
            {
                // If adding another word would exceed the length, create a new line
                if (line.Length + word.Length > idealLineLength && line.Length != 0)
                {
                    lines.Add(line.ToString());
                    line = new StringBuilder();
                }

                if (line.Length > 0)
                {
                    line.Append(" ");
                }

                line.Append(word);
            }

            // Add the final line
            if (line.Length > 0)
            {
                lines.Add(line.ToString());
            }

            // If there are too many lines, we have to use a different algorithm based on the text and max lines
            if (lines.Count > maxLines)
            {
                lines.Clear();
                line = new StringBuilder();

                int targetLineLength = (int) Math.Ceiling((double) text.Length / (double) maxLines);

                // Create the lines, word by word
                foreach (string word in words)
                {
                    // If the line length is still less than the target length, we add more.  This makes it so previous lines are typically a bit longer the following lines
                    if (line.Length > targetLineLength && line.Length != 0)
                    {
                        lines.Add(line.ToString());
                        line = new StringBuilder();
                    }

                    if (line.Length > 0)
                    {
                        line.Append(" ");
                    }

                    line.Append(word);
                }

                // Add the final line
                if (line.Length > 0)
                {
                    lines.Add(line.ToString());
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// true if the value parameter occurs within this string.
        /// </summary>
        /// <returns></returns>
        public static bool Contains(this string source, string value, StringComparison comp)
            => source.IndexOf(value, comp) >= 0;

        /// <summary>
        /// Converts a string to a secure string. 
        /// </summary>
        /// <remarks>
        /// This defeats the purporse of using a secure string...
        /// </remarks>
        public static SecureString ToSecureString(this string value)
        {
            SecureString secureString = new SecureString();

            foreach (char charInValue in value)
            {
                secureString.AppendChar(charInValue);
            }

            secureString.MakeReadOnly();

            return secureString;
        }

        /// <summary>
        /// Converts a SecureString to an insecure string
        /// </summary>
        /// <remarks>
        /// This defeats the purporse of using a secure string...
        /// </remarks>
        public static string ToInsecureString(this SecureString value)
        {
            MethodConditions.EnsureArgumentIsNotNull(value);
               
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
