using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ShipWorks.UI.Controls
{    
    /// <summary>
    /// Convert length to and from a string
    /// </summary>
    public class LengthConverter 
    {
        // Match any integer or floating point number
        private static string numberRegex = @"-?([0-9]+(\.[0-9]*)?|\.[0-9]+)";

        // Match the case where both pounds and ounces are present
        private static readonly Regex feetInchesRegex = new Regex(
            $@"^(?<Feet>{numberRegex})\s*(feet|foot|ft.|ft|f|')?\s+" +
            $@"(?<Inches>{numberRegex})\s*(inches|inch|in.|in|i|"")?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only ounces is present
        private static readonly Regex inchesRegex = new Regex(
            $@"^(?<Inches>{numberRegex})\s*(inches|inch|in.|in|i|"")?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only pounds is present
        private static readonly Regex feetRegex = new Regex(
            $@"^(?<Feet>{numberRegex})\s*(feet|foot|ft.|ft|f|')\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        /// <summary>
        /// Format the given length to inches
        /// </summary>
        public string FormatLength(double length) => $"{length:0.0#} in";

        /// <summary>
        /// Convert from length string (ft and in) to double
        /// </summary>
        [SuppressMessage("SonarQube", "S2486:Exceptions should not be ignored",
            Justification = "We treat a format exception as just invalid data, so the exception should be eaten")]
        public double? ParseLength(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            try
            {
                // See if both feet and inches are present
                Match feetInchesMatch = feetInchesRegex.Match(value);

                // Did it match
                double? newLengthInInches;
                if (feetInchesMatch.Success)
                {
                    double feet = double.Parse(feetInchesMatch.Groups["Feet"].Value);
                    double inches = double.Parse(feetInchesMatch.Groups["Inches"].Value);

                    newLengthInInches = (feet * 12) + inches;
                }
                else
                {
                    // Now see if just inches are present
                    Match inchesMatch = inchesRegex.Match(value);

                    // Did it match
                    if (inchesMatch.Success)
                    {
                        newLengthInInches = double.Parse(inchesMatch.Groups["Inches"].Value);
                    }
                    else
                    {
                        // Now see if just pounds are present
                        Match feetMatch = feetRegex.Match(value);

                        // Did it match
                        newLengthInInches = feetMatch.Success ? 
                                            (double?) double.Parse(feetMatch.Groups["Feet"].Value) * 12 :
                                            null;
                    }
                }

                // Ensure the range is valid
                if (newLengthInInches.HasValue)
                {
                    return newLengthInInches;
                }
            }
            catch (FormatException)
            {
                // There's nothing to do here; a failed parsed will just return null
            }

            return null;
        }
    }
}