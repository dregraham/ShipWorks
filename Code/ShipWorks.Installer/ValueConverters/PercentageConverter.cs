using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Converts the given size value into a percentage
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        /// <summary>
        /// Given the value return the percentage in parameter as a double
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);

        /// <summary>
        /// Do not convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
