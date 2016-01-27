using System;
using System.Collections.Generic;
using System.IO;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Interface for PDF document manipulation.
    /// </summary>
    public interface IPdfDocument
    {
        /// <summary>
        /// Convert each page of a PDF stream into images that are individually passed to the save function
        /// </summary>
        /// <remarks>This isn't a very object-oriented way of handling PDF to image conversion, but it allows
        /// us to keep the amount of memory in use as low as we can.</remarks>
        IEnumerable<T> SavePages<T>(Stream inputPdfStream, Func<MemoryStream, int, T> savePageFunction);
    }
}
