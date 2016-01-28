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
using ShipWorks.Properties;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an image to bitmap source so it can be bound
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ImageToBitmapSourceConverter : IValueConverter
    {
        /// <summary>
        /// Convert an image to a bitmap source for use in an Image control
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                // Just return a generic check mark
                return Imaging.CreateBitmapSourceFromHBitmap(Resources.check16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            Image image = value as Image;
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
        /// Converts the bitmap source back to an image
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
