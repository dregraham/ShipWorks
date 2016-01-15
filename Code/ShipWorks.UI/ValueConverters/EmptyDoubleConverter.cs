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
            double.TryParse(parameter.ToString(), out defaultValue);

            if (!(value is double))
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
            double result = 0D;

            if (string.IsNullOrEmpty(value as string))
            {
                return result; 
            }

            double.TryParse(value.ToString(), out result);

            return result;
        }
    }
}
