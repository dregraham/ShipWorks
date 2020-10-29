using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FontAwesome5;

namespace ShipWorks.Installer.ValueConverters
{
    public class IconToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convert from an EFontAwesomeIcon to a brush color
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (EFontAwesomeIcon) value == EFontAwesomeIcon.Solid_ExclamationCircle ? Brushes.Red : Brushes.Green;


        /// <summary>
        /// Converting back is not supported
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
