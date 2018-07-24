using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Returns string.Empty or value based on whether a bound property is equal to the converter parameter
    /// </summary>
    public class StringEmptyWhenEqualToParameterConverter : IValueConverter
    {
        /// <summary>
        /// Convert to an empty string if the value is the parameter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsString = value?.ToString().Trim();

            return Equals(valueAsString, parameter?.ToString()) ? string.Empty : valueAsString;
        }

        /// <summary>
        /// Convert to the parameter if the value is an empty string
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value?.ToString().Trim() == string.Empty) ? parameter : value;
    }
}
