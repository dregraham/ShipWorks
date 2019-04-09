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
        /// Converting an image to a base64 string.
        /// </summary>
        public static string ImageToBase64String(this Image image, ImageFormat imageFormat)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                ms.Close();
                data = ms.ToArray();
            }

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Converting a base64 string to an image.
        /// </summary>
        public static Image Base64StringToImage(this string base64String)
        {
            byte[] imageBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBuffer, 0, imageBuffer.Length))
            {
                ms.Write(imageBuffer, 0, imageBuffer.Length);
                return Image.FromStream(ms);
            }
        }
    }
}