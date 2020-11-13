using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ShipWorks.Installer.Enums;

namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Convert NavBarState enum to Visibility
    /// </summary>
    public class NavigationPageTypeToFontWeightConverter : IValueConverter
    {
        /// <summary>
        /// Convert from NavigationPageType to FontWeight
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
                      (NavigationPageType) value == (NavigationPageType) parameter ? FontWeights.Bold : FontWeights.Normal;

        /// <summary>
        /// Convert from NavigationPageType to FontWeight
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
