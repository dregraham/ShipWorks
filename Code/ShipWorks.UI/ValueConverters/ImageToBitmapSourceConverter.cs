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
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an image to bitmap source so it can be bound
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ImageToBitmapSourceConverter : IValueConverter
    {
        private static Lazy<BitmapSource> defaultImage = new Lazy<BitmapSource>(
            () => BitmapSource.Create(2, 2, 96, 96,
                PixelFormats.Indexed1,
                new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }),
                new byte[] { 0, 0, 0, 0 },
                1));

        private static Dictionary<string, BitmapSource> imageCache = new Dictionary<string, BitmapSource>();

        /// <summary>
        /// Convert an image to a bitmap source for use in an Image control
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is Image image)
            {
                return CreateBitmapSource(image);
            }

            if (value is Icon icon)
            {
                return CreateBitmapSource(icon);
            }

            if (DesignModeDetector.IsDesignerHosted())
            {
                // Just return a generic check mark
                return Imaging.CreateBitmapSourceFromHBitmap(Resources.check16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            if (value is string imageName)
            {
                if (imageCache.TryGetValue(imageName, out BitmapSource cachedImage))
                {
                    return cachedImage;
                }

                var loadedImage = CreateBitmapSource(ResourcesUtility.GetImage(imageName));
                imageCache.Add(imageName, loadedImage);
                return loadedImage;
            }

            return defaultImage.Value;
        }

        /// <summary>
        /// Create a bitmap source from an icon
        /// </summary>
        private object CreateBitmapSource(Icon icon) =>
            Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        /// <summary>
        /// Create a bitmap source from an image
        /// </summary>
        private static BitmapSource CreateBitmapSource(Image image)
        {
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
