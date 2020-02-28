using System;
using System.Collections.Generic;
using System.IO;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.Pdf;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wrapper around the data resource manager
    /// </summary>
    public class DataResourceManagerWrapper : IDataResourceManager
    {
        private readonly IPdfDocumentFactory pdfDocumentFactory;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DataResourceManagerWrapper(IPdfDocumentFactory pdfDocumentFactory)
        {
            this.pdfDocumentFactory = pdfDocumentFactory;
        }

        /// <summary>
        /// Saves PDF to database
        /// </summary>
        [NDependIgnoreTooManyParams]
        public IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID, string label, bool forceCreateNew = false) =>
            CreateFromPdf(pdfDocumentType, pdfStream, consumerID, i => i == 0 ? label : $"{label}-{i}", s => s.ToArray(), forceCreateNew);

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public IEnumerable<DataResourceReference> CreateFromPdf(PdfDocumentType pdfDocumentType, Stream pdfStream, long consumerID,
            Func<int, string> createLabelFromIndex, Func<MemoryStream, byte[]> getBytesFromStream, bool forceCreateNew = false)
        {
            IPdfDocument pdfDocument = pdfDocumentFactory.Create(pdfDocumentType);

            //// We need to convert the PDF into images and register each image as a resource in the database
            return pdfDocument.SavePages(pdfStream, (imageStream, index) =>
            {
                string label = createLabelFromIndex(index);
                byte[] bytes = getBytesFromStream(imageStream);

                return DataResourceManager.CreateFromBytes(bytes, consumerID, label, forceCreateNew);
            });
        }

        /// <summary>
        /// Saves Image to database
        /// </summary>
        public DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label, bool forceCreateNew = false) =>
            DataResourceManager.CreateFromBytes(data, consumerID, label, forceCreateNew);

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        public DataResourceReference CreateFromText(string text, long consumerID, bool forceCreateNew = false) =>
            DataResourceManager.CreateFromText(text, consumerID, forceCreateNew);

        /// <summary>
        /// Get all the resource referenced by the consumer, but the local cached data files will not yet be loaded
        /// </summary>
        public List<DataResourceReference> GetConsumerResourceReferences(long consumerID) =>
            DataResourceManager.GetConsumerResourceReferences(consumerID);
    }
}
