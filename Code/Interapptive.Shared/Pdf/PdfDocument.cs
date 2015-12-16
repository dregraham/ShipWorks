using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Implementation of IPdfDocument that uses Apitron to convert a PDF to an image (TIFF) .
    /// </summary>
    public class PdfDocument : IPdfDocument
    {
        /// <summary>
        /// Save the pages of a PDF by calling the save function per page
        /// </summary>
        public IEnumerable<T> SavePages<T>(Stream inputPdfStream, Func<MemoryStream, int, T> savePageFunction)
        {
            TiffRenderingSettings tiffRenderingSettings = new TiffRenderingSettings(TiffCompressionMethod.CCIT4, 300, 300);
            tiffRenderingSettings.PrinterMode = true;
            tiffRenderingSettings.RenderMode = RenderMode.HighQuality;

            using (MemoryStream stream = new MemoryStream())
            {
                using (Document doc = new Document(inputPdfStream))
                {
                    // Use the Apitron component to convert to a TIFF then convert that
                    // to a PNG image that can be used in ShipWorks
                    doc.SaveToTiff(stream, tiffRenderingSettings, false);
                }

                Image tiff = Image.FromStream(stream);

                Guid objGuid = tiff.FrameDimensionsList[0];
                FrameDimension dimension = new FrameDimension(objGuid);
                int pageCount = tiff.GetFrameCount(dimension);

                return Enumerable.Range(0, pageCount)
                    .Select(i => SavePage(i, tiff, dimension, savePageFunction))
                    .ToList();
            }
        }

        /// <summary>
        /// Save an individual page
        /// </summary>
        private T SavePage<T>(int pageNumber, Image tiff, FrameDimension dimension,
            Func<MemoryStream, int, T> savePageFunction)
        {
            tiff.SelectActiveFrame(dimension, pageNumber);
            T result;

            using (MemoryStream pngStream = new MemoryStream())
            {
                tiff.Save(pngStream, ImageFormat.Png);

                result = savePageFunction(pngStream, pageNumber);
            }

            // Saving labels from PDFs use a lot of memory because of multiple memory streams and bitmaps
            // being created. Calling collect after each label is saved lets us clean up all these temporary
            // bitmaps before moving on to the next one.
            GC.Collect();

            return result;
        }
    }
}
