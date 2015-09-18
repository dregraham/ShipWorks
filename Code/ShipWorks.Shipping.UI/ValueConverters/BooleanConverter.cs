using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean into another type
    /// </summary>
    public class BooleanConverter<T> : IValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        /// <summary>
        /// Value to use for true
        /// </summary>
        public T True { get; set; }

        /// <summary>
        /// Value to use for false
        /// </summary>
        public T False { get; set; }

        /// <summary>
        /// Convert a boolean into the requested values
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool && ((bool)value) ? True : False;

        /// <summary>
        /// Convert a value back to boolean
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is T && EqualityComparer<T>.Default.Equals((T)value, True);
    }
}
