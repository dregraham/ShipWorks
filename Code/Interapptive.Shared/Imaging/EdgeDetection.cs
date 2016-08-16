using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Imaging
{
    /// <summary>
    /// Used for image edge detection
    /// pulled from http://stackoverflow.com/questions/16583742/crop-image-white-space-in-c-sharp
    /// </summary>
    public static class EdgeDetection
    {
        /// <summary>
        /// Crops a bitmap based on edge detection
        /// Only works with Black and White images
        /// </summary>
        public static Bitmap CropImageStream(this Stream stream)
        {
            using (Bitmap image = new Bitmap(stream))
            {
                return image.Crop();
            }
        }

        /// <summary>
        /// Crops a bitmap based on edge detection
        /// Only works with Black and White images
        /// </summary>
        [NDependIgnoreLongMethod]
        public static Bitmap Crop(this Bitmap image)
        {
            //get image data
            int[] imagePixels;
            BitmapData bitmapData = CreateBitmapData(image, out imagePixels);

            int left = bitmapData.Width;
            int top = bitmapData.Height;
            int right = 0;
            int bottom = 0;

            //determine top
            for (int i = 0; i < imagePixels.Length; i++)
            {
                int color = imagePixels[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int row = i / bitmapData.Width;
                    int column = i % bitmapData.Width;

                    if (left > column)
                    {
                        left = column;
                    }
                    if (right < column)
                    {
                        right = column;
                    }
                    bottom = row;
                    top = row;
                    break;
                }
            }

            //determine bottom
            for (int i = imagePixels.Length - 1; i >= 0; i--)
            {
                int color = imagePixels[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int row = i / bitmapData.Width;
                    int column = i % bitmapData.Width;

                    if (left > column)
                    {
                        left = column;
                    }
                    if (right < column)
                    {
                        right = column;
                    }
                    bottom = row;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int row = top + 1; row < bottom; row++)
                {
                    //determine left
                    for (int column = 0; column < left; column++)
                    {
                        int color = imagePixels[row * bitmapData.Width + column] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (left > column)
                            {
                                left = column;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int column = bitmapData.Width - 1; column > right; column--)
                    {
                        int color = imagePixels[row * bitmapData.Width + column] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (right < column)
                            {
                                right = column;
                                break;
                            }
                        }
                    }
                }
            }

            int width = right - left + 1;
            int height = bottom - top + 1;

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(imagePixels, r * bitmapData.Width + left, imgData, (r - top) * width, width);
            }

            return CreateCroppedImage(width, height, imgData);
        }

        /// <summary>
        /// Creates the BitmapData used for edge detection
        /// </summary>
        private static BitmapData CreateBitmapData(Bitmap image, out int[] imagePixels)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            imagePixels = new int[image.Height * image.Width];
            Marshal.Copy(bitmapData.Scan0, imagePixels, 0, imagePixels.Length);
            image.UnlockBits(bitmapData);
            return bitmapData;
        }

        /// <summary>
        /// Creates a new image using the cropped image data
        /// </summary>
        private static Bitmap CreateCroppedImage(int width, int height, int[] imgData)
        {
            //create new image
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData newBitmapData = newImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, newBitmapData.Scan0, imgData.Length);
            newImage.UnlockBits(newBitmapData);
            return newImage;
        }
    }
}