using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension class for working with images
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converting a bitmap image into a base64 string
        /// </summary>
        public static string ImageToBase64String(this Bitmap bitmap, ImageFormat imageFormat)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, imageFormat);
                ms.Close();
                data = ms.ToArray();
            }

            return Convert.ToBase64String(data);
        }
    }
}