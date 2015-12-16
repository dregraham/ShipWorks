using System;
using System.Collections.Generic;
using System.IO;

namespace ShipWorks.Data
{
    /// <summary>
    /// Manage data resources
    /// </summary>
    public interface IDataResourceManager
    {
        /// <summary>
        /// Crate database resource from PDF
        /// </summary>
        IEnumerable<DataResourceReference> CreateFromPdf(Stream pdfStream, long consumerID, string label);

        /// <summary>
        /// Crate database resource from PDF
        /// </summary>
        /// <param name="pdfStream">Stream that contains the pdf data</param>
        /// <param name="consumerID">Id of the consumer</param>
        /// <param name="createLabelFromIndex">Function that creates a label given the index of the page</param>
        /// <param name="getBytesFromStream">Function that gets a byte array from the given image stream</param>
        IEnumerable<DataResourceReference> CreateFromPdf(Stream pdfStream, long consumerID,
            Func<int, string> createLabelFromIndex, Func<MemoryStream, byte[]> getBytesFromStream);

        /// <summary>
        /// Crate database resource from bytes
        /// </summary>
        DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label);
    }
}