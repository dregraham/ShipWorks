using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ShipWorks.UI.Controls.Weight
{
    /// <summary>
    /// Validate a weight entered with lbs or oz
    /// </summary>
    public class WeightValidationRule : ValidationRule
    {
        // Match any integer or floating point number
        static string numberRegex = @"-?([0-9]+(\.[0-9]*)?|\.[0-9]+)";

        // Match the case where both pounds and ounces are present
        readonly Regex poundsOzRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s+" +
            @"(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only ounces is present
        readonly Regex ouncesRegex = new Regex(
            @"^(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only pounds is present
        readonly Regex poundsRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        /// <summary>
        /// Perform the validation
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string valueAsString = value as string;

            // Check to see if value is not null and if it matches one of the tree Regex from above
            // if we find a match then the validation rule is satisfied and returns true
            // if no match is found or value is null we return false
            bool result = valueAsString != null &&
                (poundsOzRegex.Match(valueAsString).Success ||
                ouncesRegex.Match(valueAsString).Success ||
                 poundsRegex.Match(valueAsString).Success);

            // Weight is valid, check to see if someone tried to be smart
            // and enter a negative number
            if (result && valueAsString.Contains("-"))
            {
                return new ValidationResult(false, "The weight cannot be negative.");
            }

            return new ValidationResult(result, $"{valueAsString} is not a valid weight");
        }
    }
}