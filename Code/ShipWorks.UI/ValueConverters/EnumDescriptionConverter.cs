using System;
using System.Globalization;
using System.Windows.Data;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum into it's description
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Convert an enum into it's description
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !DesignModeDetector.IsDesignerHosted() ?
                EnumHelper.GetDescription(value as Enum) : "Designer enum description.";
        }

        /// <summary>
        /// Convert a value back to enum
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
