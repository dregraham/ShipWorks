using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Invert a boolean value
    /// </summary>
    public class InvertBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Convert a value to its opposite value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }

            throw new InvalidOperationException($"{value} is not a boolean");
        }

        /// <summary>
        /// Convert a value to its opposite value
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }

            throw new InvalidOperationException($"{value} is not a boolean");
        }
    }
}
