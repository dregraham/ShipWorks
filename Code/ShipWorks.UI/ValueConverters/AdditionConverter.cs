using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converter for adding to numeric bindings
    /// </summary>
    public class AdditionConverter : IValueConverter
    {
        /// <summary>
        /// Adds the parameter to the value if they are numbers
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                double.TryParse(value.ToString(), out double augend);
                double.TryParse(parameter.ToString(), out double addend);
            
                return augend + addend;
            }
            
            return null;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}