using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Invert a boolean value
    /// </summary>
    [SuppressMessage("SonarLint", "S4144",
        Justification = "Convert and ConvertBack are the same because the reverse of an invert is to invert again")]
    public class InvertBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Convert a value to its opposite value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            throw new InvalidOperationException($"{value} is not a boolean");
        }

        /// <summary>
        /// Convert a value to its opposite value
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            throw new InvalidOperationException($"{value} is not a boolean");
        }
    }
}
