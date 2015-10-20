using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum to an image
    /// </summary>
    public class EnumImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert an enum value into an image for use in an Image control
        /// </summary>
        //[SuppressMessage("SonarQube", "S2930:\"IDisposables\" should be disposed", 
        //    Justification = "BitmapImage will dispose the stream after loading")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                // Just return a generic check mark
                return Imaging.CreateBitmapSourceFromHBitmap(Resources.check16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            Image image = EnumHelper.GetImage((Enum)value);
            using (Bitmap bitmap = new Bitmap(image))
            {
                return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
