using System;
using System.Globalization;
using System.Windows.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a decimal into a three digit currency with half cents
    /// </summary>
    public class HalfCentCurrencyConverter : IValueConverter
    {
        /// <summary>
        /// Convert the decimal to display a half cent
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal? decimalValue = value as decimal?;
            return decimalValue.HasValue ? decimalValue.Value.FormatFriendlyCurrency() : string.Empty;
        }

        /// <summary>
        /// Convert the value back to its source type
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Don't support converting back");
        }
    }
}
