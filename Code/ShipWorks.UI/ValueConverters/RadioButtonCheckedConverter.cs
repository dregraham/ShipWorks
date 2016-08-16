using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converter for radio buttons binding to a single property
    /// </summary>
    public class RadioButtonCheckedConverter : IValueConverter
    {
        /// <summary>
        /// If the value matches parameter return true (Checked)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        /// <summary>
        /// If the value is true return parameter others do nothing
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}