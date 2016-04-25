using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
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
            Invert = false;
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
        /// Return the opposite value based on criteria.  
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Convert a boolean into the requested values
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool && ((bool) value)) ^ Invert ? True : False;
        }

        /// <summary>
        /// Convert a value back to boolean
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is T && EqualityComparer<T>.Default.Equals((T) value, True)) ^ Invert;
        }
    }
}
