using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Visible if not nulls
    /// </summary>
    public class NullVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If its null return hidden otherwise visible
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            value == null ? Visibility.Hidden : Visibility.Visible;
        /// <summary>
        /// Do not convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}