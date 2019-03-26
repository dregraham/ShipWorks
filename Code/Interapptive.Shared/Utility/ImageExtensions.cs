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
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
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
        /// <param name="base64String"></param>
        public static Image Base64StringToImage(this string base64String)
        {
            byte[] imageBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBuffer, 0, imageBuffer.Length))
            {
                ms.Write(imageBuffer, 0, imageBuffer.Length);
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Validates the size of a user selected image.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="byteLimit"></param>
        /// <returns></returns>
        public static bool ValidateImageSize(this string filename, long byteLimit)
        {
            var imageSize = new FileInfo(filename).Length;
            if (imageSize > byteLimit)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the dimensions of a user selected image.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="widthLimit"></param>
        /// <param name="heightLimit"></param>
        /// <returns></returns>
        public static bool ValidateImageDimensions(this string filename, int widthLimit, int heightLimit)
        {
            using (var image = new Bitmap(filename))
            {
                if (image.Width > widthLimit || image.Height > heightLimit)
                {
                    return false;
                }
            }

            return true;
        }
    }
}