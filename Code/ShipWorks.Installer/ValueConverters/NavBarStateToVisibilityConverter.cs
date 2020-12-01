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
    public class NavBarStateToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert from NavBarState to Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NavBarState currentState = (NavBarState) value;
            NavBarState targetState = (NavBarState) parameter;

            return currentState == targetState ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert from Visibility to NavBarState
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
