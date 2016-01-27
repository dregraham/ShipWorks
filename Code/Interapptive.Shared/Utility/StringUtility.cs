using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using log4net;

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
    }
}
