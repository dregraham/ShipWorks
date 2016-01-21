using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts an empty value to a usable money value
    /// </summary>
    public class EmptyMoneyConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
       public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
       {
            if (value == null)
            {
                return 0.ToString("C", CultureInfo.CurrentCulture);
            }

            if (value is double && Math.Abs(System.Convert.ToDouble(value) - default(double)) < 0.00001)
            {
                return 0.ToString("C", CultureInfo.CurrentCulture);
            }

            if (value is decimal && (decimal)value == default(decimal))
            {
                return 0.ToString("C", CultureInfo.CurrentCulture);
            }

            return System.Convert.ToDecimal(value).ToString("C", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal result = 0M;
            string stringValue = value?.ToString();

            if (string.IsNullOrEmpty(stringValue))
            {
                return result; 
            }
            
            decimal.TryParse(stringValue, NumberStyles.Currency, CultureInfo.CurrentCulture, out result);

            return result;
        }
    }
}
