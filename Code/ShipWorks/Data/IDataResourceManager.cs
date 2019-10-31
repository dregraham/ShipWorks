using System;
using System.Collections.Generic;
using System.IO;
using Interapptive.Shared;
using Interapptive.Shared.Pdf;

namespace ShipWorks.Data
{
    /// <summary>
    /// Manage data resources
    /// </summary>
    public interface IDataResourceManager
    {
        /// <summary>
        /// Create database resource from PDF
        /// </summary>
        [NDependIgnoreTooManyParams]
        IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID, string label, bool forceCreateNew = false);

        /// <summary>
        /// Create database resource from PDF
        /// </summary>
        /// <param name="pdfDocumentType">Type of pdf doc to create</param>
        /// <param name="pdfStream">Stream that contains the pdf data</param>
        /// <param name="consumerID">Id of the consumer</param>
        /// <param name="createLabelFromIndex">Function that creates a label given the index of the page</param>
        /// <param name="getBytesFromStream">Function that gets a byte array from the given image stream</param>
        /// <param name="forceCreateNew">This will create </param>
        [NDependIgnoreTooManyParams]
        IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID,
            Func<int, string> createLabelFromIndex, Func<MemoryStream, byte[]> getBytesFromStream, bool forceCreateNew = false);

        /// <summary>
        /// Create database resource from bytes
        /// </summary>
        DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label, bool forceCreateNew = false);

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        DataResourceReference CreateFromText(string text, long consumerID, bool forceCreateNew = false);
    }
}