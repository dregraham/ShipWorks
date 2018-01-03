using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Implementation of IPdfColorDocument that uses Apitron to render each page to a PNG.
    /// </summary>
    [KeyedComponent(typeof(IPdfDocument), PdfDocumentType.Color)]
    public class PdfColorDocument : IPdfDocument
    {
        /// <summary>
        /// Save the pages of a PDF by calling the save function per page
        /// </summary>
        public IEnumerable<T> SavePages<T>(Stream inputPdfStream, Func<MemoryStream, int, T> savePageFunction)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (Document doc = new Document(inputPdfStream))
                {
                    return doc.Pages
                        .Select((page, index) => RenderPage(savePageFunction, page, index))
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Render the given page
        /// </summary>
        private T RenderPage<T>(Func<MemoryStream, int, T> savePageFunction, Page page, int index)
        {
            var renderingSettings = new RenderingSettings { RenderMode = RenderMode.HighQuality };
            using (Image bitmap = page.Render(To300Dpi(page.Width), To300Dpi(page.Height), renderingSettings))
            {
                return SavePage(index, bitmap, savePageFunction);
            }
        }

        /// <summary>
        /// Save an individual page
        /// </summary>
        private T SavePage<T>(int pageNumber, Image bitmap, Func<MemoryStream, int, T> savePageFunction)
        {
            T result;

            using (MemoryStream m = new MemoryStream())
            {
                bitmap.Save(m, ImageFormat.Png);
                result = savePageFunction(m, pageNumber);
            }

            // Saving labels from PDFs use a lot of memory because of multiple memory streams and bitmaps
            // being created. Calling collect after each label is saved lets us clean up all these temporary
            // bitmaps before moving on to the next one.
            GC.Collect();

            return result;
        }

        /// <summary>
        /// Convert a 72 DPI measurement to 300
        /// </summary>
        int To300Dpi(double measurement) => (int) ((measurement / 72) * 300);
    }
}
