using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Stores;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum to an description
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Convert an enum value into an enum description using the EnumHelper
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                // Just return a generic string
                return "Enum Description";
            }

            return EnumHelper.GetDescription((Enum) value);
        }

        /// <summary>
        /// Converts the description back to an enum value
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
