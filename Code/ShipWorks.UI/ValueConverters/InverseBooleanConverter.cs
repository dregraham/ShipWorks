using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum to an description
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class InverseBoolean : IValueConverter
    {
        /// <summary>
        /// Converts a bool to its inverse 
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof (bool))
            {
                throw new InvalidOperationException("Cannot convert non boolean value");
            }

            return !(bool)value;
        }

        /// <summary>
        /// Converts the description back to an enum value
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
