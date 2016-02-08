using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum to an description
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
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

        /// <summary>
        /// This is so that we dont need to define the converter as a static resource in xaml
        /// </summary>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new InverseBooleanConverter();
        }
    }
}
