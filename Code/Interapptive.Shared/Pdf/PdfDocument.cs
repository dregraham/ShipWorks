using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Implementation of IPdfDocument that uses Apitron to convert a PDF to an image (TIFF) .
    /// </summary>
    public class PdfDocument : IPdfDocument, IDisposable
    {
        private Document pdfDocument;
        private readonly List<Stream> images = new List<Stream>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfDocument"/> class.
        /// </summary>
        /// <param name="pdf">Stream representation of the PDF.</param>
        public PdfDocument(Stream pdf)
        {
            pdfDocument = new Document(pdf);
        }

        /// <summary>
        /// Iterates through each page of the PDF and converts each page to a PNG image.
        /// </summary>
        /// <returns>List of streams for each page image.</returns>
        public IEnumerable<Stream> ToImages()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                TiffRenderingSettings tiffRenderingSettings = new TiffRenderingSettings(TiffCompressionMethod.CCIT4, 300, 300);
                tiffRenderingSettings.PrinterMode = true;
                tiffRenderingSettings.RenderMode = RenderMode.HighQuality;

                // Use the Apitron component to convert to a TIFF then convert that 
                // to a PNG image that can be used in ShipWorks
                pdfDocument.SaveToTiff(stream, tiffRenderingSettings);                
                images.Add(ConvertToPng(stream.ToArray()));
            }

            return images;
        }

        /// <summary>
        /// Converts the TIFF image to a PNG image.
        /// </summary>
        /// <param name="tiffBytes">The bytes of the tiff image.</param>
        /// <returns>A Stream containing the PNG image data.</returns>
        private Stream ConvertToPng(byte[] tiffBytes)
        {
            using (MemoryStream tiffStream = new MemoryStream(tiffBytes))
            {
                using (Bitmap bitmap = new Bitmap(tiffStream))
                {
                    using (MemoryStream pngStream = new MemoryStream())
                    {
                        bitmap.Save(pngStream, ImageFormat.Png);
                        return new MemoryStream(pngStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Dispose of managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of managed resources if disposing.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (pdfDocument != null)
                {
                    pdfDocument.Dispose();
                    pdfDocument = null;
                }

                if (images != null && images.Any())
                {
                    images.ForEach(image => image.Dispose());
                }
            }
        }
    }
}
