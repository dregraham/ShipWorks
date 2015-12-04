using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Convert weight to and from a string
    /// </summary>
    public class WeightConverter : IWeightConverter
    {
        // Match any integer or floating point number
        static string numberRegex = @"-?([0-9]+(\.[0-9]*)?|\.[0-9]+)";

        // Match the case where both pounds and ounces are present
        static readonly Regex poundsOzRegex = new Regex(
            $@"^(?<Pounds>{numberRegex})\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s+" +
            $@"(?<Ounces>{numberRegex})\s*(ounces|ounce|oz.|oz|o)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only ounces is present
        static readonly Regex ouncesRegex = new Regex(
            $@"^(?<Ounces>{numberRegex})\s*(ounces|ounce|oz.|oz|o)\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only pounds is present
        static readonly Regex poundsRegex = new Regex(
            $@"^(?<Pounds>{numberRegex})\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private readonly static Lazy<IWeightConverter> current =
            new Lazy<IWeightConverter>(() => IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IWeightConverter>>().Value);

        private readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSession"></param>
        public WeightConverter(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        /// <summary>
        /// Get the current instance of the weight converter
        /// </summary>
        /// <remarks>Since most usages of the WeightConverter is from the UI, it makes more sense to have a
        /// singleton than to have each user resolve it manually</remarks>
        public static IWeightConverter Current => current.Value;

        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        public string FormatWeight(double weight) =>
            FormatWeight(weight, WeightDisplayFormat.FractionalPounds);

        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        public string FormatWeight(double weight, WeightDisplayFormat defaultDisplayFormat)
        {
            WeightDisplayFormat displayFormat = (WeightDisplayFormat?) userSession.User?.Settings.ShippingWeightFormat ??
                defaultDisplayFormat;

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

        /// <summary>
        /// Convert from weight String (lbs and oz) to Double
        /// </summary>
        [SuppressMessage("SonarQube", "S2486:Exceptions should not be ignored",
            Justification = "We treat a format exception as just invalid data, so the exception should be eaten")]
        public double? ParseWeight(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            try
            {
                // See if both pounds and ounces are present
                Match poundsOzMatch = poundsOzRegex.Match(value.ToString());

                // Did it match
                double? newWeight = 0.0;
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
                        newWeight = poundsMatch.Success ? (double?) double.Parse(poundsMatch.Groups["Pounds"].Value) : null;
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
    }
}
