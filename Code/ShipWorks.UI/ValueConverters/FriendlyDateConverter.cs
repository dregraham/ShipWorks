using System;
using System.Globalization;
using System.Windows.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a date into a friendly displayed version
    /// </summary>
    public class FriendlyDateConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? date = value as DateTime?;
            if (date.HasValue)
            {
                return date.Value.FormatFriendlyDate();
            }

            return value;
        }

        /// <summary>
        /// Convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
