using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Is the value considered empty
    /// </summary>
    public class IsEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Convert from an arbitrary value to a boolean stating whether it is empty
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string);
        }

        /// <summary>
        /// Converting back is not supported
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
