using System;
using System.Collections.Generic;
using System.IO;
using Interapptive.Shared.Pdf;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wrapper around the data resource manager
    /// </summary>
    public class DataResourceManagerWrapper : IDataResourceManager
    {
        private readonly IPdfDocument pdfDocument;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pdfDocument"></param>
        public DataResourceManagerWrapper(IPdfDocument pdfDocument)
        {
            this.pdfDocument = pdfDocument;
        }

        /// <summary>
        /// Saves PDF to database
        /// </summary>
        public IEnumerable<DataResourceReference> CreateFromPdf(Stream pdfStream, long consumerID, string label) =>
            CreateFromPdf(pdfStream, consumerID, i => i == 0 ? label : $"{label}-{i}", s => s.ToArray());

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        public IEnumerable<DataResourceReference> CreateFromPdf(Stream pdfStream, long consumerID,
            Func<int, string> createLabelFromIndex, Func<MemoryStream, byte[]> getBytesFromStream)
        {
            //// We need to convert the PDF into images and register each image as a resource in the database
            return pdfDocument.SavePages(pdfStream, (imageStream, index) =>
            {
                string label = createLabelFromIndex(index);
                byte[] bytes = getBytesFromStream(imageStream);

                return DataResourceManager.CreateFromBytes(bytes, consumerID, label);
            });
        }

        /// <summary>
        /// Saves Image to database
        /// </summary>
        public DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label)
        {
            return DataResourceManager.CreateFromBytes(data, consumerID, label);
        }
    }
}