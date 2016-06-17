using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Interapptive.Shared.Utility;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum to an image
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class EnumImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert an enum value into an image for use in an Image control
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                // Just return a generic check mark
                return Imaging.CreateBitmapSourceFromHBitmap(Resources.check16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            Enum enumValue = (Enum) value;
            if (enumValue == null)
            {
                return null;
            }

            Image image = EnumHelper.GetImage(enumValue);
            if (image == null)
            {
                return BitmapSource.Create(2, 2, 96, 96,
                    PixelFormats.Indexed1,
                    new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }),
                    new byte[] { 0, 0, 0, 0 },
                    1);
            }

            using (Bitmap bitmap = new Bitmap(image))
            {
                return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        /// <summary>
        /// Converts the image back to an enum value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
