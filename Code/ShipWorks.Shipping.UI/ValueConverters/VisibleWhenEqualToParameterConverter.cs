using ShipWorks.UI.Controls.Design;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Show or hide UI elements based on whether a bound property is equal to the converter parameter
    /// </summary>
    public class VisibleWhenEqualToParameterConverter : IValueConverter
    {
        /// <summary>
        /// Return Visible if the bound value is equal to the converter parameter, else collapsed
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            DesignModeDetector.IsDesignerHosted() || Equals(value, parameter) ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Converting back does not make sense here
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Cannot convert back");
        }
    }
}
