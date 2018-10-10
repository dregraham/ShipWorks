using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an int to the corresponding enum value for the given enum type
    /// </summary>
    public class IntToEnumConverter : IValueConverter
    {
        /// <summary>
        /// Convert the int value to an enum
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && parameter != null)
            {
                Enum enumValue = default(Enum);
                if (parameter is Type)
                {
                    enumValue = Enum.Parse((Type) parameter, value.ToString()) as Enum;
                }

                return enumValue;
            }

            return null;
        }

        /// <summary>
        /// Convert the enum back to an int
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && parameter != null)
            {
                int returnValue = 0;

                if (parameter is Type)
                {
                    try
                    {
                        // cant use Enum.TryParse because it requires a type argument and we dont have one here.
                        returnValue = (int) Enum.Parse((Type) parameter, value.ToString());
                    }
                    catch (ArgumentException)
                    {
                        return null;
                    }
                    
                    return returnValue;
                }
            }

            return null;
        }
    }
}