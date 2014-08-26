using System.Collections.Generic;
using System.IO;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Iterface for pdf document manipulation.
    /// </summary>
    public interface IPdfDocument
    {
        /// <summary>
        /// Iterates through each page of the PDF and converts each page to an image.
        /// </summary>
        /// <returns>List of streams, for each page image.</returns>
        IEnumerable<Stream> ToImages();
    }
}
