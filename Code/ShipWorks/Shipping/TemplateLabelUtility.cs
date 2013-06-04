using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ShipWorks.ApplicationCore;
using System.IO;
using System.Drawing.Imaging;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for generation of template labels
    /// </summary>
    public static class TemplateLabelUtility
    {
        /// <summary>
        /// Generate a rotated version of the label found at the specified path
        /// </summary>
        public static string GenerateRotatedLabel(RotateFlipType rotate, string originalPath)
        {
            string rotateName = Enum.GetName(typeof(RotateFlipType), rotate);

            // For some reason, the "GetName" doesnt always return the right thing for this.  Probably
            // because its a flags enum.
            if (rotate == RotateFlipType.Rotate90FlipNone)
            {
                rotateName = "rot90";
            }
            if (rotate == RotateFlipType.Rotate270FlipNone)
            {
                rotateName = "rot270";
            }

            string filename = Path.GetFileNameWithoutExtension(originalPath);
            string extension = Path.GetExtension(originalPath);
            string newFilename = filename + "-" + rotateName + extension;

            // If we have already create this image, dont do it again
            if (File.Exists(Path.Combine(DataPath.CurrentResources, newFilename)))
            {
                return newFilename;
            }

            // Load the original image and rotate it
            using (Image image = Image.FromFile(Path.Combine(DataPath.CurrentResources, originalPath)))
            {
                image.RotateFlip(rotate);

                // Save it out to the file
                using (FileStream fileStream = new FileStream(Path.Combine(DataPath.CurrentResources, newFilename), FileMode.Create))
                {
                    image.Save(fileStream, GetImageFormat(extension));
                }
            }

            return newFilename;
        }

        /// <summary>
        /// Get the image format to save as given the specified extension
        /// </summary>
        private static ImageFormat GetImageFormat(string extension)
        {
            switch (extension)
            {
                case ".gif": 
                    return ImageFormat.Gif;

                case ".png": 
                    return ImageFormat.Png;
                
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
            }

            throw new InvalidOperationException("Unhandled extension saving label images. " + extension);
        }

        /// <summary>
        /// Get the file extension to use for teh given image format
        /// </summary>
        public static string GetFileExtension(ImageFormat imageFormat)
        {
            if (imageFormat == ImageFormat.Png)
            {
                return "png";
            }

            if (imageFormat == ImageFormat.Gif)
            {
                return "gif";
            }

            if (imageFormat == ImageFormat.Jpeg)
            {
                return "jpg";
            }

            throw new InvalidOperationException("Unhandled imageFormat saving label images. " + imageFormat);
        }
    }
}
