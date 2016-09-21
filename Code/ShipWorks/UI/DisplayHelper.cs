using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI
{
    /// <summary>
    /// Provides utility functions used for display.
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Darken the given color by the specified factor.
        /// </summary>
        public static Color DarkenColor(Color color, double factor)
        {
            return Color.FromArgb(
                (int) Math.Max(Math.Min(color.R - (factor * color.R), 255), 0),
                (int) Math.Max(Math.Min(color.G - (factor * color.G), 255), 0),
                (int) Math.Max(Math.Min(color.B - (factor * color.B), 255), 0));
        }

        /// <summary>
        /// Lighten the given color by the specified factor.
        /// </summary>
        public static Color LightenColor(Color color, double factor)
        {
            return Color.FromArgb(
                (int) Math.Max(Math.Min(color.R + (factor * (255 - color.R)), 255), 0),
                (int) Math.Max(Math.Min(color.G + (factor * (255 - color.G)), 255), 0),
                (int) Math.Max(Math.Min(color.B + (factor * (255 - color.B)), 255), 0));
        }

        /// <summary>
        /// Get the currently active Form, regardless of thread.  If no such Form can be found, null is returned.
        /// </summary>
        public static Form GetActiveForm()
        {
            Form activeForm = Form.ActiveForm;

            // Find an enabled (not blocked by modal) open form
            using (SafeCrossThreadScope callScope = new SafeCrossThreadScope())
            {
                foreach (Form openForm in Application.OpenForms)
                {
                    activeForm = openForm;
                    bool isEnabled = NativeMethods.IsWindowEnabled(activeForm.Handle);

                    if (isEnabled)
                    {
                        break;
                    }
                }
            }

            return activeForm;
        }

        /// <summary>
        /// Returns the cropped image
        /// </summary>
        public static Image CropImage(Image image, int x, int y, int width, int height)
        {
            // Creates a new Image object that will hold the resized Bitmap
            Bitmap cropped = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            cropped.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Creates a Graphics for the new image
            using (Graphics g = Graphics.FromImage(cropped))
            {
                // Sets High Quality Mode
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Our desired rectangle
                Rectangle destRect = new Rectangle(0, 0, width, height);

                // Draw onto the destination image
                g.DrawImage(
                    image,
                    destRect,
                    x, y,
                    width, height,
                    GraphicsUnit.Pixel);
            }

            return cropped;
        }

        /// <summary>
        /// Resize the given image to the new size
        /// </summary>
        ///
        public static Image ResizeImage(Image image, Size size)
        {
            Bitmap newImage = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height));
            }

            return newImage;
        }
    }
}
