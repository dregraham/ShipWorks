using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converter that collapses element if char is white space
    /// </summary>
    public class CharToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is char character)
            {
                return character == '\0' ? Visibility.Collapsed : Visibility.Visible;
            }

            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}