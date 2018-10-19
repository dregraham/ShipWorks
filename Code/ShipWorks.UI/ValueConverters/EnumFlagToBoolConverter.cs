using System;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts UpsEmailNotificationTypes to booleans based on the given value and parameter
    /// </summary>
    public class EnumFlagToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns a boolean value representing whether or not the given flag value contains the flag given as
        /// a converter parameter.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null &&
                value.GetType().IsEnum &&
                parameter != null &&
                parameter.GetType().IsEnum)
            {
                Enum currentFlagValue = (Enum) value;
                Enum flagChanged = (Enum) parameter;

                return currentFlagValue.HasFlag(flagChanged);
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