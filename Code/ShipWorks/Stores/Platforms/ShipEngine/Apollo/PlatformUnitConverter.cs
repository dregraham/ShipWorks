using System;
using Interapptive.Shared.Enums;

namespace ShipWorks.Stores.Platforms.ShipEngine.Apollo
{
    /// <summary>
    /// Platform unit converter
    /// </summary>
    public static class PlatformUnitConverter
    {
        /// <summary>
        /// Convert from Platform OrderSourceWeightUnit to WeightUnitOfMeasure
        /// </summary>
        public static WeightUnitOfMeasure FromPlatformWeight(OrderSourceWeightUnit platformWeight)
        {
            switch (platformWeight)
            {
                case OrderSourceWeightUnit.Gram:
                    return WeightUnitOfMeasure.Grams;
                    break;
                case OrderSourceWeightUnit.Kilogram:
                    return WeightUnitOfMeasure.Kilograms;
                    break;
                case OrderSourceWeightUnit.Ounce:
                    return WeightUnitOfMeasure.Ounces;
                    break;
                case OrderSourceWeightUnit.Pound:
                    return WeightUnitOfMeasure.Pounds;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platformWeight), platformWeight, null);
            }
        }
        /// <summary>
        /// Convert dimensions
        /// </summary>
        public static double ConvertDimension(decimal value, OrderSourceDimensionUnit unit)
        {
            switch (unit)
            {
                case OrderSourceDimensionUnit.Inch:
                    return (double) value;
                case OrderSourceDimensionUnit.Foot:
                    return ConvertFromFeetToInches((double) value);
                case OrderSourceDimensionUnit.Centimeter:
                    return ConvertFromCentimetersToInches((double) value);
                case OrderSourceDimensionUnit.Meter:
                    return ConvertFromMetersToInches((double) value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        /// <summary>
        /// Converts the value in kilograms to pounds.
        /// </summary>
        public static decimal ConvertFromKilogramsToPounds(decimal value) => value * 2.20462262m;

        /// <summary>
        /// Converts the value in kilograms to pounds.
        /// </summary>
        public static double ConvertFromKilogramsToPounds(double value) => value * 2.20462262;

        /// <summary>
        /// Converts the value in grams to pounds.
        /// </summary>
        public static decimal ConvertFromGramsToPounds(decimal value) => value * 0.00220462262m;

        /// <summary>
        /// Converts the value in grams to pounds.
        /// </summary>
        public static double ConvertFromGramsToPounds(double value) => value * 0.00220462262;

        /// <summary>
        /// Converts the value in ounces to pounds.
        /// </summary>
        public static decimal ConvertFromOuncesToPounds(decimal value) => value / 16m;

        /// <summary>
        /// Converts the value in ounces to pounds.
        /// </summary>
        public static double ConvertFromOuncesToPounds(double value) => value / 16;

        /// <summary>
        /// Converts the value in centimeters to inches.
        /// </summary>
        public static double ConvertFromCentimetersToInches(double value) => value * 0.393701;

        /// <summary>
        /// Converts the value in feet to inches
        /// </summary>
        public static double ConvertFromFeetToInches(double value) => value * 12;

        /// <summary>
        /// Converts the value in meters to inches
        /// </summary>
        public static double ConvertFromMetersToInches(double value) => value * 39.3701;
    }
}
