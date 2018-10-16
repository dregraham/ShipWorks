using System;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts UpsEmailNotificationTypes to booleans based on the given value and parameter
    /// </summary>
    public class UpsEmailNotificationTypeToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns a boolean value representing whether or not the given flag value contains the flag given as
        /// a converter parameter.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is UpsEmailNotificationType existingNotificationType && 
               parameter is UpsEmailNotificationType valueChanged)
            {
                return existingNotificationType.HasFlag(valueChanged);
            }

            return false;
        }

        /// <summary>
        /// Returns the Flag type of the converter parameter
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}