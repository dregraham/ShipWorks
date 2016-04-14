using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts an empty value to a usable double value
    /// </summary>
    public class EmptyDoubleConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
       public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double defaultValue = 0D;
            if (!string.IsNullOrWhiteSpace(parameter?.ToString()))
            {
                double.TryParse(parameter.ToString(), out defaultValue);
            }

            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return defaultValue;
            }

            if (Math.Abs(System.Convert.ToDouble(value) - default(double)) < 0.00001)
            {
                return defaultValue;
            }

            return System.Convert.ToDouble(value);
        }

        /// <summary>
        /// Convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack should use the same logic as Convert
            return Convert(value, targetType, parameter, culture);
        }
    }
}
