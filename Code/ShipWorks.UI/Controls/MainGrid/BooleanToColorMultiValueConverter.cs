using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace ShipWorks.UI.Controls.MainGrid
{
    /// <summary>
    /// Boolean to Color multi value converter
    /// </summary>
    [Obfuscation]
    public class BooleanToColorMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Color to use for true
        /// </summary>
        public Color True { get; set; }

        /// <summary>
        /// Color to use for false
        /// </summary>
        public Color False { get; set; }

        /// <summary>
        /// Return the opposite value based on criteria.
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Convert multiple boolean values to a color
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values?.OfType<bool>().Any(x => x) == true ? True : False;

        /// <summary>
        /// Convert back
        /// </summary>
        /// <remarks>
        /// This is not supported
        /// </remarks>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
