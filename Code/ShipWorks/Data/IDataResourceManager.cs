﻿using System;
using System.Collections.Generic;
using System.IO;
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
        IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID, string label);

        /// <summary>
        /// Create database resource from PDF
        /// </summary>
        /// <param name="pdfDocumentType">Type of pdf doc to create</param>
        /// <param name="pdfStream">Stream that contains the pdf data</param>
        /// <param name="consumerID">Id of the consumer</param>
        /// <param name="createLabelFromIndex">Function that creates a label given the index of the page</param>
        /// <param name="getBytesFromStream">Function that gets a byte array from the given image stream</param>
        IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID,
            Func<int, string> createLabelFromIndex, Func<MemoryStream, byte[]> getBytesFromStream);

        /// <summary>
        /// Create database resource from bytes
        /// </summary>
        DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label);

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        DataResourceReference CreateFromText(string text, long consumerID);
    }
}