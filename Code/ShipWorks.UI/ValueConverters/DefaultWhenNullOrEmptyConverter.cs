using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Provide a default value when the input is null or empty
    /// </summary>
    public class DefaultWhenNullOrEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Convert the value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return parameter;
            }

            if (value is string)
            {
                return string.IsNullOrEmpty((string) value) ? parameter : value;
            }

            return value;
        }

        /// <summary>
        /// Don't support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
