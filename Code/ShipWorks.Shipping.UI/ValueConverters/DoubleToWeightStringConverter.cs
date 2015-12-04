using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert double to formatted weight text
    /// </summary>
    public class DoubleToWeightStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert from Double to weight String (lbs and oz)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                throw new InvalidOperationException("Value is not a double");
            }

            return WeightConverter.Current.FormatWeight((double) value);
        }

        /// <summary>
        /// Convert from weight String (lbs and oz) to Double
        /// </summary>
        [SuppressMessage("SonarQube", "S2486:Exceptions should not be ignored",
            Justification = "We treat a format exception as just invalid data, so the exception should be eaten")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            WeightConverter.Current.ParseWeight(value as string);
    }
}
