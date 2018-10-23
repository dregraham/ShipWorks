using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converter for returning the first not null or empty string in a list
    /// </summary>
    public class FirstNotNullOrEmptyStringMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// Return the first not null or empty string
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
                {
                    return stringValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}