using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts a bool to a wait cursor
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class BooleanToWaitCursorConverter : MarkupExtension, IValueConverter
    {
        private static readonly BooleanToWaitCursorConverter instance = new BooleanToWaitCursorConverter();

        /// <summary>
        /// Returns a wait cursor if the value is true
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return Cursors.Wait;
            }

            return Cursors.Arrow;
        }

        /// <summary>
        /// cannot convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Needed so that the converter does not need to be defined as a static resource 
        /// </summary>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance;
        }
    }
}