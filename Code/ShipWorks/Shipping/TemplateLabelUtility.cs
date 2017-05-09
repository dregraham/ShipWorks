using System;
using System.Drawing;
using ShipWorks.ApplicationCore;
using System.IO;
using System.Drawing.Imaging;
using log4net;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for generation of template labels
    /// </summary>
    public static class TemplateLabelUtility
    {
        private static ILog log = LogManager.GetLogger(typeof(TemplateLabelUtility));

        /// <summary>
        /// Generate a rotated version of the label found at the specified path
        /// </summary>
        public static string GenerateRotatedLabel(RotateFlipType rotate, string originalPath)
        {
            string newFilename = string.Empty;

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
            newFilename = filename + "-" + rotateName + extension;
            string fullNewFilenameAndPath = Path.Combine(DataPath.CurrentResources, newFilename);

            // If we have already create this image, dont do it again
            if (File.Exists(fullNewFilenameAndPath))
            {
                return newFilename;
            }

            // Load the original image and rotate it
            using (Image image = Image.FromFile(Path.Combine(DataPath.CurrentResources, originalPath)))
            {
                image.RotateFlip(rotate);

                // Save it out to the file
                SaveImageToDisk(fullNewFilenameAndPath, image, extension);
            }

            return newFilename;
        }

        /// <summary>
        /// Save the image to disk
        /// </summary>
        private static void SaveImageToDisk(string fullFilenameAndPath, Image image, string extension)
        {
            int attempts = 0;
            while (!File.Exists(fullFilenameAndPath) && attempts++ <= 10)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(fullFilenameAndPath, FileMode.Create))
                    {
                        image.Save(fileStream, GetImageFormat(extension));
                    }
                    return;
                }
                catch (IOException ex)
                {
                    log.ErrorFormat($"IOException occurred while trying to save the image to disk.", ex);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat($"Unknown exception occurred while trying to save the image to disk.", ex);
                }
            }
        }

        /// <summary>
        /// Save the image to disk
        /// </summary>
        public static Image LoadImageFromDisk(string fullFilenameAndPath)
        {
            if (!File.Exists(fullFilenameAndPath))
            {
                throw new FileNotFoundException("The image file was not found on disk.", fullFilenameAndPath);
            }

            int attempts = 0;
            Exception lastException = new ArgumentException($"Unable to load image from disk: {fullFilenameAndPath}");

            while (File.Exists(fullFilenameAndPath) && attempts++ <= 10)
            {
                try
                {
                    return Image.FromFile(Path.Combine(DataPath.CurrentResources, fullFilenameAndPath));
                }
                catch (IOException ex)
                {
                    lastException = ex;
                    log.ErrorFormat($"IOException occurred while trying to load the image disk.", ex);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    log.ErrorFormat($"Unknown exception occurred while trying to load the image to disk.", ex);
                }
            }

            throw lastException;
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
