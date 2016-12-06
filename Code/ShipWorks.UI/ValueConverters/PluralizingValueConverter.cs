using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Return the parameter, adding an 's' if value is greater than 1
    /// </summary>
    /// <remarks>
    /// This is a very simplistic pluralizer. There are packages that will do a much better job with words that
    /// need something other than an 's' appended, but for the immediate need, that was out of scope.
    /// </remarks>
    public class PluralizingValueConverter : IValueConverter
    {
        /// <summary>
        /// Convert the number into a possibly pluralized string
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long numericValue = System.Convert.ToInt64(value);
            string text = parameter?.ToString() ?? string.Empty;

            return numericValue > 1 ? text + "s" : text;
        }

        /// <summary>
        /// Don't support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back");
        }
    }
}
