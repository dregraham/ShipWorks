using Interapptive.Shared.Business;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    public class DoubleToWeightStringConverter : IValueConverter
    {
        // Match any integer or floating point number
        static string numberRegex = @"-?([0-9]+(\.[0-9]*)?|\.[0-9]+)";

        // Match the case where both pounds and ounces are present
        Regex poundsOzRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s+" +
            @"(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only ounces is present
        Regex ouncesRegex = new Regex(
            @"^(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only pounds is present
        Regex poundsRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        /// <summary>
        /// Convert from Double to weight String (lbs and oz)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                throw new InvalidOperationException("Value is not a double");
            }

            return FormatWeight((double)value, UserSession.User == null ? WeightDisplayFormat.FractionalPounds : (WeightDisplayFormat)UserSession.User.Settings.ShippingWeightFormat);
        }
        
        /// <summary>
        /// Convert from weight String (lbs and oz) to Double
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? newWeight = 0.0;

            try
            {
                // See if both pounds and ounces are present
                Match poundsOzMatch = poundsOzRegex.Match(value.ToString());

                // Did it match
                if (poundsOzMatch.Success)
                {
                    double pounds = double.Parse(poundsOzMatch.Groups["Pounds"].Value);
                    double ounces = double.Parse(poundsOzMatch.Groups["Ounces"].Value);

                    newWeight = pounds + ounces / 16.0;
                }

                else
                {
                    // Now see if just ounces are present
                    Match ouncesMatch = ouncesRegex.Match(value.ToString());

                    // Did it match
                    if (ouncesMatch.Success)
                    {
                        newWeight = double.Parse(ouncesMatch.Groups["Ounces"].Value) / 16.0;
                    }

                    else
                    {
                        // Now see if just pounds are present
                        Match poundsMatch = poundsRegex.Match(value.ToString());

                        // Did it match
                        if (poundsMatch.Success)
                        {
                            newWeight = double.Parse(poundsMatch.Groups["Pounds"].Value);
                        }
                        else
                        {
                            // Nothing worked!
                            newWeight = null;
                        }
                    }
                }

                // Ensure the range is valid
                if (newWeight.HasValue)
                {
                    return newWeight;
                }
            }
            catch (FormatException)
            {
                // There's nothing to do here; a failed parsed will just return null
            }

            return null;
        }
        
        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        public static string FormatWeight(double weight, WeightDisplayFormat displayFormat)
        {
            string result;

            if (displayFormat == WeightDisplayFormat.FractionalPounds)
            {
                result = string.Format("{0:0.0#} lbs", weight);
            }
            else
            {
                WeightValue weightValue = new WeightValue(weight);

                result = string.Format("{0} lbs  {1} oz", weightValue.PoundsOnly, Math.Round(weightValue.OuncesOnly, 1, MidpointRounding.AwayFromZero));
            }

            return result;
        }
    }
}
