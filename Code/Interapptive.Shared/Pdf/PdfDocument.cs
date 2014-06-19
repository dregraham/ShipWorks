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
    /// Implementation of IPdfDocument
    /// </summary>
    public class PdfDocument : IPdfDocument, IDisposable
    {
        private Document pdfDocument;
        private List<Stream> images = new List<Stream>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pdf">Stream representation of the pdf</param>
        public PdfDocument(Stream pdf)
        {
            pdfDocument = new Document(pdf);
        }

        /// <summary>
        /// Iterates through each page of the PDF and converts each page to an image.
        /// </summary>
        /// <returns>List of streams, for each page image.</returns>
        public IEnumerable<Stream> ToImages()
        {
            // Iterate through each page, rendering each to an image
            foreach (Page currentPage in pdfDocument.Pages)
            {
                // We use original page's width and height for image as well as default rendering settings
                using (Bitmap bitmap = currentPage.Render((int)currentPage.Width, (int)currentPage.Height, new RenderingSettings()))
                {
                    Stream imageMemoryStream = new MemoryStream();
                    bitmap.Save(imageMemoryStream, ImageFormat.Png);

                    images.Add(imageMemoryStream);
                }
            }

            return images;
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
